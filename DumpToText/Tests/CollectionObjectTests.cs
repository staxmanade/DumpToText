using System;
using System.Linq;
using NUnit.Framework;

namespace DumpToText.Tests
{
	public class CollectionObjectTests
	{
		public void CollectionAssertHelper(Type expectedType, object itemValue, string expectedTextValue)
		{
			var dumpItem = ObjectTypeFactory.Create(itemValue);
			dumpItem.ShouldBeOfType(expectedType);

			dumpItem.Value.ShouldEqual(expectedTextValue);
		}

		[Test]
		public void CollectionObject_EmptyArray()
		{
			CollectionAssertHelper(typeof(CollectionObject), new int[0], DumpItemBase.TextForEmptyCollectionOf(typeof(int[])));
		}

		[Test]
		public void CollectionWithItems()
		{
			var items = new[] {1, 2, 3};
			var dumpItemBase = ObjectTypeFactory.Create(items);
			dumpItemBase.Children.Count().ShouldEqual(3);
		}
	}
}