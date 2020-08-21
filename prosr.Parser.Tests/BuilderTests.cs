using System.IO;
using Xunit;

namespace prosr.Parser.Tests
{
    public class BuilderTests
    {
        [Fact]
        public void Build()
        {
            var builder = new Builder();
            var ast = builder.Build(File.ReadAllText("HelloWorldHub.prosr"));

        }
    }
}
