using QueryAsJson.Core;
using QueryAsJson.Core.Compiled;
using System;
using System.Text;
using Xunit;

namespace QueryAsJson.Tests
{
    public class DefinitionTemplateCompilerTests
    {
        [Fact]
        public void ArgumentNullTest()
        {
            Assert.Throws<ArgumentNullException>(() => DefinitionTemplateCompiler.CompileNestedResults(null, null));
        }


        [Fact]
        public void AnonymousTypeTest()
        {
            var mappedObject = new MappedObject();
            var anonObj = new { Field1 = "Test", Field2 = 2 };
            DefinitionTemplateCompiler.CompileProperties(anonObj, mappedObject, null);
            Assert.Equal(mappedObject.MappedPropertyList[0].TargetPropertyName, "Field1");
            Assert.Equal(mappedObject.MappedPropertyList[0].StaticValue, anonObj.Field1);
            Assert.Equal(mappedObject.MappedPropertyList[1].TargetPropertyName, "Field2");
            Assert.Equal(mappedObject.MappedPropertyList[1].StaticValue, anonObj.Field2);

            Assert.Throws<ArgumentException>(() => DefinitionTemplateCompiler.CompileProperties(new[] { "a", "b" }, mappedObject, null));
            Assert.Throws<ArgumentException>(() => DefinitionTemplateCompiler.CompileProperties(new StringBuilder(), mappedObject, null));

        }
    }
}
