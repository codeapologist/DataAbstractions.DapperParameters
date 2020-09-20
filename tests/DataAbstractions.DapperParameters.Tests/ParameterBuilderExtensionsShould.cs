using Dapper;
using FluentAssertions;
using Xunit;

namespace DataAbstractions.DapperParameters.Tests
{
    public class ParameterBuilderExtensionsShould
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

            var parameters = sourceObject.CreateParameters();

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

            var parameters = sourceObject.Parameterize()
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

            var parameters = sourceObject.Parameterize()
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

            var parameters = sourceObject.Parameterize()
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
}
