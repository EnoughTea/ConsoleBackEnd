using NUnit.Framework;

namespace ConsoleBackEnd.Tests
{
    public class UserInputParserTests
    {
        private ConsoleCommandParser _parser;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _parser = new ConsoleCommandParser();
        }

        [Test]
        public void JsonTypesShouldBeParsedAndReturnedAsIs()
        {
            var parseResult = _parser.Parse("TestCommand(true, 0.5, -1000000, \"this is ,simple string\"," +
                " \"and ,this \\\",is \\\"heavily,\\\"nested\\\"\\\" string\", [1,2,3])");

            Assert.IsTrue(parseResult.IsSuccess);
            Assert.AreEqual("TestCommand", parseResult.GetOrEmpty().Name);
            CollectionAssert.AreEquivalent(new[] {
                "true", "0.5", "-1000000", "\"this is ,simple string\"",
                "\"and ,this \\\",is \\\"heavily,\\\"nested\\\"\\\" string\"", "[1,2,3]"
            }, parseResult.GetOrEmpty().Args);
        }

        [Test]
        public void ParserShouldTolerateWhitespace()
        {
            var parseResult = _parser.Parse(" mul ( 0.5 , 0.5 ) ");
            Assert.IsTrue(parseResult.IsSuccess);
            Assert.AreEqual("mul", parseResult.GetOrEmpty().Name);
            CollectionAssert.AreEquivalent(new[] { "0.5", "0.5" }, parseResult.GetOrEmpty().Args);
        }
    }
}