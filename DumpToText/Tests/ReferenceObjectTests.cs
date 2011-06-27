﻿using System;
using System.Collections.Generic;
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

			((ReferenceObject)dumpItemBase).Properties.Count().ShouldEqual(1);
		}

		[Test]
		public void SimpleRefWithCollectionProperty()
		{
			var foo = new {Foo = new int[0]};
			var dumpItemBase =(ReferenceObject)ObjectTypeFactory.Create(foo);
			dumpItemBase.Properties.Count().ShouldEqual(1);
		}
	}
}