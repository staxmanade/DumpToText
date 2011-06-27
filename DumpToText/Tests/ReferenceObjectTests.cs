using System.Linq;
using NUnit.Framework;

namespace DumpToText.Tests
{
	public class ReferenceObjectTests
	{

		[Test]
		public void SimpleReferenceObject()
		{
			var foo = new { Foo = 1 };
			var dumpItemBase = ObjectTypeFactory.Create(foo);
			dumpItemBase.ShouldBeOfType(typeof (ReferenceObject));

			dumpItemBase.Children.Count().ShouldEqual(1);
		}

		[Test]
		public void SimpleRefWithCollectionProperty()
		{
			var foo = new {Foo = new int[0]};
			var dumpItemBase = ObjectTypeFactory.Create(foo);
			dumpItemBase.Children.Count().ShouldEqual(1);
			dumpItemBase.Children.First().Children.Count().ShouldEqual(0);
		}
	}
}