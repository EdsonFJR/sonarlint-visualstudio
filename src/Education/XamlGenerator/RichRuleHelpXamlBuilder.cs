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

using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Markup;
using SonarLint.VisualStudio.Education.Layout.Logical;
using SonarLint.VisualStudio.Education.Layout.Visual.Tabs;
using SonarLint.VisualStudio.Rules;

namespace SonarLint.VisualStudio.Education.XamlGenerator
{
    internal interface IRichRuleHelpXamlBuilder
    {
        /// <summary>
        /// Generates a XAML document containing the help information for the specified rule
        /// </summary>
        /// <remarks>Assumes that the <see cref="IRuleInfo.Description"/> and <see cref="IRuleInfo.DescriptionSections"/> are parseable as XML.
        /// Also assumes that the containing control defines a list of Style resources, one for each
        /// value in the enum <see cref="StyleResourceNames"/>.
        /// The document will still render if a style is missing, but the styling won't be correct.</remarks>
        /// <param name="ruleInfo">Rule description information</param>
        /// <param name="issueContext">Key for the How to fix it Context acquired from a specific issue</param>
        FlowDocument Create(IRuleInfo ruleInfo, string issueContext);
    }

    [Export(typeof(IRichRuleHelpXamlBuilder))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class RichRuleHelpXamlBuilder : IRichRuleHelpXamlBuilder
    {
        private readonly IRuleInfoTranslator ruleInfoTranslator;
        private readonly IXamlGeneratorHelperFactory xamlGeneratorHelperFactory;
        private readonly IStaticXamlStorage staticXamlStorage;
        private readonly IXamlWriterFactory xamlWriterFactory;

        [ImportingConstructor]
        public RichRuleHelpXamlBuilder(IRuleInfoTranslator ruleInfoTranslator, IXamlGeneratorHelperFactory xamlGeneratorHelperFactory, IStaticXamlStorage staticXamlStorage, IXamlWriterFactory xamlWriterFactory)
        {
            this.ruleInfoTranslator = ruleInfoTranslator;
            this.xamlGeneratorHelperFactory = xamlGeneratorHelperFactory;
            this.staticXamlStorage = staticXamlStorage;
            this.xamlWriterFactory = xamlWriterFactory;
        }

        public FlowDocument Create(IRuleInfo ruleInfo, string issueContext)
        {
            var richRuleDescriptionSections = ruleInfoTranslator.GetRuleDescriptionSections(ruleInfo, issueContext).ToList();
            var mainTabGroup = new TabGroup(richRuleDescriptionSections
                .Select(richRuleDescriptionSection =>
                    new TabItem(richRuleDescriptionSection.Title,
                        richRuleDescriptionSection.GetVisualizationTreeNode(staticXamlStorage)))
                .ToList<ITabItem>(),
                0);

            var sb = new StringBuilder();
            var writer = xamlWriterFactory.Create(sb);
            var helper = xamlGeneratorHelperFactory.Create(writer);

            helper.WriteDocumentHeader(ruleInfo);
            mainTabGroup.ProduceXaml(writer);
            helper.EndDocument();

            return (FlowDocument)XamlReader.Parse(sb.ToString());
        }
    }
}
