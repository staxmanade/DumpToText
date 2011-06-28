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
| Foo | 1                    |
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
|  Foo | 1                           |
|------------------------------------|
| Foo2 | HELLO                       |
|------------------------------------|
");
		}

		[Test]
		public void Simple_AnonymousType_with_multiline_string()
		{
			(new { Foo = 1, Foo2 = @"HELLO
World
How
Are
You" }).DumpToText().Trace().ShouldEqual(
@"|------------------------------------|
| <>f__AnonymousType1<Int32, String> |
|------------------------------------|
|  Foo | 1                           |
|------------------------------------|
| Foo2 | HELLO                       |
|      | World                       |
|      | How                         |
|      | Are                         |
|      | You                         |
|------------------------------------|
");
		}

		[Test]
		public void Simple_Nested_AnonymousType2()
		{
			(new { Foo = 1, Foo2 = new { Bar = 1.234m } }).DumpToText().Trace().ShouldEqual(
@"|----------------------------------------------------------|
| <>f__AnonymousType1<Int32, <>f__AnonymousType2<Decimal>> |
|----------------------------------------------------------|
|  Foo | 1                                                 |
|----------------------------------------------------------|
| Foo2 | |------------------------------|                  |
|      | | <>f__AnonymousType2<Decimal> |                  |
|      | |------------------------------|                  |
|      | | Bar | 1.234                  |                  |
|      | |------------------------------|                  |
|----------------------------------------------------------|
");
		}


		[Test]
		public void Simple_Nested_AnonymousType_with_multiline_string()
		{
			(new
			{
				Foo = 1,
				Foo2 = new
				{
					Bar = @"HELLO
World
How
Are
You"
				}
			}).DumpToText().Trace().ShouldEqual(
@"|---------------------------------------------------------|
| <>f__AnonymousType1<Int32, <>f__AnonymousType2<String>> |
|---------------------------------------------------------|
|  Foo | 1                                                |
|---------------------------------------------------------|
| Foo2 | |-----------------------------|                  |
|      | | <>f__AnonymousType2<String> |                  |
|      | |-----------------------------|                  |
|      | | Bar | HELLO                 |                  |
|      | |     | World                 |                  |
|      | |     | How                   |                  |
|      | |     | Are                   |                  |
|      | |     | You                   |                  |
|      | |-----------------------------|                  |
|---------------------------------------------------------|
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