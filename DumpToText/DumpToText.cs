using System;
using System.Collections;
using System.Linq;

namespace DumpToText
{
	public class ValueObject : DumpItemBase
	{
		private readonly object _o;

		public ValueObject(object @object)
		{
			_o = @object;
		}

		public override string Text
		{
			get
			{
				if (_o == null)
					return "<NULL>";

				return _o.ToString();
			}
		}
	}

	public class ObjectTypeFactory
	{
		public static DumpItemBase Create(object item)
		{
			if (item == null)
				return new ValueObject(item);

			if (item is string)
				return new ValueObject(item);

			if (item is IEnumerable)
				return new CollectionObject(item);

			return new ValueObject(item);
		}
	}

	public abstract class DumpItemBase
	{
		public const string NullValue = "<NULL>";
		protected string NullString
		{
			get { return NullValue; }
		}

		public abstract string Text { get; }

		public static string TextForEmptyCollectionOf(Type type)
		{
			return PrettifyTypeName(type) + " (0 items)";
		}

		private static string PrettifyTypeName(Type type)
		{
			if (type.IsGenericType)
				return type.Name.Substring(0, type.Name.Length - 2) + "<" + string.Join(", ", type.GetGenericArguments().Select(PrettifyTypeName).ToArray()) + ">";

			return type.Name;
		}
	}

	public class CollectionObject : DumpItemBase
	{
		private readonly object _item;

		public CollectionObject(object item)
		{
			_item = item;
		}

		public override string Text
		{
			get
			{
				return TextForEmptyCollectionOf(_item.GetType());
			}
		}
	}
}