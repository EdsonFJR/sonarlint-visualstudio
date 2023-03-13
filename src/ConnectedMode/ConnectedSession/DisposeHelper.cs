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
using System.Linq;
using SonarLint.VisualStudio.Core;
using SonarLint.VisualStudio.Integration;

namespace SonarLint.VisualStudio.ConnectedMode.ConnectedSession
{
    internal static class DisposeHelper
    {
        /// <summary>
        /// Safely disposes of all disposable items in the supplied list
        /// </summary>
        /// <remarks>Any objects that are not disposable are ignored</remarks>
        public static void SafeDisposeAll(IEnumerable<object> objects, ILogger logger = null)
        {
            foreach (var disposable in objects.OfType<IDisposable>())
            {
                SafeDispose(disposable, logger);
            }
        }

        public static void SafeDispose(IDisposable disposable, ILogger logger = null)
        {
            try
            {
                disposable.Dispose();
            }
            catch (Exception ex) when (!ErrorHandler.IsCriticalException(ex))
            {
                logger.LogVerbose($"[ConnectedSession] Error disposing artefact: {disposable.GetType().FullName}, Error: {ex}");
                logger.WriteLine($"[ConnectedSession] Error disposing artefact: {disposable.GetType().FullName}, Error: {ex.Message}");
            }
        }
    }
}
