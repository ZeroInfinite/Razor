// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    public sealed class ITagHelperBoundAttributeDescriptorBuilder
    {
        public static readonly string DescriptorKind = "ITagHelper";
        public static readonly string PropertyNameKey = "ITagHelper.PropertyName";

        private static readonly IReadOnlyDictionary<string, string> PrimitiveDisplayTypeNameLookups = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            [typeof(byte).FullName] = "byte",
            [typeof(sbyte).FullName] = "sbyte",
            [typeof(int).FullName] = "int",
            [typeof(uint).FullName] = "uint",
            [typeof(short).FullName] = "short",
            [typeof(ushort).FullName] = "ushort",
            [typeof(long).FullName] = "long",
            [typeof(ulong).FullName] = "ulong",
            [typeof(float).FullName] = "float",
            [typeof(double).FullName] = "double",
            [typeof(char).FullName] = "char",
            [typeof(bool).FullName] = "bool",
            [typeof(object).FullName] = "object",
            [typeof(string).FullName] = "string",
            [typeof(decimal).FullName] = "decimal",
        };

        private bool _isEnum;
        private string _dictionaryValueTypeName;
        private string _name;
        private string _propertyName;
        private string _typeName;
        private string _documentation;
        private string _dictionaryAttributeNamePrefix;
        private List<RazorDiagnostic> _diagnostics;
        private readonly string _containingTypeName;
        private readonly Dictionary<string, string> _propertyBag;

        private ITagHelperBoundAttributeDescriptorBuilder(string containingTypeName)
        {
            _containingTypeName = containingTypeName;
            _propertyBag = new Dictionary<string, string>();
        }

        public static ITagHelperBoundAttributeDescriptorBuilder Create(string containingTypeName)
        {
            return new ITagHelperBoundAttributeDescriptorBuilder(containingTypeName);
        }

        public ITagHelperBoundAttributeDescriptorBuilder Name(string name)
        {
            _name = name;

            return this;
        }

        public ITagHelperBoundAttributeDescriptorBuilder PropertyName(string propertyName)
        {
            _propertyName = propertyName;

            return this;
        }

        public ITagHelperBoundAttributeDescriptorBuilder TypeName(string typeName)
        {
            _typeName = typeName;

            return this;
        }

        public ITagHelperBoundAttributeDescriptorBuilder AsEnum()
        {
            _isEnum = true;

            return this;
        }

        public ITagHelperBoundAttributeDescriptorBuilder AsDictionary(string attributeNamePrefix, string valueTypeName)
        {
            _dictionaryAttributeNamePrefix = attributeNamePrefix;
            _dictionaryValueTypeName = valueTypeName;

            return this;
        }

        public ITagHelperBoundAttributeDescriptorBuilder Documentation(string documentation)
        {
            _documentation = documentation;

            return this;
        }

        public ITagHelperBoundAttributeDescriptorBuilder AddMetadata(string key, string value)
        {
            _propertyBag[key] = value;

            return this;
        }

        public BoundAttributeDescriptor Build()
        {
            if (!PrimitiveDisplayTypeNameLookups.TryGetValue(_typeName, out var simpleName))
            {
                simpleName = _typeName;
            }

            var displayName = $"{simpleName} {_containingTypeName}.{_propertyName}";
            var descriptor = new ITagHelperBoundAttributeDescriptor(
                _isEnum,
                _name,
                _propertyName,
                _typeName,
                _dictionaryAttributeNamePrefix,
                _dictionaryValueTypeName,
                _documentation,
                displayName,
                _propertyBag,
                _diagnostics ?? Enumerable.Empty<RazorDiagnostic>());

            return descriptor;
        }

        private class ITagHelperBoundAttributeDescriptor : BoundAttributeDescriptor
        {
            public ITagHelperBoundAttributeDescriptor(
                bool isEnum,
                string name,
                string propertyName,
                string typeName,
                string dictionaryAttributeNamePrefix,
                string dictionaryValueTypeName,
                string documentation,
                string displayName,
                Dictionary<string, string> propertyBag,
                IEnumerable<RazorDiagnostic> diagnostics) : base(DescriptorKind)
            {
                IsEnum = isEnum;
                IsKeyValueStringProperty = dictionaryValueTypeName == typeof(string).FullName || dictionaryValueTypeName == "string";
                IsStringProperty = typeName == typeof(string).FullName || typeName == "string";
                Name = name;
                TypeName = typeName;
                KeyValueAttributeNamePrefix = dictionaryAttributeNamePrefix;
                KeyValueTypeName = dictionaryValueTypeName;
                Documentation = documentation;
                DisplayName = displayName;
                Diagnostics = diagnostics;

                propertyBag[PropertyNameKey] = propertyName;
                Metadata = propertyBag;
            }
        }
    }
}
