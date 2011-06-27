using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;

namespace DumpToText
{
	public class DumpItemBaseTests
	{
		[Test]
		public void Should_return_correct_empty_array_string()
		{
			DumpItemBase.TextForEmptyCollectionOf(typeof(int[])).ShouldEqual("Int32[] (0 items)");
		}

		[Test]
		public void Should_return_correct_empty_collection_of_ints_string()
		{
			DumpItemBase.TextForEmptyCollectionOf(typeof(Collection<int>)).ShouldEqual("Collection<Int32> (0 items)");
		}
		[Test]
		public void Should_return_correct_empty_list_of_strings_string()
		{
			DumpItemBase.TextForEmptyCollectionOf(typeof(List<string>)).ShouldEqual("List<String> (0 items)");
		}

		[Test]
		public void Should_return_correct_empty_dictionary_of_int_object_string()
		{
			DumpItemBase.TextForEmptyCollectionOf(typeof(Dictionary<int, object>)).ShouldEqual("Dictionary<Int32, Object> (0 items)");
		}
	}
}