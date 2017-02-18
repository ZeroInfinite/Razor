// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Evolution.Intermediate;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    public class RazorEngineConfigurationFeature : IRazorEngineFeature
    {
        public RazorEngine Engine { get; set; }

        public IList<Action<ClassDeclarationIRNode>> ConfigureClass { get; } = new List<Action<ClassDeclarationIRNode>>();

        public IList<Action<NamespaceDeclarationIRNode>> ConfigureNamespace { get; } = new List<Action<NamespaceDeclarationIRNode>>();

        public IList<Action<RazorMethodDeclarationIRNode>> ConfigureMethod { get; } = new List<Action<RazorMethodDeclarationIRNode>>();
    }
}
