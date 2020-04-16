using Xunit;

namespace AxisCapacity.Common.Tests
{
    public class UtilsTest
    {
        [Fact]
        public void CanTurnListIntoCommaSeparatedValuesString()
        {
            var input = new []{"this", "is", "a", "string", "sequence"};

            var result = Utils.ToCsv(input);

            Assert.Equal("this,is,a,string,sequence", result);
        }

        [Fact]
        public void CanTurnListIntoCommaSeparatedPrefixedValuesString()
        {
            var input = new []{"b", "c", "d", "e"};

            var result = Utils.ToCsv(input, "@");

            Assert.Equal("@b,@c,@d,@e", result);
        }

    }
}