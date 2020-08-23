using System;
using System.Collections.Generic;
using System.Text;

namespace TestTask
{
    class Tests
    {
        [Fact]
        public void One()
        {
            Assert.Contains("Discriminant less than zero.", Program.Calculations(1, 1, 1));
        }

        [Fact]
        public void Two()
        {
            Assert.Contains("-3.7320", Program.Calculations(1, 4, 1));
            Assert.Contains("-0.2679", Program.Calculations(1, 4, 1));
        }
    }
}
