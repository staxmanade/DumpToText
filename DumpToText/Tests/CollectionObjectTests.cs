using System;
using NUnit.Framework;

namespace DumpToText.Tests
{
	public class CollectionObjectTests
	{
		public void CollectionAssertHelper(Type expectedType, object itemValue, string expectedTextValue)
		{
			var dumpItem = ObjectTypeFactory.Create(itemValue);
			dumpItem.ShouldBeOfType(expectedType);

			dumpItem.Text.ShouldEqual(expectedTextValue);
		}

		[Test]
		public void CollectionObject_EmptyArray()
		{
			CollectionAssertHelper(typeof(CollectionObject), new int[0], DumpItemBase.TextForEmptyCollectionOf(typeof(int[])));
		}
	}
}