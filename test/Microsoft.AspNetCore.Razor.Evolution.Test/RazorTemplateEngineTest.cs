// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.AspNetCore.Testing;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    public class RazorTemplateEngineTest
    {
        [Fact]
        public void GenerateCode_ThrowsIfItemCannotBeFound()
        {
            // Arrange
            var project = new TestRazorProject(new RazorProjectItem[] { });
            var razorEngine = RazorEngine.Create();
            var templateEngine = new RazorTemplateEngine(razorEngine, project);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(
                () => templateEngine.GenerateCode("/does-not-exist.cstml"));
            Assert.Equal("The item '/does-not-exist.cstml' could not be found.", ex.Message);
        }

        [Fact]
        public void SettingOptions_ThrowsIfValueIsNull()
        {
            // Arrange
            var project = new TestRazorProject(new RazorProjectItem[] { });
            var razorEngine = RazorEngine.Create();
            var templateEngine = new RazorTemplateEngine(razorEngine, project);

            // Act & Assert
            ExceptionAssert.ThrowsArgumentNull(
                () => templateEngine.Options = null,
                "value");
        }

        [Fact]
        public void CreateCodeDocument_IncludesImportsIfFileIsPresent()
        {
            // Arrange
            var projectItem = new TestRazorProjectItem("/Views/Home/Index.cshtml");
            var import1 = new TestRazorProjectItem("/MyImport.cshtml");
            var import2 = new TestRazorProjectItem("/Views/Home/MyImport.cshtml");
            var project = new TestRazorProject(new[] { import1, import2, projectItem });
            var razorEngine = RazorEngine.Create();
            var templateEngine = new RazorTemplateEngine(razorEngine, project)
            {
                Options =
                {
                    ImportsFileName = "MyImport.cshtml",
                }
            };

            // Act
            var codeDocument = templateEngine.CreateCodeDocument(projectItem);

            // Assert
            Assert.Collection(codeDocument.Imports,
                import => Assert.Equal("/MyImport.cshtml", import.Filename),
                import => Assert.Equal("/Views/Home/MyImport.cshtml", import.Filename));
        }

        [Fact]
        public void CreateCodeDocument_IncludesDefaultImportIfNotNull()
        {
            // Arrange
            var projectItem = new TestRazorProjectItem("/Views/Home/Index.cshtml");
            var import1 = new TestRazorProjectItem("/MyImport.cshtml");
            var import2 = new TestRazorProjectItem("/Views/Home/MyImport.cshtml");
            var project = new TestRazorProject(new[] { import1, import2, projectItem });
            var razorEngine = RazorEngine.Create();
            var defaultImport = RazorSourceDocument.ReadFrom(new MemoryStream(), "Default.cshtml");
            var templateEngine = new RazorTemplateEngine(razorEngine, project)
            {
                Options =
                {
                    ImportsFileName = "MyImport.cshtml",
                    DefaultImports = defaultImport,
                }
            };

            // Act
            var codeDocument = templateEngine.CreateCodeDocument(projectItem);

            // Assert
            Assert.Collection(codeDocument.Imports,
                import => Assert.Same(defaultImport, import),
                import => Assert.Equal("/MyImport.cshtml", import.Filename),
                import => Assert.Equal("/Views/Home/MyImport.cshtml", import.Filename));
        }
    }
}
