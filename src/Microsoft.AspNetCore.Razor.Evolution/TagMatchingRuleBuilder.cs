// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    public sealed class TagMatchingRuleBuilder
    {
        private string _tagName;
        private string _parentTag;
        private TagStructure _tagStructure;
        private List<RazorDiagnostic> _diagnostics;
        private List<RequiredAttributeDescriptor> _requiredAttributeDescriptors;

        private TagMatchingRuleBuilder()
        {
        }

        public static TagMatchingRuleBuilder Create()
        {
            return new TagMatchingRuleBuilder();
        }

        public TagMatchingRuleBuilder RequireTagName(string tagName)
        {
            _tagName = tagName;

            return this;
        }

        public TagMatchingRuleBuilder RequireParentTag(string parentTag)
        {
            _parentTag = parentTag;

            return this;
        }

        public TagMatchingRuleBuilder RequireTagStructure(TagStructure tagStructure)
        {
            _tagStructure = tagStructure;

            return this;
        }

        public TagMatchingRuleBuilder RequireAttribute(Action<RequiredAttributeDescriptorBuilder> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var builder = RequiredAttributeDescriptorBuilder.Create();

            configure(builder);

            var requiredAttributeDescriptor = builder.Build();

            EnsureRequiredAttributeDescriptors();
            _requiredAttributeDescriptors.Add(requiredAttributeDescriptor);

            return this;
        }

        public TagMatchingRuleBuilder AddDiagnostic(RazorDiagnostic diagnostic)
        {
            EnsureDiagnostics();
            _diagnostics.Add(diagnostic);

            return this;
        }

        public TagMatchingRule Build()
        {
            var rule = new DefaultTagMatchingRule(
                _tagName,
                _parentTag,
                _tagStructure,
                _requiredAttributeDescriptors ?? Enumerable.Empty<RequiredAttributeDescriptor>(),
                _diagnostics ?? Enumerable.Empty<RazorDiagnostic>());

            return rule;
        }

        private void EnsureRequiredAttributeDescriptors()
        {
            if (_requiredAttributeDescriptors == null)
            {
                _requiredAttributeDescriptors = new List<RequiredAttributeDescriptor>();
            }
        }

        private void EnsureDiagnostics()
        {
            if (_diagnostics == null)
            {
                _diagnostics = new List<RazorDiagnostic>();
            }
        }

        private class DefaultTagMatchingRule : TagMatchingRule
        {
            public DefaultTagMatchingRule(
                string tagName,
                string parentTag,
                TagStructure tagStructure,
                IEnumerable<RequiredAttributeDescriptor> requiredAttributeDescriptors,
                IEnumerable<RazorDiagnostic> diagnostics)
            {
                TagName = tagName;
                ParentTag = parentTag;
                TagStructure = tagStructure;
                Attributes = requiredAttributeDescriptors;
                Diagnostics = diagnostics;
            }
        }
    }
}
