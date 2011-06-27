using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace DumpToText
{
	public class ValueObject : DumpItemBase
	{
		private readonly object _o;

		public ValueObject(object @object)
		{
			_o = @object;
		}

		public override string Value
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

			if (item.GetType().IsValueType)
				return new ValueObject(item);

			if (item is string)
				return new ValueObject(item);

			if (item is IEnumerable)
				return new CollectionObject(item);

			return new ReferenceObject(item);
		}
	}

	public class ReferenceObject : DumpItemBase
	{
		private readonly object _item;

		public ReferenceObject(object item)
		{
			_item = item;
		}

		public override string Value
		{
			get { return PrettifyTypeName(_item.GetType()); }
		}


		public override IEnumerable<DumpItemBase> Children
		{
			get
			{
				return _item.GetType()
					.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Select(propertyInfo => ObjectTypeFactory.Create(propertyInfo.GetValue(_item, new object[0])));
			}
		}
	}

	public abstract class DumpItemBase
	{
		public const string NullValue = "<NULL>";
		protected string NullString
		{
			get { return NullValue; }
		}

		public abstract string Value { get; }
		public virtual IEnumerable<DumpItemBase> Children
		{
			get
			{
				return new DumpItemBase[0];
			}
		}

		public static string TextForEmptyCollectionOf(Type type)
		{
			return PrettifyTypeName(type) + " (0 items)";
		}

		protected static string PrettifyTypeName(Type type)
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

		public override string Value
		{
			get
			{
				return TextForEmptyCollectionOf(_item.GetType());
			}
		}

	}
}