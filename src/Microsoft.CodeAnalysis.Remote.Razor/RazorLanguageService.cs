﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Evolution;
using Microsoft.AspNetCore.Razor.Evolution.Legacy;
using Razevolution.Tooling;

namespace Microsoft.CodeAnalysis.Remote.Razor
{
    internal class RazorLanguageService : ServiceHubServiceBase
    {
        public RazorLanguageService(Stream stream, IServiceProvider serviceProvider) 
            : base(stream, serviceProvider)
        {
        }

        public async Task<IEnumerable<TagHelperDescriptor>> GetTagHelpersAsync(byte[] projectIdBytes, string projectDebugName, byte[] solutionChecksum, CancellationToken cancellationToken = default(CancellationToken))
        {
            var projectId = ProjectId.CreateFromSerialized(new Guid(projectIdBytes), projectDebugName);

            var solution = await GetSolutionAsync().ConfigureAwait(false);
            var project = solution.GetProject(projectId);

            var compilation = await project.GetCompilationAsync().ConfigureAwait(false);
            var results = new SymbolTableTagHelperDescriptorProvider(compilation).GetTagHelperDescriptors();

            return results;
        }

        public Task<IEnumerable<DirectiveDescriptor>> GetDirectivesAsync(byte[] projectIdBytes, string projectDebugName, byte[] solutionChecksum, CancellationToken cancellationToken = default(CancellationToken))
        {
            var engine = RazorEngine.Create();
            var directives = engine.Features.OfType<IRazorDirectiveFeature>().FirstOrDefault()?.Directives;
            return Task.FromResult<IEnumerable<DirectiveDescriptor>>(directives ?? Enumerable.Empty<DirectiveDescriptor>());
        }

        public Task<GeneratedDocument> GenerateDocumentAsync(byte[] projectIdBytes, string projectDebugName, string filename, string text, byte[] solutionChecksum, CancellationToken cancellationToken = default(CancellationToken))
        {
            var engine = RazorEngine.Create();

            RazorSourceDocument source;
            using (var stream = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(text);
                stream.Write(bytes, 0, bytes.Length);

                stream.Seek(0L, SeekOrigin.Begin);
                source = RazorSourceDocument.ReadFrom(stream, filename, Encoding.UTF8);
            }

            var code = RazorCodeDocument.Create(source);
            engine.Process(code);

            var csharp = code.GetCSharpDocument();
            if (csharp == null)
            {
                throw new InvalidOperationException();
            }

            return Task.FromResult(new GeneratedDocument() { Text = csharp.GeneratedCode, });
        }
    }
}
