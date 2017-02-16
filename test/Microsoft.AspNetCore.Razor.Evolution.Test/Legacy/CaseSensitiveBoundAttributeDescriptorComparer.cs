// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Internal;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Evolution.Legacy
{
    internal class CaseSensitiveBoundAttributeDescriptorComparer : BoundAttributeDescriptorComparer
    {
        public new static readonly CaseSensitiveBoundAttributeDescriptorComparer Default =
            new CaseSensitiveBoundAttributeDescriptorComparer();

        private CaseSensitiveBoundAttributeDescriptorComparer()
        {
        }

        public override bool Equals(BoundAttributeDescriptor descriptorX, BoundAttributeDescriptor descriptorY)
        {
            if (descriptorX == descriptorY)
            {
                return true;
            }

            Assert.True(base.Equals(descriptorX, descriptorY));
            Assert.Equal(descriptorX.Name, descriptorY.Name);
            Assert.Equal(descriptorX.KeyValueAttributeNamePrefix, descriptorY.KeyValueAttributeNamePrefix);

            return true;
        }

        public override int GetHashCode(BoundAttributeDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            var hashCodeCombiner = HashCodeCombiner.Start();
            hashCodeCombiner.Add(base.GetHashCode(descriptor));
            hashCodeCombiner.Add(descriptor.Name, StringComparer.Ordinal);
            hashCodeCombiner.Add(descriptor.KeyValueAttributeNamePrefix, StringComparer.Ordinal);

            return hashCodeCombiner.CombinedHash;
        }
    }
}