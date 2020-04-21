using System;
using Xunit;

namespace AxisCapacity.Common.Tests
{
    public class ShiftTest
    {
        [Theory]
        [InlineData("am")]
        [InlineData("pm")]
        public void ValidShifts(string shiftName)
        {
            Assert.Equal(shiftName.ToLower(), Shift.From(shiftName).Value().ToLower());
        }

        [Fact]
        public void InvalidShiftThrowsException()
        {
            var exception = Assert.Throws<ArgumentException>(() => Shift.From("pmmmm"));
            Assert.True(exception.Message.Contains(Utils.ToCsv(Shift.Values())));
        }
    }
}