using System;
using NUnit.Framework;

namespace DumpToText.Tests
{
	public class ValueObjectTests
	{
		[TestCase(typeof(DumpToTextExtensions.ValueObject), 1, "1")]
		[TestCase(typeof(DumpToTextExtensions.ValueObject), "FOO", "FOO")]
		[TestCase(typeof(DumpToTextExtensions.ValueObject), null, DumpToTextExtensions.DumpItemBase.NullValue)]
		public void ValueTests(Type expectedType, object itemValue, string expectedTextValue)
		{
			var dumpItem = DumpToTextExtensions.ObjectTypeFactory.Create(itemValue);
			dumpItem.ShouldBeOfType(expectedType);

			dumpItem.Value.ShouldEqual(expectedTextValue);
		}
	}
}
