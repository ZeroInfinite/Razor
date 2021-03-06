﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Evolution.Intermediate;

namespace Microsoft.AspNetCore.Razor.Evolution.CodeGeneration
{
    public abstract class TagHelperWriter
    {
        public abstract void WriteInitializeTagHelperStructure(CSharpRenderingContext context, InitializeTagHelperStructureIRNode node);

        public abstract void WriteSetTagHelperProperty(CSharpRenderingContext context, SetTagHelperPropertyIRNode node);

        public abstract void WriteAddTagHelperHtmlAttribute(CSharpRenderingContext context, AddTagHelperHtmlAttributeIRNode node);

        public abstract void WriteCreateTagHelper(CSharpRenderingContext context, CreateTagHelperIRNode node);

        public abstract void WriteExecuteTagHelpers(CSharpRenderingContext context, ExecuteTagHelpersIRNode node);
    }
}
