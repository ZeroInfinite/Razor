// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    public abstract class RequiredAttributeDescriptor
    {
        public string Name { get; protected set; }

        public NameComparisonMode NameComparison { get; protected set; }

        public string Value { get; protected set; }

        public ValueComparisonMode ValueComparison { get; protected set; }

        public IEnumerable<RazorDiagnostic> Diagnostics { get; protected set; }

        public bool HasAnyErrors
        {
            get
            {
                var anyErrors = Diagnostics.Any(diagnostic => diagnostic.Severity == RazorDiagnosticSeverity.Error);

                return anyErrors;
            }
        }

        /// <summary>
        /// Determines if the current <see cref="RequiredAttributeDescriptor"/> matches the given
        /// <paramref name="attributeName"/> and <paramref name="attributeValue"/>.
        /// </summary>
        /// <param name="attributeName">An HTML attribute name.</param>
        /// <param name="attributeValue">An HTML attribute value.</param>
        /// <returns><c>true</c> if the current <see cref="RequiredAttributeDescriptor"/> matches
        /// <paramref name="attributeName"/> and <paramref name="attributeValue"/>; <c>false</c> otherwise.</returns>
        public bool IsMatch(string attributeName, string attributeValue)
        {
            var nameMatches = false;
            if (NameComparison == NameComparisonMode.FullMatch)
            {
                nameMatches = string.Equals(Name, attributeName, StringComparison.OrdinalIgnoreCase);
            }
            else if (NameComparison == NameComparisonMode.PrefixMatch)
            {
                // attributeName cannot equal the Name if comparing as a PrefixMatch.
                nameMatches = attributeName.Length != Name.Length &&
                    attributeName.StartsWith(Name, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                Debug.Assert(false, "Unknown name comparison.");
            }

            if (!nameMatches)
            {
                return false;
            }

            switch (ValueComparison)
            {
                case ValueComparisonMode.None:
                    return true;
                case ValueComparisonMode.PrefixMatch: // Value starts with
                    return attributeValue.StartsWith(Value, StringComparison.Ordinal);
                case ValueComparisonMode.SuffixMatch: // Value ends with
                    return attributeValue.EndsWith(Value, StringComparison.Ordinal);
                case ValueComparisonMode.FullMatch: // Value equals
                    return string.Equals(attributeValue, Value, StringComparison.Ordinal);
                default:
                    Debug.Assert(false, "Unknown value comparison.");
                    return false;
            }
        }

        /// <summary>
        /// Acceptable <see cref="RequiredAttributeDescriptor.Name"/> comparison modes.
        /// </summary>
        public enum NameComparisonMode
        {
            /// <summary>
            /// HTML attribute name case insensitively matches <see cref="RequiredAttributeDescriptor.Name"/>.
            /// </summary>
            FullMatch,

            /// <summary>
            /// HTML attribute name case insensitively starts with <see cref="RequiredAttributeDescriptor.Name"/>.
            /// </summary>
            PrefixMatch,
        }

        /// <summary>
        /// Acceptable <see cref="RequiredAttributeDescriptor.Value"/> comparison modes.
        /// </summary>
        public enum ValueComparisonMode
        {
            /// <summary>
            /// HTML attribute value always matches <see cref="RequiredAttributeDescriptor.Value"/>.
            /// </summary>
            None,

            /// <summary>
            /// HTML attribute value case sensitively matches <see cref="RequiredAttributeDescriptor.Value"/>.
            /// </summary>
            FullMatch,

            /// <summary>
            /// HTML attribute value case sensitively starts with <see cref="RequiredAttributeDescriptor.Value"/>.
            /// </summary>
            PrefixMatch,

            /// <summary>
            /// HTML attribute value case sensitively ends with <see cref="RequiredAttributeDescriptor.Value"/>.
            /// </summary>
            SuffixMatch,
        }
    }
}
