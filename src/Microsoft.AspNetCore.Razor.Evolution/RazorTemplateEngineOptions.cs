// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.Evolution
{
    /// <summary>
    /// Options for code generation in the <see cref="RazorTemplateEngine"/>.
    /// </summary>
    public class RazorTemplateEngineOptions
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RazorTemplateEngineOptions"/>.
        /// </summary>
        /// <param name="importsFileName">The file name of the imports (e.g. _ViewImports.cshtml).</param>
        /// <param name="defaultImports">The default set of imports.</param>
        public RazorTemplateEngineOptions(string importsFileName, RazorSourceDocument defaultImports)
        {
            ImportsFileName = importsFileName;
            DefaultImports = defaultImports;
        }

        /// <summary>
        /// Default set of options for <see cref="RazorTemplateEngine"/>.
        /// </summary>
        public static RazorTemplateEngineOptions Default { get; } =
            new RazorTemplateEngineOptions(string.Empty, defaultImports: null);

        /// <summary>
        /// Gets the file name of the imports file (e.g. _ViewImports.cshtml).
        /// </summary>
        public string ImportsFileName { get; }

        /// <summary>
        /// Gets the default set of imports.
        /// </summary>
        public RazorSourceDocument DefaultImports { get; }
    }
}
