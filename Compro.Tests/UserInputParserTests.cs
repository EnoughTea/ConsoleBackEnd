using NUnit.Framework;

namespace Compro.Tests
{
    public class UserInputParserTests
    {
        [Test]
        public void Test()
        {
            var p = new UserInputParser();
            var parseResult = p.Parse("sqrt(0.5, -1000000, \"this is ,simple string\"," +
                " \"and ,this \\\",is \\\"heavily,\\\"nested\\\"\\\" string\")");
        }
    }
}