// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    /// <summary>
    /// Entry point to parse Razor files and generate code.
    /// </summary>
    public class RazorTemplateEngine
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RazorTemplateEngine"/>.
        /// </summary>
        /// <param name="engine">The <see cref="RazorEngine"/>.</param>
        /// <param name="project">The <see cref="RazorProject"/>.</param>
        public RazorTemplateEngine(
            RazorEngine engine,
            RazorProject project)
        {
            Engine = engine;
            Project = project;
        }

        /// <summary>
        /// Gets the <see cref="RazorEngine"/>.
        /// </summary>
        public RazorEngine Engine { get; }

        /// <summary>
        /// Gets the <see cref="RazorProject"/>.
        /// </summary>
        public RazorProject Project { get; }

        public RazorTemplateEngineResult GenerateCode(string path)
        {
            return GenerateCode(path, RazorTemplateEngineOptions.Default);
        }

        /// <summary>
        /// Parses the template specified by the project item <paramref name="path"/>.
        /// </summary>
        public RazorTemplateEngineResult GenerateCode(string path, RazorTemplateEngineOptions options)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(path));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var projectItem = Project.GetItem(path);
            if (!projectItem.Exists)
            {
                throw new InvalidOperationException(Resources.FormatRazorTemplateEngine_ItemCouldNotBeFound(path));
            }

            var codeDocument = CreateCodeDocument(projectItem, options);
            var csharpDocument = CreateCSharpDocument(codeDocument);

            return new RazorTemplateEngineResult(projectItem, codeDocument, csharpDocument);
        }

        /// <summary>
        /// Generates a <see cref="RazorCodeDocument"/> for the specified <paramref name="projectItem"/>.
        /// </summary>
        /// <param name="projectItem">The <see cref="RazorProjectItem"/>.</param>
        /// <param name="options">The <see cref="RazorTemplateEngineOptions"/>.</param>
        /// <returns>The created <see cref="RazorCodeDocument"/>.</returns>
        protected virtual RazorCodeDocument CreateCodeDocument(
            RazorProjectItem projectItem,
            RazorTemplateEngineOptions options)
        {
            if (projectItem == null)
            {
                throw new ArgumentNullException(nameof(projectItem));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var source = CreateSourceDocument(projectItem);
            var imports = GetImports(projectItem, options);

            return RazorCodeDocument.Create(source, imports);
        }

        /// <summary>
        /// Processes the <paramref name="codeDocument"/> to produce a <see cref="RazorCSharpDocument"/>.
        /// </summary>
        /// <param name="codeDocument">The <see cref="RazorCodeDocument"/>.</param>
        /// <returns>The created <see cref="RazorCSharpDocument"/>.</returns>
        protected virtual RazorCSharpDocument CreateCSharpDocument(RazorCodeDocument codeDocument)
        {
            if (codeDocument == null)
            {
                throw new ArgumentNullException(nameof(codeDocument));
            }

            Engine.Process(codeDocument);
            return codeDocument.GetCSharpDocument();
        }

        /// <summary>
        /// Creates a <see cref="RazorSourceDocument"/>.
        /// </summary>
        /// <param name="projectItem">The <see cref="RazorProjectItem"/> to produce the document for.</param>
        /// <returns>The <see cref="RazorSourceDocument"/>.</returns>
        protected virtual RazorSourceDocument CreateSourceDocument(RazorProjectItem projectItem)
        {
            if (projectItem == null)
            {
                throw new ArgumentNullException(nameof(projectItem));
            }

            var path = projectItem.PhysicalPath;
            if (string.IsNullOrEmpty(path))
            {
                path = projectItem.Path;
            }

            RazorSourceDocument source;
            using (var inputStream = projectItem.Read())
            {
                source = RazorSourceDocument.ReadFrom(inputStream, path);
            }

            return source;
        }

        /// <summary>
        /// Gets <see cref="RazorSourceDocument"/> that are applicable to the specified <paramref name="projectItem"/>.
        /// </summary>
        /// <param name="projectItem">The <see cref="RazorProjectItem"/>.</param>
        /// <param name="options">The <see cref="RazorTemplateEngineOptions"/>.</param>
        /// <returns></returns>
        protected virtual IEnumerable<RazorSourceDocument> GetImports(
            RazorProjectItem projectItem,
            RazorTemplateEngineOptions options)
        {
            if (projectItem == null)
            {
                throw new ArgumentNullException(nameof(projectItem));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var importsFileName = options.ImportsFileName;
            if (string.IsNullOrEmpty(importsFileName))
            {
                return Enumerable.Empty<RazorSourceDocument>();
            }

            var result = new List<RazorSourceDocument>();
            if (options.DefaultImports != null)
            {
                result.Add(options.DefaultImports);
            }

            var importProjectItems = Project.FindHierarchicalItems(projectItem.Path, importsFileName);
            foreach (var importItem in importProjectItems)
            {
                if (importItem.Exists)
                {
                    // We want items in descending order. FindHierarchicalItems returns items in ascending order.
                    result.Insert(0, CreateSourceDocument(importItem));
                }
            }

            return result;
        }
    }
}
