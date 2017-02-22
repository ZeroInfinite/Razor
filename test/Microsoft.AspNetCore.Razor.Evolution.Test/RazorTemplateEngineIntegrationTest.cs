// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Microsoft.AspNetCore.Razor.Evolution.IntegrationTests;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    public class RazorTemplateEngineIntegrationTest : IntegrationTestBase
    {
        [Fact]
        public void GenerateCodeWithDefaults()
        {
            // Arrange
            var filePath = Path.Combine(TestProjectRoot, $"{Filename}.cshtml");
            var content = File.ReadAllText(filePath);
            var projectItem = new TestRazorProjectItem($"{Filename}.cshtml", "")
            {
                Content = content,
            };
            var project = new TestRazorProject(new[] { projectItem });
            var razorEngine = RazorEngine.Create(icanhasDefaults: true);
            var templateEngine = new RazorTemplateEngine(razorEngine, project);

            // Act
            var result = templateEngine.GenerateCode(projectItem.Path);

            // Assert
            Assert.NotNull(result);
            var cSharpDocument = result.GetCSharpDocument();
            AssertCSharpDocumentMatchesBaseline(cSharpDocument);
        }

        [Fact]
        public void GenerateCodeWithBaseType()
        {
            // Arrange
            var filePath = Path.Combine(TestProjectRoot, $"{Filename}.cshtml");
            var content = File.ReadAllText(filePath);
            var projectItem = new TestRazorProjectItem($"{Filename}.cshtml", "")
            {
                Content = content,
            };
            var project = new TestRazorProject(new[] { projectItem });
            var razorEngine = RazorEngine.Create(
                engine => engine.SetBaseType("MyBaseType"),
                icanhasDefaults: true);
            var templateEngine = new RazorTemplateEngine(razorEngine, project);

            // Act
            var result = templateEngine.GenerateCode(projectItem.Path);

            // Assert
            Assert.NotNull(result);
            var cSharpDocument = result.GetCSharpDocument();
            AssertCSharpDocumentMatchesBaseline(cSharpDocument);
        }
    }
}
