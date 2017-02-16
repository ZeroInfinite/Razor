﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    internal class BoundAttributeDescriptorComparer : IEqualityComparer<BoundAttributeDescriptor>
    {
        public static readonly BoundAttributeDescriptorComparer Default = new BoundAttributeDescriptorComparer();

        protected BoundAttributeDescriptorComparer()
        {
        }

        public bool Equals(BoundAttributeDescriptor descriptorX, BoundAttributeDescriptor descriptorY)
        {
            if (descriptorX == descriptorY)
            {
                return true;
            }

            return descriptorX != null &&
                descriptorX.Kind == descriptorY.Kind &&
                descriptorX.IsKeyValueStringProperty == descriptorY.IsKeyValueStringProperty &&
                descriptorX.IsEnum == descriptorY.IsEnum &&
                string.Equals(descriptorX.Name, descriptorY.Name, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(descriptorX.KeyValueAttributeNamePrefix, descriptorY.KeyValueAttributeNamePrefix, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(descriptorX.TypeName, descriptorY.TypeName, StringComparison.Ordinal) &&
                string.Equals(descriptorX.KeyValueTypeName, descriptorY.KeyValueTypeName, StringComparison.Ordinal) &&
                string.Equals(descriptorX.Documentation, descriptorY.Documentation, StringComparison.Ordinal) &&
                string.Equals(descriptorX.DisplayName, descriptorY.DisplayName, StringComparison.Ordinal) &&
                Enumerable.SequenceEqual(descriptorX.Diagnostics, descriptorY.Diagnostics) &&
                Enumerable.SequenceEqual(
                    descriptorX.Metadata.OrderBy(propertyX => propertyX.Key, StringComparer.Ordinal),
                    descriptorY.Metadata.OrderBy(propertyY => propertyY.Key, StringComparer.Ordinal));
        }

        public int GetHashCode(BoundAttributeDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            var hashCodeCombiner = HashCodeCombiner.Start();
            hashCodeCombiner.Add(descriptor.Kind);
            hashCodeCombiner.Add(descriptor.IsKeyValueStringProperty);
            hashCodeCombiner.Add(descriptor.IsEnum);
            hashCodeCombiner.Add(descriptor.Name, StringComparer.OrdinalIgnoreCase);
            hashCodeCombiner.Add(descriptor.KeyValueAttributeNamePrefix, StringComparer.OrdinalIgnoreCase);
            hashCodeCombiner.Add(descriptor.TypeName, StringComparer.Ordinal);
            hashCodeCombiner.Add(descriptor.KeyValueTypeName, StringComparer.Ordinal);
            hashCodeCombiner.Add(descriptor.Documentation, StringComparer.Ordinal);
            hashCodeCombiner.Add(descriptor.DisplayName, StringComparer.Ordinal);

            return hashCodeCombiner.CombinedHash;
        }
    }
}