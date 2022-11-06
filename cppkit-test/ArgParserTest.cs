using Microsoft.VisualStudio.TestTools.UnitTesting;
using cppkit;
using System.Collections.Generic;
using System;

namespace cppkit_test
{
    [TestClass]
    public class ArgParserTest
    {
        [TestMethod]
        public void TestIsArg()
        {
            Assert.IsTrue(ArgParser.IsArg("--class"));
            Assert.IsFalse(ArgParser.IsArg("className"));
        }

        [TestMethod]
        public void TestRemoveDashes()
        {
            Assert.AreEqual("class", ArgParser.RemoveDashes("--class"));
            Assert.AreEqual("class", ArgParser.RemoveDashes("class"));
        }

        [TestMethod]
        public void TestParseArgs()
        {
            string[] goodArgs = new string[]
            {
                "--class",
                "className"
            };
            string[] badArgs = new string[]
            {
                "badClass",
                "BadlassName"
            };
            List<Arg> agrs = ArgParser.ParseArgs(goodArgs);
            Assert.AreEqual(1, agrs.Count);
            Assert.AreEqual("class", agrs[0].ArgName);
        }
    }
}
