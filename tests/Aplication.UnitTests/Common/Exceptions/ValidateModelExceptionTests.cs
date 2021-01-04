using System;
using Application.Common.Exceptions;
using FluentAssertions;
using Xunit;

namespace Application.UnitTests.Common.Exceptions
{
    public class ValidateModelExceptionTests
    {
        [Fact]
        public void DefaultConstructorCreatesAnEmptyErrorDictionary()
        {
            var actual = new ValidateModelException().Errors;

            actual.Keys.Should().BeEquivalentTo(Array.Empty<string>());
        }

    }
}
