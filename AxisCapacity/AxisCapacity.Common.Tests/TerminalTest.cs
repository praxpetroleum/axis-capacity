using System;
using Xunit;

namespace AxisCapacity.Common.Tests
{
    public class TerminalTest
    {
        [Theory]
        [InlineData("grays")]
        [InlineData("dagenham")]
        [InlineData("westerleigh")]
        [InlineData("kingsbury")]
        [InlineData("immingham")]
        [InlineData("jarrow")]
        [InlineData("grangemouth")]
        public void ValidTerminals(string terminalName)
        {
            Assert.Equal(terminalName.ToLower(), Terminal.From(terminalName).Value().ToLower());
        }

        [Fact]
        public void InValidTerminalThrowsException()
        {
            var exception = Assert.Throws<ArgumentException>(() => Terminal.From("someValue"));
            Assert.True(exception.Message.Contains(Utils.ToCsv(Terminal.Values())));
        }
    }
}