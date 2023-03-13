/*
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

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using SonarLint.VisualStudio.Core;
using SonarLint.VisualStudio.Core.Binding;
using SonarLint.VisualStudio.Integration;

namespace SonarLint.VisualStudio.ConnectedMode.ConnectedSession
{
    [Export(typeof(IConnectedSessionManager))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class ConnectedSessionManager2 : IConnectedSessionManager
    {
        private readonly IReadOnlyCollection<ISessionArtefactFactory> factories;
        private readonly ILogger logger;
        private readonly IThreadHandling threadHandling;

        private readonly SemaphoreSlim toLock = new SemaphoreSlim(1, 1);

        private ConnectedSession currentSession;
        private bool disposed;

        [ImportingConstructor]
        public ConnectedSessionManager2(
            [ImportMany] IEnumerable<ISessionArtefactFactory> factories,
            ILogger logger,
            IThreadHandling threadHandling)
        {
            this.factories = factories.ToList();
            this.logger = logger;
            this.threadHandling = threadHandling;
        }

        public async Task InitializeAsync(BindingConfiguration bindingConfiguration)
            => await HandleBindingChangedAsync(bindingConfiguration);
        
        public async Task HandleBindingChangedAsync(BindingConfiguration bindingConfiguration)
        {
            await threadHandling.SwitchToBackgroundThread();

            await toLock.WaitAsync(); // TODO - timeout?

            try
            {
                var isInConnectedMode = !bindingConfiguration.Equals(BindingConfiguration.Standalone);

                currentSession?.Dispose();
                currentSession = isInConnectedMode ? new ConnectedSession(factories, logger) : null;
                currentSession?.InitializeAsync(bindingConfiguration);
            }
            finally
            {
                toLock.Release();
            }
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            logger.LogVerbose("[Connected mode] Disposing connected session manager");
            currentSession?.Dispose();
            currentSession = null;
            DisposeHelper.SafeDisposeAll(factories, logger);
        }

        private sealed class ConnectedSession : IDisposable
        {
            /// <summary>
            /// Static counter to provide a unique session identifier for debugging/logging
            /// </summary>
            private static int sessionCounter;

            private readonly IEnumerable<ISessionArtefactFactory> factories;
            private readonly ILogger logger;
            private readonly int sessionId;

            /// <summary>
            /// List of all artefacts created in the current session
            /// </summary>
            private readonly IList<ISessionArtefact> sessionArtefacts = new List<ISessionArtefact>();
            private bool disposedValue;

            public ConnectedSession(IEnumerable<ISessionArtefactFactory> factories, ILogger logger)
            {
                sessionId = sessionCounter++;
                this.factories = factories;
                this.logger = logger;
            }

            public async Task InitializeAsync(BindingConfiguration bindingConfiguration)
            {
                logger.LogVerbose($"[Connected Mode] Created session: {sessionId}");

                foreach (var factory in factories)
                {
                    var newArtefact = await factory.CreateAsync(bindingConfiguration);
                    sessionArtefacts.Add(newArtefact);
                    await newArtefact.InitializeAsync();
                }
            }

            private void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        logger.LogVerbose($"[Connected Mode] Disposed session: {sessionId}");
                        DisposeHelper.SafeDisposeAll(sessionArtefacts);
                    }
                    disposedValue = true;
                }
            }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
    }
}
