﻿/*
 * SonarLint for Visual Studio
 * Copyright (C) 2016-2022 SonarSource SA
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

using Microsoft.CodeAnalysis;
using System;
using System.Collections.Immutable;

namespace SonarLint.VisualStudio.Roslyn.Suppressions
{
    /// <summary>
    /// Generated class that returns SupportedSuppressions for all Sonar C# and VB.NET rules
    /// </summary>
    internal sealed class SupportedSuppressionsBuilder
    {
        private static readonly Lazy<SupportedSuppressionsBuilder> lazy = new Lazy<SupportedSuppressionsBuilder>(() => new SupportedSuppressionsBuilder());

        public static SupportedSuppressionsBuilder Instance => lazy.Value;

        public ImmutableArray<SuppressionDescriptor> Descriptors { get; }

        private SupportedSuppressionsBuilder()
        {
            Descriptors = GetDescriptors();
        }

        private static ImmutableArray<SuppressionDescriptor> GetDescriptors()
        {
            var descriptors = new SuppressionDescriptor[]
            {
                // *************************************************************************************************
                // If the number of diagnostic ids changes significantly or decreases when the analysers are updated,
                // investigate! There may be a problem with code generator.
                // *************************************************************************************************
                // Number of unique diagnostic ids (C# and VB.NET): 429
                CreateDescriptor("S100"),
                CreateDescriptor("S1006"),
                CreateDescriptor("S101"),
                CreateDescriptor("S103"),
                CreateDescriptor("S104"),
                CreateDescriptor("S1048"),
                CreateDescriptor("S105"),
                CreateDescriptor("S106"),
                CreateDescriptor("S1066"),
                CreateDescriptor("S1067"),
                CreateDescriptor("S107"),
                CreateDescriptor("S1075"),
                CreateDescriptor("S108"),
                CreateDescriptor("S109"),
                CreateDescriptor("S110"),
                CreateDescriptor("S1104"),
                CreateDescriptor("S1109"),
                CreateDescriptor("S1110"),
                CreateDescriptor("S1116"),
                CreateDescriptor("S1117"),
                CreateDescriptor("S1118"),
                CreateDescriptor("S112"),
                CreateDescriptor("S1121"),
                CreateDescriptor("S1123"),
                CreateDescriptor("S1125"),
                CreateDescriptor("S1128"),
                CreateDescriptor("S113"),
                CreateDescriptor("S1134"),
                CreateDescriptor("S1135"),
                CreateDescriptor("S114"),
                CreateDescriptor("S1144"),
                CreateDescriptor("S1147"),
                CreateDescriptor("S1151"),
                CreateDescriptor("S1155"),
                CreateDescriptor("S1163"),
                CreateDescriptor("S1168"),
                CreateDescriptor("S117"),
                CreateDescriptor("S1172"),
                CreateDescriptor("S1185"),
                CreateDescriptor("S1186"),
                CreateDescriptor("S1192"),
                CreateDescriptor("S1197"),
                CreateDescriptor("S1199"),
                CreateDescriptor("S1200"),
                CreateDescriptor("S1206"),
                CreateDescriptor("S121"),
                CreateDescriptor("S1210"),
                CreateDescriptor("S1215"),
                CreateDescriptor("S122"),
                CreateDescriptor("S1226"),
                CreateDescriptor("S1227"),
                CreateDescriptor("S1244"),
                CreateDescriptor("S125"),
                CreateDescriptor("S126"),
                CreateDescriptor("S1264"),
                CreateDescriptor("S127"),
                CreateDescriptor("S1301"),
                CreateDescriptor("S1309"),
                CreateDescriptor("S131"),
                CreateDescriptor("S1313"),
                CreateDescriptor("S134"),
                CreateDescriptor("S138"),
                CreateDescriptor("S139"),
                CreateDescriptor("S1449"),
                CreateDescriptor("S1450"),
                CreateDescriptor("S1451"),
                CreateDescriptor("S1479"),
                CreateDescriptor("S1481"),
                CreateDescriptor("S1541"),
                CreateDescriptor("S1542"),
                CreateDescriptor("S1607"),
                CreateDescriptor("S1643"),
                CreateDescriptor("S1645"),
                CreateDescriptor("S1654"),
                CreateDescriptor("S1656"),
                CreateDescriptor("S1659"),
                CreateDescriptor("S1694"),
                CreateDescriptor("S1696"),
                CreateDescriptor("S1698"),
                CreateDescriptor("S1699"),
                CreateDescriptor("S1751"),
                CreateDescriptor("S1764"),
                CreateDescriptor("S1821"),
                CreateDescriptor("S1848"),
                CreateDescriptor("S1854"),
                CreateDescriptor("S1858"),
                CreateDescriptor("S1862"),
                CreateDescriptor("S1871"),
                CreateDescriptor("S1905"),
                CreateDescriptor("S1939"),
                CreateDescriptor("S1940"),
                CreateDescriptor("S1944"),
                CreateDescriptor("S1994"),
                CreateDescriptor("S2053"),
                CreateDescriptor("S2068"),
                CreateDescriptor("S2077"),
                CreateDescriptor("S2092"),
                CreateDescriptor("S2114"),
                CreateDescriptor("S2115"),
                CreateDescriptor("S2123"),
                CreateDescriptor("S2148"),
                CreateDescriptor("S2156"),
                CreateDescriptor("S2178"),
                CreateDescriptor("S2183"),
                CreateDescriptor("S2184"),
                CreateDescriptor("S2187"),
                CreateDescriptor("S2190"),
                CreateDescriptor("S2197"),
                CreateDescriptor("S2201"),
                CreateDescriptor("S2219"),
                CreateDescriptor("S2221"),
                CreateDescriptor("S2222"),
                CreateDescriptor("S2223"),
                CreateDescriptor("S2225"),
                CreateDescriptor("S2228"),
                CreateDescriptor("S2234"),
                CreateDescriptor("S2245"),
                CreateDescriptor("S2251"),
                CreateDescriptor("S2252"),
                CreateDescriptor("S2255"),
                CreateDescriptor("S2257"),
                CreateDescriptor("S2259"),
                CreateDescriptor("S2275"),
                CreateDescriptor("S2290"),
                CreateDescriptor("S2291"),
                CreateDescriptor("S2292"),
                CreateDescriptor("S2302"),
                CreateDescriptor("S2304"),
                CreateDescriptor("S2306"),
                CreateDescriptor("S2325"),
                CreateDescriptor("S2326"),
                CreateDescriptor("S2327"),
                CreateDescriptor("S2328"),
                CreateDescriptor("S2330"),
                CreateDescriptor("S2333"),
                CreateDescriptor("S2339"),
                CreateDescriptor("S2340"),
                CreateDescriptor("S2342"),
                CreateDescriptor("S2343"),
                CreateDescriptor("S2344"),
                CreateDescriptor("S2345"),
                CreateDescriptor("S2346"),
                CreateDescriptor("S2347"),
                CreateDescriptor("S2348"),
                CreateDescriptor("S2349"),
                CreateDescriptor("S2352"),
                CreateDescriptor("S2353"),
                CreateDescriptor("S2354"),
                CreateDescriptor("S2355"),
                CreateDescriptor("S2357"),
                CreateDescriptor("S2358"),
                CreateDescriptor("S2359"),
                CreateDescriptor("S2360"),
                CreateDescriptor("S2362"),
                CreateDescriptor("S2363"),
                CreateDescriptor("S2364"),
                CreateDescriptor("S2365"),
                CreateDescriptor("S2366"),
                CreateDescriptor("S2367"),
                CreateDescriptor("S2368"),
                CreateDescriptor("S2369"),
                CreateDescriptor("S2370"),
                CreateDescriptor("S2372"),
                CreateDescriptor("S2373"),
                CreateDescriptor("S2374"),
                CreateDescriptor("S2375"),
                CreateDescriptor("S2376"),
                CreateDescriptor("S2386"),
                CreateDescriptor("S2387"),
                CreateDescriptor("S2429"),
                CreateDescriptor("S2436"),
                CreateDescriptor("S2437"),
                CreateDescriptor("S2479"),
                CreateDescriptor("S2486"),
                CreateDescriptor("S2551"),
                CreateDescriptor("S2583"),
                CreateDescriptor("S2589"),
                CreateDescriptor("S2612"),
                CreateDescriptor("S2674"),
                CreateDescriptor("S2681"),
                CreateDescriptor("S2688"),
                CreateDescriptor("S2692"),
                CreateDescriptor("S2696"),
                CreateDescriptor("S2699"),
                CreateDescriptor("S2701"),
                CreateDescriptor("S2737"),
                CreateDescriptor("S2743"),
                CreateDescriptor("S2755"),
                CreateDescriptor("S2757"),
                CreateDescriptor("S2760"),
                CreateDescriptor("S2761"),
                CreateDescriptor("S2857"),
                CreateDescriptor("S2930"),
                CreateDescriptor("S2931"),
                CreateDescriptor("S2933"),
                CreateDescriptor("S2934"),
                CreateDescriptor("S2951"),
                CreateDescriptor("S2952"),
                CreateDescriptor("S2953"),
                CreateDescriptor("S2955"),
                CreateDescriptor("S2971"),
                CreateDescriptor("S2995"),
                CreateDescriptor("S2996"),
                CreateDescriptor("S2997"),
                CreateDescriptor("S3005"),
                CreateDescriptor("S3010"),
                CreateDescriptor("S3011"),
                CreateDescriptor("S3052"),
                CreateDescriptor("S3059"),
                CreateDescriptor("S3060"),
                CreateDescriptor("S3168"),
                CreateDescriptor("S3169"),
                CreateDescriptor("S3172"),
                CreateDescriptor("S3215"),
                CreateDescriptor("S3216"),
                CreateDescriptor("S3217"),
                CreateDescriptor("S3218"),
                CreateDescriptor("S3220"),
                CreateDescriptor("S3234"),
                CreateDescriptor("S3235"),
                CreateDescriptor("S3236"),
                CreateDescriptor("S3237"),
                CreateDescriptor("S3240"),
                CreateDescriptor("S3241"),
                CreateDescriptor("S3242"),
                CreateDescriptor("S3244"),
                CreateDescriptor("S3246"),
                CreateDescriptor("S3247"),
                CreateDescriptor("S3249"),
                CreateDescriptor("S3251"),
                CreateDescriptor("S3253"),
                CreateDescriptor("S3254"),
                CreateDescriptor("S3256"),
                CreateDescriptor("S3257"),
                CreateDescriptor("S3260"),
                CreateDescriptor("S3261"),
                CreateDescriptor("S3262"),
                CreateDescriptor("S3263"),
                CreateDescriptor("S3264"),
                CreateDescriptor("S3265"),
                CreateDescriptor("S3267"),
                CreateDescriptor("S3329"),
                CreateDescriptor("S3330"),
                CreateDescriptor("S3343"),
                CreateDescriptor("S3346"),
                CreateDescriptor("S3353"),
                CreateDescriptor("S3358"),
                CreateDescriptor("S3366"),
                CreateDescriptor("S3376"),
                CreateDescriptor("S3385"),
                CreateDescriptor("S3397"),
                CreateDescriptor("S3400"),
                CreateDescriptor("S3415"),
                CreateDescriptor("S3427"),
                CreateDescriptor("S3431"),
                CreateDescriptor("S3433"),
                CreateDescriptor("S3440"),
                CreateDescriptor("S3441"),
                CreateDescriptor("S3442"),
                CreateDescriptor("S3443"),
                CreateDescriptor("S3444"),
                CreateDescriptor("S3445"),
                CreateDescriptor("S3447"),
                CreateDescriptor("S3449"),
                CreateDescriptor("S3450"),
                CreateDescriptor("S3451"),
                CreateDescriptor("S3453"),
                CreateDescriptor("S3456"),
                CreateDescriptor("S3457"),
                CreateDescriptor("S3458"),
                CreateDescriptor("S3459"),
                CreateDescriptor("S3464"),
                CreateDescriptor("S3466"),
                CreateDescriptor("S3532"),
                CreateDescriptor("S3597"),
                CreateDescriptor("S3598"),
                CreateDescriptor("S3600"),
                CreateDescriptor("S3603"),
                CreateDescriptor("S3604"),
                CreateDescriptor("S3610"),
                CreateDescriptor("S3626"),
                CreateDescriptor("S3655"),
                CreateDescriptor("S3717"),
                CreateDescriptor("S3776"),
                CreateDescriptor("S3860"),
                CreateDescriptor("S3866"),
                CreateDescriptor("S3869"),
                CreateDescriptor("S3871"),
                CreateDescriptor("S3872"),
                CreateDescriptor("S3874"),
                CreateDescriptor("S3875"),
                CreateDescriptor("S3876"),
                CreateDescriptor("S3877"),
                CreateDescriptor("S3880"),
                CreateDescriptor("S3881"),
                CreateDescriptor("S3884"),
                CreateDescriptor("S3885"),
                CreateDescriptor("S3887"),
                CreateDescriptor("S3889"),
                CreateDescriptor("S3897"),
                CreateDescriptor("S3898"),
                CreateDescriptor("S3900"),
                CreateDescriptor("S3902"),
                CreateDescriptor("S3903"),
                CreateDescriptor("S3904"),
                CreateDescriptor("S3906"),
                CreateDescriptor("S3908"),
                CreateDescriptor("S3909"),
                CreateDescriptor("S3923"),
                CreateDescriptor("S3925"),
                CreateDescriptor("S3926"),
                CreateDescriptor("S3927"),
                CreateDescriptor("S3928"),
                CreateDescriptor("S3937"),
                CreateDescriptor("S3949"),
                CreateDescriptor("S3956"),
                CreateDescriptor("S3962"),
                CreateDescriptor("S3963"),
                CreateDescriptor("S3966"),
                CreateDescriptor("S3967"),
                CreateDescriptor("S3971"),
                CreateDescriptor("S3972"),
                CreateDescriptor("S3973"),
                CreateDescriptor("S3981"),
                CreateDescriptor("S3984"),
                CreateDescriptor("S3990"),
                CreateDescriptor("S3992"),
                CreateDescriptor("S3993"),
                CreateDescriptor("S3994"),
                CreateDescriptor("S3995"),
                CreateDescriptor("S3996"),
                CreateDescriptor("S3997"),
                CreateDescriptor("S3998"),
                CreateDescriptor("S4000"),
                CreateDescriptor("S4002"),
                CreateDescriptor("S4004"),
                CreateDescriptor("S4005"),
                CreateDescriptor("S4015"),
                CreateDescriptor("S4016"),
                CreateDescriptor("S4017"),
                CreateDescriptor("S4018"),
                CreateDescriptor("S4019"),
                CreateDescriptor("S4022"),
                CreateDescriptor("S4023"),
                CreateDescriptor("S4025"),
                CreateDescriptor("S4026"),
                CreateDescriptor("S4027"),
                CreateDescriptor("S4035"),
                CreateDescriptor("S4036"),
                CreateDescriptor("S4039"),
                CreateDescriptor("S4040"),
                CreateDescriptor("S4041"),
                CreateDescriptor("S4047"),
                CreateDescriptor("S4049"),
                CreateDescriptor("S4050"),
                CreateDescriptor("S4052"),
                CreateDescriptor("S4055"),
                CreateDescriptor("S4056"),
                CreateDescriptor("S4057"),
                CreateDescriptor("S4058"),
                CreateDescriptor("S4059"),
                CreateDescriptor("S4060"),
                CreateDescriptor("S4061"),
                CreateDescriptor("S4069"),
                CreateDescriptor("S4070"),
                CreateDescriptor("S4136"),
                CreateDescriptor("S4143"),
                CreateDescriptor("S4144"),
                CreateDescriptor("S4158"),
                CreateDescriptor("S4159"),
                CreateDescriptor("S4200"),
                CreateDescriptor("S4201"),
                CreateDescriptor("S4210"),
                CreateDescriptor("S4211"),
                CreateDescriptor("S4212"),
                CreateDescriptor("S4214"),
                CreateDescriptor("S4220"),
                CreateDescriptor("S4225"),
                CreateDescriptor("S4226"),
                CreateDescriptor("S4260"),
                CreateDescriptor("S4261"),
                CreateDescriptor("S4275"),
                CreateDescriptor("S4277"),
                CreateDescriptor("S4423"),
                CreateDescriptor("S4426"),
                CreateDescriptor("S4428"),
                CreateDescriptor("S4433"),
                CreateDescriptor("S4456"),
                CreateDescriptor("S4457"),
                CreateDescriptor("S4462"),
                CreateDescriptor("S4487"),
                CreateDescriptor("S4502"),
                CreateDescriptor("S4507"),
                CreateDescriptor("S4524"),
                CreateDescriptor("S4564"),
                CreateDescriptor("S4581"),
                CreateDescriptor("S4583"),
                CreateDescriptor("S4586"),
                CreateDescriptor("S4635"),
                CreateDescriptor("S4784"),
                CreateDescriptor("S4787"),
                CreateDescriptor("S4790"),
                CreateDescriptor("S4792"),
                CreateDescriptor("S4818"),
                CreateDescriptor("S4823"),
                CreateDescriptor("S4829"),
                CreateDescriptor("S4830"),
                CreateDescriptor("S4834"),
                CreateDescriptor("S5034"),
                CreateDescriptor("S5042"),
                CreateDescriptor("S5122"),
                CreateDescriptor("S5332"),
                CreateDescriptor("S5443"),
                CreateDescriptor("S5445"),
                CreateDescriptor("S5542"),
                CreateDescriptor("S5547"),
                CreateDescriptor("S5659"),
                CreateDescriptor("S5693"),
                CreateDescriptor("S5753"),
                CreateDescriptor("S5766"),
                CreateDescriptor("S5773"),
                CreateDescriptor("S5944"),
                CreateDescriptor("S6145"),
                CreateDescriptor("S6146"),
                CreateDescriptor("S6354"),
                CreateDescriptor("S818"),
                CreateDescriptor("S881"),
                CreateDescriptor("S907"),
                CreateDescriptor("S927"),
            };
            return ImmutableArray.ToImmutableArray(descriptors);
        }

        private static SuppressionDescriptor CreateDescriptor(string diagId) =>
            new SuppressionDescriptor("X" + diagId, diagId, "Suppressed on the Sonar server");
    }
}
