// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    internal static class TagHelperDescriptorMatchingConventions
    {
        public static bool CanMatchName(this BoundAttributeDescriptor descriptor, string name)
        {
            if (string.Equals(descriptor.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // See if the descriptor is associated with a dictinoary 
            if (descriptor.KeyValueAttributeNamePrefix != null &&
                name.StartsWith(descriptor.KeyValueAttributeNamePrefix, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
