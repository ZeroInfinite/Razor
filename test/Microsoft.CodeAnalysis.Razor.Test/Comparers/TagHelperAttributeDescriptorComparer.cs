// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Evolution;
using Microsoft.Extensions.Internal;
using Xunit;

namespace Microsoft.CodeAnalysis.Razor.Workspaces.Test.Comparers
{
    internal class TagHelperAttributeDescriptorComparer : IEqualityComparer<BoundAttributeDescriptor>
    {
        public static readonly TagHelperAttributeDescriptorComparer Default =
            new TagHelperAttributeDescriptorComparer();

        private TagHelperAttributeDescriptorComparer()
        {
        }

        public bool Equals(BoundAttributeDescriptor descriptorX, BoundAttributeDescriptor descriptorY)
        {
            if (descriptorX == descriptorY)
            {
                return true;
            }

            Assert.NotNull(descriptorX);
            Assert.NotNull(descriptorY);
            Assert.Equal(descriptorX.IsIndexer, descriptorY.IsIndexer);
            Assert.Equal(descriptorX.Name, descriptorY.Name, StringComparer.Ordinal);
            Assert.Equal(descriptorX.PropertyName, descriptorY.PropertyName, StringComparer.Ordinal);
            Assert.Equal(descriptorX.TypeName, descriptorY.TypeName, StringComparer.Ordinal);
            Assert.Equal(descriptorX.IsEnum, descriptorY.IsEnum);
            Assert.Equal(descriptorX.IsStringProperty, descriptorY.IsStringProperty);

            return TagHelperAttributeDesignTimeDescriptorComparer.Default.Equals(
                    descriptorX.DesignTimeDescriptor,
                    descriptorY.DesignTimeDescriptor);
        }

        public int GetHashCode(BoundAttributeDescriptor descriptor)
        {
            var hashCodeCombiner = HashCodeCombiner.Start();
            hashCodeCombiner.Add(descriptor.IsIndexer);
            hashCodeCombiner.Add(descriptor.Name, StringComparer.Ordinal);
            hashCodeCombiner.Add(descriptor.PropertyName, StringComparer.Ordinal);
            hashCodeCombiner.Add(descriptor.TypeName, StringComparer.Ordinal);
            hashCodeCombiner.Add(descriptor.IsEnum);
            hashCodeCombiner.Add(descriptor.IsStringProperty);
            hashCodeCombiner.Add(TagHelperAttributeDesignTimeDescriptorComparer.Default.GetHashCode(
                descriptor.DesignTimeDescriptor));

            return hashCodeCombiner;
        }
    }
}