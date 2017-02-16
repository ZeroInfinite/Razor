using Xunit;

namespace Microsoft.AspNetCore.Razor.Evolution
{
    public class TagHelperRequiredAttributeDescriptorTest
    {
        public static TheoryData RequiredAttributeDescriptorData
        {
            get
            {
                // requiredAttributeDescriptor, attributeName, attributeValue, expectedResult
                return new TheoryData<RequiredAttributeDescriptor, string, string, bool>
                {
                    {
                        new RequiredAttributeDescriptor
                        {
                            Name = "key"
                        },
                        "KeY",
                        "value",
                        true
                    },
                    {
                        new RequiredAttributeDescriptor
                        {
                            Name = "key"
                        },
                        "keys",
                        "value",
                        false
                    },
                    {
                        new RequiredAttributeDescriptor
                        {
                            Name = "route-",
                            NameComparison = TagHelperRequiredAttributeNameComparison.PrefixMatch,
                        },
                        "ROUTE-area",
                        "manage",
                        true
                    },
                    {
                        new RequiredAttributeDescriptor
                        {
                            Name = "route-",
                            NameComparison = TagHelperRequiredAttributeNameComparison.PrefixMatch,
                        },
                        "routearea",
                        "manage",
                        false
                    },
                    {
                        new RequiredAttributeDescriptor
                        {
                            Name = "route-",
                            NameComparison = TagHelperRequiredAttributeNameComparison.PrefixMatch,
                        },
                        "route-",
                        "manage",
                        false
                    },
                    {
                        new RequiredAttributeDescriptor
                        {
                            Name = "key",
                            NameComparison = TagHelperRequiredAttributeNameComparison.FullMatch,
                        },
                        "KeY",
                        "value",
                        true
                    },
                    {
                        new RequiredAttributeDescriptor
                        {
                            Name = "key",
                            NameComparison = TagHelperRequiredAttributeNameComparison.FullMatch,
                        },
                        "keys",
                        "value",
                        false
                    },
                    {
                        new RequiredAttributeDescriptor
                        {
                            Name = "key",
                            NameComparison = TagHelperRequiredAttributeNameComparison.FullMatch,
                            Value = "value",
                            ValueComparison = TagHelperRequiredAttributeValueComparison.FullMatch,
                        },
                        "key",
                        "value",
                        true
                    },
                    {
                        new RequiredAttributeDescriptor
                        {
                            Name = "key",
                            NameComparison = TagHelperRequiredAttributeNameComparison.FullMatch,
                            Value = "value",
                            ValueComparison = TagHelperRequiredAttributeValueComparison.FullMatch,
                        },
                        "key",
                        "Value",
                        false
                    },
                    {
                        new RequiredAttributeDescriptor
                        {
                            Name = "class",
                            NameComparison = TagHelperRequiredAttributeNameComparison.FullMatch,
                            Value = "btn",
                            ValueComparison = TagHelperRequiredAttributeValueComparison.PrefixMatch,
                        },
                        "class",
                        "btn btn-success",
                        true
                    },
                    {
                        new RequiredAttributeDescriptor
                        {
                            Name = "class",
                            NameComparison = TagHelperRequiredAttributeNameComparison.FullMatch,
                            Value = "btn",
                            ValueComparison = TagHelperRequiredAttributeValueComparison.PrefixMatch,
                        },
                        "class",
                        "BTN btn-success",
                        false
                    },
                    {
                        new RequiredAttributeDescriptor
                        {
                            Name = "href",
                            NameComparison = TagHelperRequiredAttributeNameComparison.FullMatch,
                            Value = "#navigate",
                            ValueComparison = TagHelperRequiredAttributeValueComparison.SuffixMatch,
                        },
                        "href",
                        "/home/index#navigate",
                        true
                    },
                    {
                        new RequiredAttributeDescriptor
                        {
                            Name = "href",
                            NameComparison = TagHelperRequiredAttributeNameComparison.FullMatch,
                            Value = "#navigate",
                            ValueComparison = TagHelperRequiredAttributeValueComparison.SuffixMatch,
                        },
                        "href",
                        "/home/index#NAVigate",
                        false
                    },
                };
            }
        }

        [Theory]
        [MemberData(nameof(RequiredAttributeDescriptorData))]
        public void Matches_ReturnsExpectedResult(
            object requiredAttributeDescriptor,
            string attributeName,
            string attributeValue,
            bool expectedResult)
        {
            // Act
            var result = ((RequiredAttributeDescriptor)requiredAttributeDescriptor).IsMatch(attributeName, attributeValue);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
