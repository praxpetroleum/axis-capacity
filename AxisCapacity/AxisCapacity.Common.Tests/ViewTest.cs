using System;
using Xunit;

namespace AxisCapacity.Common.Tests
{
    public class ViewTest
    {
        [Theory]
        [InlineData("month")]
        [InlineData("day")]
        [InlineData("week")]
        public void ValidViews(string viewName)
        {
            Assert.Equal(viewName.ToLower(), View.From(viewName).Value().ToLower());
        }

        [Fact]
        public void InvalidViewThrowsException()
        {
            var exception = Assert.Throws<ArgumentException>(() => View.From("miaow"));
            Assert.True(exception.Message.Contains(Utils.ToCsv(View.Values())));
        }
    }
}