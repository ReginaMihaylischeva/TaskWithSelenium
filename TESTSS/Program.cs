using System;
using Test;
using Xunit;

namespace TESTSS
{
    public class Tests
    {
        [Fact]
        public void One()
        {
            Assert.Contains("Discriminant less than zero.", Program.Calculations(1, 1, 1));
        }

        [Fact]
        public void Two()
        {
            string rezult = Program.Calculations(1, 4, 1);
            Assert.Contains("-3,7320", rezult);
            Assert.Contains("-0,2679", rezult);
        }

        [Fact]
        public void Three()
        {
            Assert.Contains("Discriminant less than zero.", Program.Calculations(1, 0, 1));
        }

        [Fact]
        public void Four()
        {
            string rezult = Program.Calculations(2, 5, -3.5);
            Assert.Contains("0,57002", rezult);
            Assert.Contains("-3,0700", rezult);

        }

        [Fact]
        public void Five()
        {
            string rezult = Program.Calculations(1, 5, 0);
            Assert.Contains("-5", rezult);
            Assert.Contains("0", rezult);

        }

        [Fact]
        public void Six()
        {
            string rezult = Program.Calculations(0, 6, 9);
            Assert.Contains("-1,5", rezult);
            Assert.Contains("-1,5", rezult);

        }
    }
}
