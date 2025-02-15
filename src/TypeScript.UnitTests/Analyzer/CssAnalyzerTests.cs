﻿/*
 * SonarLint for Visual Studio
 * Copyright (C) 2016-2023 SonarSource SA
 * mailto:info AT sonarsource DOT com
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SonarLint.VisualStudio.Core.Analysis;
using SonarLint.VisualStudio.Core.Telemetry;
using SonarLint.VisualStudio.Core;
using SonarLint.VisualStudio.TestInfrastructure;
using SonarLint.VisualStudio.TypeScript.Analyzer;
using SonarLint.VisualStudio.TypeScript.EslintBridgeClient;
using SonarLint.VisualStudio.TypeScript.Rules;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System;
using FluentAssertions;

namespace SonarLint.VisualStudio.TypeScript.UnitTests.Analyzer;

[TestClass]
public class CssAnalyzerTests
{
        private const string ValidFilePath = "some path";
        private const string TelemetryLanguageName = "css";
        private const string AnalyzerName = nameof(CssAnalyzer);

        [TestMethod]
        public void MefCtor_CheckIsExported()
        {
            var eslintBridgeClient = Mock.Of<ICssEslintBridgeClient>();

            var rulesProvider = Mock.Of<IRulesProvider>();
            var rulesProviderFactory = new Mock<IRulesProviderFactory>();
            rulesProviderFactory
                .Setup(x => x.Create("css", Language.Css))
                .Returns(rulesProvider);

            var eslintBridgeAnalyzer = Mock.Of<IEslintBridgeAnalyzer>();
            var eslintBridgeAnalyzerFactory = new Mock<IEslintBridgeAnalyzerFactory>();
            eslintBridgeAnalyzerFactory
                .Setup(x => x.Create(rulesProvider, eslintBridgeClient))
                .Returns(eslintBridgeAnalyzer);

            MefTestHelpers.CheckTypeCanBeImported<CssAnalyzer, IAnalyzer>(
                MefTestHelpers.CreateExport<ICssEslintBridgeClient>(eslintBridgeClient),
                MefTestHelpers.CreateExport<IRulesProviderFactory>(rulesProviderFactory.Object),
                MefTestHelpers.CreateExport<IAnalysisStatusNotifierFactory>(),
                MefTestHelpers.CreateExport<IEslintBridgeAnalyzerFactory>(eslintBridgeAnalyzerFactory.Object),
                MefTestHelpers.CreateExport<ITelemetryManager>(),
                MefTestHelpers.CreateExport<IThreadHandling>());
        }

        [TestMethod]
        public void IsAnalysisSupported_NotTypeScript_False()
        {
            var testSubject = CreateTestSubject();

            var languages = new[] { AnalysisLanguage.CFamily, AnalysisLanguage.Javascript };
            var result = testSubject.IsAnalysisSupported(languages);

            result.Should().BeFalse();
        }

        [TestMethod]
        public void IsAnalysisSupported_HasCss_True()
        {
            var testSubject = CreateTestSubject();

            var languages = new[] { AnalysisLanguage.CFamily, AnalysisLanguage.TypeScript, AnalysisLanguage.CascadingStyleSheets };
            var result = testSubject.IsAnalysisSupported(languages);

            result.Should().BeTrue();
        }

        [TestMethod]
        public async Task ExecuteAnalysisAsync_AlwaysCollectsTelemetry()
        {
            var telemetryManager = new Mock<ITelemetryManager>();
            var client = SetupEslintBridgeAnalyzer(null);

            var testSubject = CreateTestSubject(client.Object, telemetryManager: telemetryManager.Object);
            await testSubject.ExecuteAsync(ValidFilePath, Mock.Of<IIssueConsumer>(), CancellationToken.None);

            telemetryManager.Verify(x => x.LanguageAnalyzed(TelemetryLanguageName), Times.Once);
            telemetryManager.VerifyNoOtherCalls();
        }

    [TestMethod]
    public async Task ExecuteAnalysisAsync_ResponseWithNoIssues_ConsumerNotCalled()
    {
        var issues = Array.Empty<IAnalysisIssue>();
        var eslintBridgeAnalyzer = SetupEslintBridgeAnalyzer(issues);
        var consumer = new Mock<IIssueConsumer>();

        var testSubject = CreateTestSubject(eslintBridgeAnalyzer.Object);
        await testSubject.ExecuteAsync(ValidFilePath, consumer.Object, CancellationToken.None);

        consumer.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task ExecuteAnalysisAsync_ResponseWithIssues_ConsumerCalled()
    {
        var issues = new[] { Mock.Of<IAnalysisIssue>(), Mock.Of<IAnalysisIssue>() };
        var eslintBridgeAnalyzer = SetupEslintBridgeAnalyzer(issues);
        var consumer = new Mock<IIssueConsumer>();

        var testSubject = CreateTestSubject(eslintBridgeAnalyzer.Object);
        await testSubject.ExecuteAsync(ValidFilePath, consumer.Object, CancellationToken.None);

        consumer.Verify(x => x.Accept(ValidFilePath, issues));
    }

    [TestMethod]
    public void ExecuteAnalysis_CriticalException_ExceptionThrown()
    {
        var eslintBridgeAnalyzer = SetupEslintBridgeAnalyzer(exceptionToThrow: new StackOverflowException());

        var testSubject = CreateTestSubject(eslintBridgeAnalyzer.Object);

        Func<Task> act = async () => await testSubject.ExecuteAsync(ValidFilePath, Mock.Of<IIssueConsumer>(), CancellationToken.None);
        act.Should().ThrowExactly<StackOverflowException>();
    }

    [TestMethod]
    public void Dispose_DisposesEslintBridgeAnalyzer()
    {
        var eslintBridgeAnalyzer = SetupEslintBridgeAnalyzer(null);

        var testSubject = CreateTestSubject(eslintBridgeAnalyzer.Object);

        Action act = () => testSubject.Dispose();
        act.Should().NotThrow();

        eslintBridgeAnalyzer.Verify(x => x.Dispose(), Times.Once);
    }

    [TestMethod, Description("Regression test for #2452")]
    public async Task ExecuteAnalysisAsync_EslintBridgeProcessLaunchException_NotifiesThatAnalysisFailedWithoutStackTrace()
    {
        var statusNotifier = new Mock<IAnalysisStatusNotifier>();
        var exception = new EslintBridgeProcessLaunchException("this is a test");
        var eslintBridgeAnalyzer = SetupEslintBridgeAnalyzer(exceptionToThrow: exception);

        var testSubject = CreateTestSubject(eslintBridgeAnalyzer.Object, statusNotifier: statusNotifier.Object);
        await testSubject.ExecuteAsync(ValidFilePath, Mock.Of<IIssueConsumer>(), CancellationToken.None);

        statusNotifier.Verify(x => x.AnalysisStarted(), Times.Once);
        statusNotifier.Verify(x => x.AnalysisFailed("this is a test"), Times.Once);
        statusNotifier.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task ExecuteAnalysisAsync_AnalysisFailed_NotifiesThatAnalysisFailed()
    {
        var statusNotifier = new Mock<IAnalysisStatusNotifier>();
        var exception = new NotImplementedException("this is a test");
        var eslintBridgeAnalyzer = SetupEslintBridgeAnalyzer(exceptionToThrow: exception);

        var testSubject = CreateTestSubject(eslintBridgeAnalyzer.Object, statusNotifier: statusNotifier.Object);
        await testSubject.ExecuteAsync(ValidFilePath, Mock.Of<IIssueConsumer>(), CancellationToken.None);

        statusNotifier.Verify(x => x.AnalysisStarted(), Times.Once);
        statusNotifier.Verify(x => x.AnalysisFailed(exception), Times.Once);
        statusNotifier.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task ExecuteAnalysisAsync_TaskCancelled_NotifiesThatAnalysisWasCancelled()
    {
        var statusNotifier = new Mock<IAnalysisStatusNotifier>();
        var eslintBridgeAnalyzer = SetupEslintBridgeAnalyzer(exceptionToThrow: new TaskCanceledException());

        var testSubject = CreateTestSubject(eslintBridgeAnalyzer.Object, statusNotifier: statusNotifier.Object);
        await testSubject.ExecuteAsync(ValidFilePath, Mock.Of<IIssueConsumer>(), CancellationToken.None);

        statusNotifier.Verify(x => x.AnalysisStarted(), Times.Once);
        statusNotifier.Verify(x => x.AnalysisCancelled(), Times.Once);
        statusNotifier.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task ExecuteAnalysisAsync_AnalysisFinished_NotifiesThatAnalysisFinished()
    {
        var statusNotifier = new Mock<IAnalysisStatusNotifier>();
        var issues = new[] { Mock.Of<IAnalysisIssue>(), Mock.Of<IAnalysisIssue>() };
        var eslintBridgeAnalyzer = SetupEslintBridgeAnalyzer(issues);

        var testSubject = CreateTestSubject(eslintBridgeAnalyzer.Object, statusNotifier: statusNotifier.Object);
        await testSubject.ExecuteAsync(ValidFilePath, Mock.Of<IIssueConsumer>(), CancellationToken.None);

        statusNotifier.Verify(x => x.AnalysisStarted(), Times.Once);
        statusNotifier.Verify(x => x.AnalysisFinished(2, It.IsAny<TimeSpan>()), Times.Once);
        statusNotifier.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task ExecuteAnalysisAsync_SwitchesToBackgroundThreadBeforeProcessing()
    {
        var callOrder = new List<string>();

        var threadHandling = new Mock<IThreadHandling>();
        threadHandling.Setup(x => x.SwitchToBackgroundThread())
            .Returns(() => new NoOpThreadHandler.NoOpAwaitable())
            .Callback(() => callOrder.Add("SwitchToBackgroundThread"));

        var statusNotifier = new Mock<IAnalysisStatusNotifier>();
        statusNotifier.Setup(x => x.AnalysisStarted()).Callback(() => callOrder.Add("AnalysisStarted"));

        var testSubject = CreateTestSubject(statusNotifier: statusNotifier.Object, threadHandling: threadHandling.Object);
        await testSubject.ExecuteAsync(ValidFilePath, Mock.Of<IIssueConsumer>(), CancellationToken.None);

        callOrder.Should().Equal("SwitchToBackgroundThread", "AnalysisStarted");
    }

    private Mock<IEslintBridgeAnalyzer> SetupEslintBridgeAnalyzer(IReadOnlyCollection<IAnalysisIssue> issues = null, Exception exceptionToThrow = null)
    {
        var eslintBridgeAnalyzer = new Mock<IEslintBridgeAnalyzer>();
        var setup = eslintBridgeAnalyzer.Setup(x => x.Analyze(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

        if (exceptionToThrow == null)
        {
            setup.ReturnsAsync(issues);
        }
        else
        {
            setup.ThrowsAsync(exceptionToThrow);
        }

        return eslintBridgeAnalyzer;
    }

    private CssAnalyzer CreateTestSubject(
        IEslintBridgeAnalyzer eslintBridgeAnalyzer = null,
        IRulesProvider rulesProvider = null,
        ITelemetryManager telemetryManager = null,
        IAnalysisStatusNotifier statusNotifier = null,
        IThreadHandling threadHandling = null)
    {
        telemetryManager ??= Mock.Of<ITelemetryManager>();
        statusNotifier ??= Mock.Of<IAnalysisStatusNotifier>();
        rulesProvider ??= Mock.Of<IRulesProvider>();
        threadHandling ??= new NoOpThreadHandler();

        var rulesProviderFactory = new Mock<IRulesProviderFactory>();
        rulesProviderFactory.Setup(x => x.Create("css", Language.Css)).Returns(rulesProvider);

        var eslintBridgeClient = Mock.Of<ICssEslintBridgeClient>();
        var eslintBridgeAnalyzerFactory = new Mock<IEslintBridgeAnalyzerFactory>();
        eslintBridgeAnalyzerFactory
            .Setup(x => x.Create(rulesProvider, eslintBridgeClient))
            .Returns(eslintBridgeAnalyzer);

        return new CssAnalyzer(eslintBridgeClient,
            rulesProviderFactory.Object,
            telemetryManager,
            SetUpStatusNotifierFactory(statusNotifier),
            eslintBridgeAnalyzerFactory.Object,
            threadHandling);
    }

    private IAnalysisStatusNotifierFactory SetUpStatusNotifierFactory(IAnalysisStatusNotifier statusNotifier)
    {
        var statusNotifierFactory = new Mock<IAnalysisStatusNotifierFactory>();
        statusNotifierFactory.Setup(x => x.Create(AnalyzerName, ValidFilePath)).Returns(statusNotifier);

        return statusNotifierFactory.Object;
    }
}
