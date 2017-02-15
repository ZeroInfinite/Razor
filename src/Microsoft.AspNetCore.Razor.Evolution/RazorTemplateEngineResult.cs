// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.Evolution
{
    public struct RazorTemplateEngineResult
    {
        public RazorTemplateEngineResult(
            RazorProjectItem projectItem,
            RazorCodeDocument codeDocument, 
            RazorCSharpDocument csharpDocument)
        {
            ProjectItem = projectItem;
            CodeDocument = codeDocument;
            CSharpDocument = csharpDocument;
        }

        public RazorProjectItem ProjectItem { get; }

        public RazorCodeDocument CodeDocument { get; }

        public RazorCSharpDocument CSharpDocument { get; }
    }
}
