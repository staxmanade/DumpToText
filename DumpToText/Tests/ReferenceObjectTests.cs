using System.Linq;
using NUnit.Framework;

namespace DumpToText.Tests
{
	public class ReferenceObjectTests
	{
		public class SimpleFoo
		{
			public int Foo { get; set; }
		}
		[Test]
		public void SimpleReferenceObject()
		{
			var foo = new SimpleFoo { Foo = 1 };
			var dumpItemBase = ObjectTypeFactory.Create(foo);
			dumpItemBase.ShouldBeOfType(typeof (ReferenceObject));

			dumpItemBase.Children.Count().ShouldEqual(1);
		}
	}
}