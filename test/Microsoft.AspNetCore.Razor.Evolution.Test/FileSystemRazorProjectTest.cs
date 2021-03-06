﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    public class FileSystemRazorProjectTest
    {
        private static string TestFolder { get; } =
            Path.Combine(TestProject.GetProjectDirectory(), "TestFiles", "FileSystemRazorProject");

        [Fact]
        public void EnumerateItems_DiscoversAllCshtmlFiles()
        {
            // Arrange
            var fileSystemProject = new FileSystemRazorProject(TestFolder);

            // Act
            var files = fileSystemProject.EnumerateItems("/");

            // Assert
            Assert.Collection(files.OrderBy(f => f.Path),
                file =>
                {
                    Assert.Equal("/Home.cshtml", file.Path);
                    Assert.Equal("/", file.BasePath);
                },
                file =>
                {
                    Assert.Equal("/Views/About/About.cshtml", file.Path);
                    Assert.Equal("/", file.BasePath);
                },
                file =>
                {
                    Assert.Equal("/Views/Home/Index.cshtml", file.Path);
                    Assert.Equal("/", file.BasePath);
                });
        }

        [Fact]
        public void EnumerateItems_DiscoversAllCshtmlFiles_UnderSpecifiedBasePath()
        {
            // Arrange
            var fileSystemProject = new FileSystemRazorProject(TestFolder);

            // Act
            var files = fileSystemProject.EnumerateItems("/Views");

            // Assert
            Assert.Collection(files.OrderBy(f => f.Path),
                file =>
                {
                    Assert.Equal("/About/About.cshtml", file.Path);
                    Assert.Equal("/Views", file.BasePath);
                },
                file =>
                {
                    Assert.Equal("/Home/Index.cshtml", file.Path);
                    Assert.Equal("/Views", file.BasePath);
                });
        }

        [Fact]
        public void EnumerateItems_ReturnsEmptySequence_WhenBasePathDoesNotExist()
        {
            // Arrange
            var fileSystemProject = new FileSystemRazorProject(TestFolder);

            // Act
            var files = fileSystemProject.EnumerateItems("/Does-Not-Exist");

            // Assert
            Assert.Empty(files);
        }

        [Fact]
        public void GetItem_ReturnsFileFromDisk()
        {
            // Arrange
            var path = "/Views/About/About.cshtml";
            var fileSystemProject = new FileSystemRazorProject(TestFolder);

            // Act
            var file = fileSystemProject.GetItem(path);

            // Assert
            Assert.True(file.Exists);
            Assert.Equal(path, file.Path);
        }

        [Fact]
        public void GetItem_ReturnsNotFoundResult()
        {
            // Arrange
            var path = "/NotFound.cshtml";
            var fileSystemProject = new FileSystemRazorProject(TestFolder);

            // Act
            var file = fileSystemProject.GetItem(path);

            // Assert
            Assert.False(file.Exists);
        }
    }
}
