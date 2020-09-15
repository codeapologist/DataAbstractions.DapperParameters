using Dapper;
using FluentAssertions;
using Xunit;

namespace DataAbstractions.DapperParameters.Tests
{
    public class FluentParameterExtensionsShould
    {
        [Fact]
        public void IncludeAllPropertiesOfSourceObject()
        {
            var sourceObject = new ExampleClass
            {
                IntegerProperty = 123,
                StringProperty = "Some Text",
                BoolProperty = true
            };

            var parameters = sourceObject.BuildParameters().Create();

            var expected = new DynamicParameters();
            expected.Add(nameof(sourceObject.IntegerProperty).ToLowerInvariant(), sourceObject.IntegerProperty);
            expected.Add(nameof(sourceObject.StringProperty).ToLowerInvariant(), sourceObject.StringProperty);
            expected.Add(nameof(sourceObject.BoolProperty).ToLowerInvariant(), sourceObject.BoolProperty);

            parameters.Should().BeOfType<DynamicParameters>();
            parameters.Should().BeEquivalentTo(expected);

        }

        [Fact]
        public void IncludeAddedProperties()
        {
            var sourceObject = new ExampleClass
            {
                IntegerProperty = 123,
                StringProperty = "Some Text",
                BoolProperty = true
            };

            var parameters = sourceObject.BuildParameters()
                .Add("NewParameter", "New Value")
                .Create();

            var expected = new DynamicParameters();
            expected.Add(nameof(sourceObject.IntegerProperty).ToLowerInvariant(), sourceObject.IntegerProperty);
            expected.Add(nameof(sourceObject.StringProperty).ToLowerInvariant(), sourceObject.StringProperty);
            expected.Add(nameof(sourceObject.BoolProperty).ToLowerInvariant(), sourceObject.BoolProperty);
            expected.Add("newparameter", "New Value");

            parameters.Should().BeOfType<DynamicParameters>();
            parameters.Should().BeEquivalentTo(expected);

        }

        [Fact]
        public void ExcludeRemovedProperties()
        {
            var sourceObject = new ExampleClass
            {
                IntegerProperty = 123,
                StringProperty = "Some Text",
                BoolProperty = true
            };

            var parameters = sourceObject.BuildParameters()
                .Remove(x => x.IntegerProperty)
                .Create();

            var expected = new DynamicParameters();
            expected.Add(nameof(sourceObject.StringProperty).ToLowerInvariant(), sourceObject.StringProperty);
            expected.Add(nameof(sourceObject.BoolProperty).ToLowerInvariant(), sourceObject.BoolProperty);

            parameters.Should().BeOfType<DynamicParameters>();
            parameters.Should().BeEquivalentTo(expected);

        }

        [Fact]
        public void UpdateReplacedProperties()
        {
            var sourceObject = new ExampleClass
            {
                IntegerProperty = 123,
                StringProperty = "Some Text",
                BoolProperty = true
            };

            var parameters = sourceObject.BuildParameters()
                .Replace(x => x.StringProperty, "Updated")
                .Create();

            var expected = new DynamicParameters();
            expected.Add(nameof(sourceObject.IntegerProperty).ToLowerInvariant(), sourceObject.IntegerProperty);
            expected.Add(nameof(sourceObject.StringProperty).ToLowerInvariant(), "Updated");
            expected.Add(nameof(sourceObject.BoolProperty).ToLowerInvariant(), sourceObject.BoolProperty);

            parameters.Should().BeOfType<DynamicParameters>();
            parameters.Should().BeEquivalentTo(expected);

        }
    }

    public class ExampleClass
    {
        public int IntegerProperty { get; set; }
        public string StringProperty { get; set; }
        public bool BoolProperty { get; set; }
    }
}
