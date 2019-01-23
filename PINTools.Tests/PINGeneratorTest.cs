using PinTools;
using System;
using Xunit;

namespace PinTools.Tests
{
    public class PinGeneratorTest
    {
        [Fact]
        public void GeneratePin_ValidPin()
        {

            var pinGenerator = new PinTools.PinGenerator();
            var pin = pinGenerator.GeneratePin();

            Assert.IsType<string>(pin);

            // Check that the value is not one of the barred numbers
            // TODO: Need a better way to test this using multiple runs
            Assert.NotEqual("1111", pin);
            Assert.NotEqual("1234", pin);
            Assert.NotEqual("9999", pin);

            // Convert to a number to test the range
            ushort.TryParse(pin, out ushort pinValue);
            Assert.InRange(pinValue, 0, 9999);



        }
    }
}

