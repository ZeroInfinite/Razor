// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    public sealed class ITagHelperDescriptorBuilder
    {
        public static readonly string DescriptorKind = "ITagHelper";
        public static readonly string TypeNameKey = "ITagHelper.TypeName";

        private string _assemblyName;
        private string _typeName;
        private string _documentation;
        private string _tagOutputHint;
        private List<string> _allowedChildTags;
        private List<BoundAttributeDescriptor> _attributeDescriptors;
        private List<TagMatchingRule> _tagMatchingRules;
        private List<RazorDiagnostic> _diagnostics;
        private readonly Dictionary<string, string> _propertyBag;

        private ITagHelperDescriptorBuilder(string typeName, string assemblyName)
        {
            _assemblyName = assemblyName;
            _typeName = typeName;
            _propertyBag = new Dictionary<string, string>(StringComparer.Ordinal);
        }

        public static ITagHelperDescriptorBuilder Create(string typeName, string assemblyName)
        {
            return new ITagHelperDescriptorBuilder(typeName, assemblyName);
        }

        public ITagHelperDescriptorBuilder BindAttribute(Action<ITagHelperBoundAttributeDescriptorBuilder> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var builder = ITagHelperBoundAttributeDescriptorBuilder.Create(_typeName);

            configure(builder);

            var attributeDescriptor = builder.Build();

            EnsureAttributeDescriptors();
            _attributeDescriptors.Add(attributeDescriptor);

            return this;
        }

        public ITagHelperDescriptorBuilder TagMatchingRule(Action<TagMatchingRuleBuilder> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var builder = TagMatchingRuleBuilder.Create();

            configure(builder);

            var rule = builder.Build();

            EnsureTagMatchingRules();
            _tagMatchingRules.Add(rule);

            return this;
        }

        public ITagHelperDescriptorBuilder AllowChildTag(string allowedChild)
        {
            EnsureAllowedChildTags();
            _allowedChildTags.Add(allowedChild);

            return this;
        }

        public ITagHelperDescriptorBuilder TagOutputHint(string hint)
        {
            _tagOutputHint = hint;

            return this;
        }

        public ITagHelperDescriptorBuilder Documentation(string documentation)
        {
            _documentation = documentation;

            return this;
        }

        public ITagHelperDescriptorBuilder AddMetadata(string key, string value)
        {
            _propertyBag[key] = value;

            return this;
        }

        public ITagHelperDescriptorBuilder AddDiagnostic(RazorDiagnostic diagnostic)
        {
            EnsureDiagnostics();
            _diagnostics.Add(diagnostic);

            return this;
        }

        public TagHelperDescriptor Build()
        {
            var descriptor = new ITagHelperDescriptor(
                _typeName,
                _assemblyName,
                _typeName /* Name */,
                _typeName /* DisplayName */,
                _documentation,
                _tagOutputHint,
                _tagMatchingRules ?? Enumerable.Empty<TagMatchingRule>(),
                _attributeDescriptors ?? Enumerable.Empty<BoundAttributeDescriptor>(),
                _allowedChildTags,
                _propertyBag,
                _diagnostics ?? Enumerable.Empty<RazorDiagnostic>());

            return descriptor;
        }

        private void EnsureAttributeDescriptors()
        {
            if (_attributeDescriptors == null)
            {
                _attributeDescriptors = new List<BoundAttributeDescriptor>();
            }
        }

        private void EnsureTagMatchingRules()
        {
            if (_tagMatchingRules == null)
            {
                _tagMatchingRules = new List<TagMatchingRule>();
            }
        }

        private void EnsureAllowedChildTags()
        {
            if (_allowedChildTags == null)
            {
                _allowedChildTags = new List<string>();
            }
        }

        private void EnsureDiagnostics()
        {
            if (_diagnostics == null)
            {
                _diagnostics = new List<RazorDiagnostic>();
            }
        }

        private class ITagHelperDescriptor : TagHelperDescriptor
        {
            public ITagHelperDescriptor(
                string typeName,
                string assemblyName,
                string name,
                string displayName,
                string documentation,
                string tagOutputHint,
                IEnumerable<TagMatchingRule> tagMatchingRules,
                IEnumerable<BoundAttributeDescriptor> attributeDescriptors,
                IEnumerable<string> allowedChildTags,
                Dictionary<string, string> propertyBag,
                IEnumerable<RazorDiagnostic> diagnostics) : base(DescriptorKind)
            {
                AssemblyName = assemblyName;
                Name = typeName;
                DisplayName = displayName;
                Documentation = documentation;
                TagOutputHint = tagOutputHint;
                TagMatchingRules  = tagMatchingRules;
                BoundAttributes = attributeDescriptors;
                AllowedChildTags = allowedChildTags;
                Diagnostics = diagnostics;

                propertyBag[TypeNameKey] = typeName;
                Metadata = propertyBag;
            }
        }
    }
}
