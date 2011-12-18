using System.Linq;
using NUnit.Framework;

namespace DumpToText.Tests
{
    [TestFixture]
    public class ReferenceObjectTests
    {

        [Test]
        public void SimpleReferenceObject()
        {
            var foo = new { Foo = 1 };
            var dumpItemBase = DumpToTextExtensions.ObjectTypeFactory.Create(foo);
            dumpItemBase.ShouldBeOfType(typeof (DumpToTextExtensions.ReferenceObject));

            ((DumpToTextExtensions.ReferenceObject)dumpItemBase).Properties.Count().ShouldEqual(1);
        }

        [Test]
        public void SimpleRefWithCollectionProperty()
        {
            var foo = new {Foo = new int[0]};
            var dumpItemBase =(DumpToTextExtensions.ReferenceObject)DumpToTextExtensions.ObjectTypeFactory.Create(foo);
            dumpItemBase.Properties.Count().ShouldEqual(1);
        }
    }
}