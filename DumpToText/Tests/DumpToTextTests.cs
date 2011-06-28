using NUnit.Framework;

namespace DumpToText.Tests
{
	public class DumpToTextTests
	{
		[Test]
		public void Simple_Value_int()
		{
			1.DumpToText().ShouldEqual("1");
		}

		[Test]
		public void Simple_Value_string()
		{
			"HELLO".DumpToText().ShouldEqual("HELLO");
		}

		[Test]
		public void Simple_AnonymousType()
		{
			(new { Foo = 1 }).DumpToText().Trace().ShouldEqual(
@"|----------------------------|
| <>f__AnonymousType0<Int32> |
|----------------------------|
| Foo |                    1 |
|----------------------------|
");
		}

		[Test]
		public void Simple_AnonymousType2()
		{
			(new { Foo = 1, Foo2 = "HELLO" }).DumpToText().Trace().ShouldEqual(
@"|------------------------------------|
| <>f__AnonymousType1<Int32, String> |
|------------------------------------|
|  Foo |                           1 |
|------------------------------------|
| Foo2 |                       HELLO |
|------------------------------------|
");
		}
	}

	public static class Extensions
	{
		public static T Trace<T>(this T item)
		{
			System.Diagnostics.Trace.WriteLine(item.ToString());
			return item;
		}
	}
}