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

using System.Threading.Tasks;
using SonarLint.VisualStudio.Core.Binding;

/* Lifecycle:
 * The session manager monitors when a connected mode starts/stops (?is updated?)
 * - 1 connected session starts when a bound solution is opened, or an unbound solution is bound
 * for the first time.
 * - a connected session ends when a bound solution is closed.
 * 
 * When a session starts, the manager will:
 * 1. Dispose any existing artefacts
 * 2. Call all registered factories and ask them to create and return artefacts for the new session
 * 3. The factory will then call "InitializeAsync" on each new artefact
 * 
 * When a session ends, the manager will dispose any arterfacts created by the factories for that
 * session.
 * 
 * All artefacts are created/disposed on a background thread
 */

namespace SonarLint.VisualStudio.ConnectedMode.ConnectedSession
{
    /// <summary>
    /// Reacts to project changes and creates/disposes objects that should
    /// only exist for connected projects
    /// </summary>
    internal interface IConnectedSessionManager
    {
        Task InitializeAsync(BindingConfiguration bindingConfiguration);

        Task HandleBindingChangedAsync(BindingConfiguration bindingConfiguration);
    }

    /// <summary>
    /// Creates objects with a lifetime tied to a single connected mode session
    /// </summary>
    internal interface ISessionArtefactFactory
    {
        Task<ISessionArtefact> CreateAsync(BindingConfiguration bindingConfig);
    }

    internal interface ISessionArtefact
    {
        Task InitializeAsync();
    }
}
