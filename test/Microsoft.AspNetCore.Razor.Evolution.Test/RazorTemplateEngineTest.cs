// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Razor.Evolution.IntegrationTests;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    public class RazorTemplateEngineTest : IntegrationTestBase
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
        public void GenerateCodeWithDefaults()
        {
            // Arrange
            var filePath = Path.Combine(TestProjectRoot, $"{Filename}.cshtml");
            var content = File.ReadAllText(filePath);
            var projectItem = new TestRazorProjectItem($"{Filename}.cshtml", "", content);
            var project = new TestRazorProject(new[] { projectItem });
            var razorEngine = RazorEngine.Create(c => c.WithDefaultConfiguration());
            var templateEngine = new RazorTemplateEngine(razorEngine, project);

            // Act
            var result = templateEngine.GenerateCode(projectItem.Path);

            // Assert
            Assert.Same(projectItem, result.ProjectItem);
            Assert.NotNull(result.CodeDocument);
            AssertCSharpDocumentMatchesBaseline(result.CSharpDocument);
        }

        private class TestRazorProject : RazorProject
        {
            private readonly IDictionary<string, RazorProjectItem> _lookup;

            public TestRazorProject(IList<RazorProjectItem> items)
            {
                _lookup = items.ToDictionary(item => item.Path);
            }

            public override IEnumerable<RazorProjectItem> EnumerateItems(string basePath)
            {
                throw new NotImplementedException();
            }

            public override RazorProjectItem GetItem(string path)
            {
                if (!_lookup.TryGetValue(path, out var value))
                {
                    value = new NotFoundProjectItem("", path);
                }

                return value;
            }
        }

        private class TestRazorProjectItem : RazorProjectItem
        {
            private string _path;
            private string _physicalPath;
            private readonly string _content;

            public TestRazorProjectItem(string path, string physicalPath, string content)
            {
                _path = path;
                _physicalPath = physicalPath;
                _content = content;
            }

            public override string BasePath => throw new NotImplementedException();

            public override string Path => _path;

            public void SetPath(string path) => _path = path;

            public override string PhysicalPath => _physicalPath;

            public override bool Exists => true;

            public override Stream Read()
            {
                return new MemoryStream(Encoding.UTF8.GetBytes(_content));
            }
        }
    }
}
