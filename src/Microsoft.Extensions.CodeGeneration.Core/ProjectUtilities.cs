// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.CodeGeneration.Sources.DotNet;

namespace Microsoft.Extensions.CodeGeneration
{
    public static class ProjectUtilities
    {
        private static string[] _frameworkProjectNames = new[]
        {
            "Microsoft.Extensions.CodeGeneration",
            "Microsoft.Extensions.CodeGeneration.Core",
            "Microsoft.Extensions.CodeGeneration.Templating",
            "Microsoft.Extensions.CodeGeneration.Sources",
            "Microsoft.Extensions.CodeGenerators.Mvc",
        };

        public static CompilationReference GetProject(
            this ILibraryExporter libraryExporter,
            IApplicationEnvironment environment)
        {
            if (libraryExporter == null)
            {
                throw new ArgumentNullException(nameof(libraryExporter));
            }

            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            var project = libraryExporter.GetExport(environment.ApplicationName).GetMetadataReferences()
                //.OfType<IMetadataProjectReference>()
                //.OfType<IRoslynMetadataReference>()
                .Select(reference => reference as CompilationReference)
                .FirstOrDefault();

            Debug.Assert(project != null);
            return project;
        }

        public static IEnumerable<CompilationReference> GetProjectsInApp(
            this ILibraryExporter libraryExporter,
            IApplicationEnvironment environment)
        {
            if (libraryExporter == null)
            {
                throw new ArgumentNullException(nameof(libraryExporter));
            }

            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            var export = libraryExporter.GetExport(environment.ApplicationName);

            return export.GetMetadataReferences()
                //.OfType<IMetadataProjectReference>()
                //.OfType<IRoslynMetadataReference>()
                .Select(reference => reference as CompilationReference)
                .Where(compilation => !_frameworkProjectNames.Contains(compilation.Compilation.AssemblyName));
        }
    }
}
