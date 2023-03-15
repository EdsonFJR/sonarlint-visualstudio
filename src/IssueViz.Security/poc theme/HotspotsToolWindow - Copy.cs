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

using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace SonarLint.VisualStudio.IssueVisualization.Security.poc_theme
{
    [Guid(ToolWindowIdAsString)]
    public class TestToolWindow : ToolWindowPane
    {
        private const string ToolWindowIdAsString = "0e67dd49-fd90-494b-9323-a4fe26905f2a";
        public static readonly Guid ToolWindowId = new Guid(ToolWindowIdAsString);

        public TestToolWindow()
        {
            Caption = Resources.HotspotsToolWindowCaption;
            Content = new MainWindow();
        }
    }
}
