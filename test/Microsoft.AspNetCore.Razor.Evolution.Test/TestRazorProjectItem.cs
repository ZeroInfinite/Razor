// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Text;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    public class TestRazorProjectItem : RazorProjectItem
        {
            private readonly string _physicalPath;
            private readonly string _path;

            public TestRazorProjectItem(string path, string physicalPath = null)
            {
                _path = path;
                _physicalPath = physicalPath;
            }

            public override string BasePath => string.Empty;

            public override string Path => _path;

            public override string PhysicalPath => _physicalPath;

            public override bool Exists => true;

            public string Content { get; set; } = "Default content";

            public override Stream Read() => new MemoryStream(Encoding.UTF8.GetBytes(Content));
        }
}
