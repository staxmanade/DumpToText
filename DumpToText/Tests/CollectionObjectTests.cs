﻿using System;
using System.Linq;
using NUnit.Framework;

namespace DumpToText.Tests
{
	public class CollectionObjectTests
	{
		[Test]
		public void CollectionWithItems()
		{
			var items = new[] { 1, 2, 3 };
			var dumpItemBase = (CollectionObject) ObjectTypeFactory.Create(items);
			dumpItemBase.Children.Count().ShouldEqual(3);
		}
	}
}