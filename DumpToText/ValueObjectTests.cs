using System;
using NUnit.Framework;

namespace DumpToText
{
	public class ValueObjectTests
	{
		[TestCase(typeof(ValueObject), 1, "1")]
		[TestCase(typeof(ValueObject), "FOO", "FOO")]
		[TestCase(typeof(ValueObject), null, DumpItemBase.NullValue)]
		public void ValueTests(Type expectedType, object itemValue, string expectedTextValue)
		{
			var dumpItem = ObjectTypeFactory.Create(itemValue);
			dumpItem.ShouldBeOfType(expectedType);

			dumpItem.Text.ShouldEqual(expectedTextValue);
		}
	}
}
