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

using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
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
    internal class ConnectedSessionManager : IConnectedSessionManager
    {
        private readonly IReadOnlyCollection<ISessionArtefactFactory> factories;
        private readonly ILogger logger;
        private readonly IThreadHandling threadHandling;

        /// <summary>
        /// List of all artefacts created in the current session
        /// </summary>
        private readonly IList<ISessionArtefact> sessionArtefacts = new List<ISessionArtefact>();

        private bool disposed;

        private readonly SemaphoreSlim toLock = new SemaphoreSlim(1, 1);

        [ImportingConstructor]
        public ConnectedSessionManager(
            [ImportMany] IEnumerable<ISessionArtefactFactory> factories,
            ILogger logger,
            IThreadHandling threadHandling)
        {
            this.factories = factories.ToList();
            this.logger = logger;
            this.threadHandling = threadHandling;
        }

        public async Task InitializeAsync(BindingConfiguration bindingConfiguration)
        {
            await threadHandling.SwitchToBackgroundThread();
            await HandleBindingChangedAsync(bindingConfiguration);
        }

        public async Task HandleBindingChangedAsync(BindingConfiguration bindingConfiguration)
        {
            try
            {
                await threadHandling.SwitchToBackgroundThread();
                await toLock.WaitAsync();
 
                EndCurrentSessionUnderLock();

                var isInConnectedMode = !bindingConfiguration.Equals(BindingConfiguration.Standalone);

                if (!isInConnectedMode)
                {
                    return;
                }

                foreach(var factory in factories)
                {
                    var newArtefact = await factory.CreateAsync(bindingConfiguration);
                    sessionArtefacts.Add(newArtefact);
                    await newArtefact.InitializeAsync();
                }
            }
            finally
            {
                toLock.Release();
            }
        }

        private async Task AcquireLockAndEndCurrentSessionAsync()
        {
            try
            {
                await threadHandling.SwitchToBackgroundThread();
                await toLock.WaitAsync();
                DisposeHelper.SafeDisposeAll(sessionArtefacts);
                sessionArtefacts.Clear();
            }
            finally
            {
                toLock.Release();
            }
        }

        private void EndCurrentSessionUnderLock()
        {
            Debug.Assert(toLock.CurrentCount == 0, "Should have acquired the lock before calling this method");
            DisposeHelper.SafeDisposeAll(sessionArtefacts);
            sessionArtefacts.Clear();
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            DisposeHelper.SafeDisposeAll(factories);
            AcquireLockAndEndCurrentSessionAsync().Forget();
            disposed = true;
        }
    }
}
