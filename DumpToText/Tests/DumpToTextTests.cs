using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace DumpToText.Tests
{
    [TestFixture]
    public class DumpToTextTests
    {
        [Test]
        public void Simple_Value_int()
        {
            1.DumpToTextValue().ShouldEqual("1");
        }

        [Test]
        public void Simple_Value_string()
        {
            "HELLO".DumpToTextValue().ShouldEqual("HELLO");
        }

        [Test]
        public void Simple_AnonymousType()
        {
            (new { Foo = 1 }).DumpToTextValue().Trace().ShouldEqual(
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
            (new { Foo = 1, Foo2 = "HELLO" }).DumpToTextValue().Trace().ShouldEqual(
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
            (new
            {
                Foo = 1,
                Foo2 = @"HELLO
World
How
Are
You"
            }).DumpToTextValue().Trace().ShouldEqual(
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
            (new { Foo = 1, Foo2 = new { Bar = 1.234m } }).DumpToTextValue().Trace().ShouldEqual(
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
            }).DumpToTextValue().ShouldEqual(
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

        [Test]
        public void Simple_empty_array()
        {
            (new int[0]).DumpToTextValue().Trace().ShouldEqual(
@"|-------------------|
| Int32[] (0 items) |
|-------------------|
");
        }


        [Test]
        public void Simple_array_one_item()
        {
            (new[] { 1 }).DumpToTextValue().Trace().ShouldEqual(
@"|-------------------|
| Int32[] (1 items) |
|-------------------|
| 1                 |
|-------------------|
");
        }

        [Test]
        public void Simple_Dictionary()
        {
            var dictionary = new Dictionary<int, string>
		    {
		        {1, "FOO"},
		        {2, "FOOTee Doo"},
		        {10, "FOO"},
		        {100, "FOO"},
		        {1000, "FOO"},
		        {10000, "FOO"},
		    };

            dictionary.DumpToTextValue().Trace();
        }


        [Test]
        public void Simple_collection_of_custom_objects()
        {
            var items = new[]
			{
				new {Name = "FOO", Value = 1m},
				new {Name = "Bar", Value = 1.45m},
			};

            items.DumpToTextValue().Trace().ShouldEqual(
                @"|-------------------------------------------|
| <>f__AnonymousType3`2[] (2 items)         |
|-------------------------------------------|
| |--------------------------------------|  |
| | <>f__AnonymousType3<String, Decimal> |  |
| |--------------------------------------|  |
| |  Name | FOO                          |  |
| |--------------------------------------|  |
| | Value | 1                            |  |
| |--------------------------------------|  |
|-------------------------------------------|
| |--------------------------------------|  |
| | <>f__AnonymousType3<String, Decimal> |  |
| |--------------------------------------|  |
| |  Name | Bar                          |  |
| |--------------------------------------|  |
| | Value | 1.45                         |  |
| |--------------------------------------|  |
|-------------------------------------------|
");
        }

        [Test]
        public void ComplexType()
        {
            ComplexTypeParent complexTypeParent = GetComplexTypeParent();
            complexTypeParent.DumpToTextValue().Trace().ShouldEqual(
@"|-------------------------------------------------------------------|
| ComplexTypeParent                                                 |
|-------------------------------------------------------------------|
|                  Name | SomeName                                  |
|-------------------------------------------------------------------|
|           ListOfItems | |------------------------|                |
|                       | | List<String> (3 items) |                |
|                       | |------------------------|                |
|                       | | a                      |                |
|                       | |------------------------|                |
|                       | | b                      |                |
|                       | |------------------------|                |
|                       | | c                      |                |
|                       | |------------------------|                |
|-------------------------------------------------------------------|
| SomeDictionaryOfStuff | |--------------------------------------|  |
|                       | | Dictionary<String, String> (3 items) |  |
|                       | |--------------------------------------|  |
|                       | | [a, 1]                               |  |
|                       | |--------------------------------------|  |
|                       | | [b, 10]                              |  |
|                       | |--------------------------------------|  |
|                       | | [c, 100]                             |  |
|                       | |--------------------------------------|  |
|-------------------------------------------------------------------|
|   ComplexChildObjects | |------------------------------------|    |
|                       | | List<ComplexChildObject> (3 items) |    |
|                       | |------------------------------------|    |
|                       | | |--------------------|             |    |
|                       | | | ComplexChildObject |             |    |
|                       | | |--------------------|             |    |
|                       | | |  Name | FOO        |             |    |
|                       | | |--------------------|             |    |
|                       | | | Value | 1.2        |             |    |
|                       | | |--------------------|             |    |
|                       | |------------------------------------|    |
|                       | | |--------------------|             |    |
|                       | | | ComplexChildObject |             |    |
|                       | | |--------------------|             |    |
|                       | | |  Name | Hello      |             |    |
|                       | | |--------------------|             |    |
|                       | | | Value | 10.2       |             |    |
|                       | | |--------------------|             |    |
|                       | |------------------------------------|    |
|                       | | |--------------------|             |    |
|                       | | | ComplexChildObject |             |    |
|                       | | |--------------------|             |    |
|                       | | |  Name | World      |             |    |
|                       | | |--------------------|             |    |
|                       | | | Value | 100.2      |             |    |
|                       | | |--------------------|             |    |
|                       | |------------------------------------|    |
|-------------------------------------------------------------------|
");
        }

        [Test]
        public void ComplexType_ListOf()
        {
            var items = new List<ComplexTypeParent>
			{
				GetComplexTypeParent(),
				GetComplexTypeParent(),
			};

            items.DumpToTextValue().Trace();
        }

        private ComplexTypeParent GetComplexTypeParent()
        {
            return new ComplexTypeParent
                    {
                        Name = "SomeName",
                        ComplexChildObjects = new List<ComplexChildObject>
			       		                      	{
			       		                      		new ComplexChildObject {Name = "FOO", Value = 1.2m},
			       		                      		new ComplexChildObject {Name = "Hello", Value = 10.2m},
			       		                      		new ComplexChildObject {Name = "World", Value = 100.2m},
			       		                      	},
                        ListOfItems = new List<string> { "a", "b", "c" },
                        SomeDictionaryOfStuff = new Dictionary<string, string>
			       		                        	{
			       		                        		{"a", "1"},
			       		                        		{"b", "10"},
			       		                        		{"c", "100"},
			       		                        	}
                    };
        }

        public class ComplexTypeParent
        {
            public string Name { get; set; }
            public IEnumerable<string> ListOfItems { get; set; }
            public IDictionary<string, string> SomeDictionaryOfStuff { get; set; }
            public List<ComplexChildObject> ComplexChildObjects { get; set; }
        }
        public class ComplexChildObject
        {
            public string Name { get; set; }
            public decimal Value { get; set; }
        }


    }

    public static class Extensions
    {
        public static T Trace<T>(this T item)
        {
#if SILVERLIGHT
            Console.WriteLine(item.ToString());
#else
            System.Diagnostics.Trace.WriteLine(item.ToString());
#endif
            return item;
        }
    }
}