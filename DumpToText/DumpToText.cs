using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DumpToText
{
	public static class DumpToTextExtensions
	{
		public static string DumpToText(this object item)
		{
			var dumpItemBase = ObjectTypeFactory.Create(item);

			return dumpItemBase.Value;
		}
	}
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

			var enumerableItem = item as IEnumerable;
			if (enumerableItem != null)
				return new CollectionObject(enumerableItem);

			return new ReferenceObject(item);
		}
	}

	public class ReferenceObject : DumpItemBase
	{
		private readonly object _item;
		private Lazy<string> _value;

		public ReferenceObject(object item)
		{
			_item = item;
			_value = new Lazy<string>(() =>
			{
				var sb = new StringBuilder();

				var name = PrettifyTypeName(_item.GetType());
				Trace.WriteLine(name);

				var maxPropertyWidth = Properties.Max(p => p.PropertyInfo.Name.Length);
				var maxValueWidth = Properties.Max(p => p.Value.Width);
				var totalWidth = new[] { name.Length, maxPropertyWidth + maxValueWidth }.Max();
				Trace.WriteLine("totalWidth=" + totalWidth);
				Trace.WriteLine("maxPropertyWidth=" + maxPropertyWidth);
				Trace.WriteLine("maxValueWidth=" + maxValueWidth);

				Action writeDividerLine = () =>
				{
					sb.Append("|");
					sb.Append(new string('-', totalWidth + 2));
					sb.AppendLine("|");
				};

				Action<string> writeTextLine = lineToWrite =>
				{
					sb.Append("| ");
					sb.Append(lineToWrite);
					sb.AppendLine(" |");
				};

				writeDividerLine();
				writeTextLine(name);
				writeDividerLine();

				var valueColumnWidth = totalWidth - 3 - maxPropertyWidth;

				foreach (var child in Properties)
				{
					var eachRowInChildItem = child.Value.Value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

					sb.Append("| ");
					sb.Append(string.Format("{0," + maxPropertyWidth + "}", child.PropertyInfo.Name));
					sb.Append(" | ");
					sb.Append(string.Format("{0,-" + (valueColumnWidth) + "}", eachRowInChildItem.First()));
					sb.AppendLine(" |");

					foreach (var row in eachRowInChildItem.Skip(1))
					{
						sb.Append("| ");
						sb.Append(new string(' ', maxPropertyWidth));
						sb.Append(" | ");
						sb.Append(string.Format("{0,-" + valueColumnWidth + "}", row.TrimEnd('\n')));
						sb.AppendLine(" |");
					}


					writeDividerLine();
				}

				return sb.ToString();
			});
		}

		public override string Value
		{
			get
			{
				return _value.Value;

			}
		}


		public IEnumerable<Property> Properties
		{
			get
			{
				return _item.GetType()
					.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Select(propertyInfo => new Property
												{
													PropertyInfo = propertyInfo,
													Value = ObjectTypeFactory.Create(propertyInfo.GetValue(_item, new object[0]))
												});
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

		public int Width
		{
			get { return (Value ?? "").Split('\n').Select(s => s.Length).Max(); }
		}

		public static string TextForCollectionOf(Type type, int count)
		{
			return PrettifyTypeName(type) + " (" + count + " items)";
		}

		protected static string PrettifyTypeName(Type type)
		{
			if (type.IsGenericType)
				return type.Name.Substring(0, type.Name.Length - 2) + "<" + string.Join(", ", type.GetGenericArguments().Select(PrettifyTypeName).ToArray()) + ">";

			return type.Name;
		}
	}

	public class Property
	{
		public PropertyInfo PropertyInfo { get; set; }
		public DumpItemBase Value { get; set; }
	}

	public class CollectionObject : DumpItemBase
	{
		private readonly IEnumerable _items;
		private readonly Lazy<string> _value;

		public CollectionObject(IEnumerable items)
		{
			_items = items;

			_value = new Lazy<string>(() =>
			{
				var sb = new StringBuilder();

				var name = TextForCollectionOf(_items.GetType(), Children.Count());
				var dumpItems = (from object item in _items
								 select ObjectTypeFactory.Create(item)).ToList();

				var totalWidth = name.Length;
				var valueColumnWidth = dumpItems.Select(s => s.Width).Concat(new[] { totalWidth }).Max();

				Action writeDividerLine = () =>
				{
					sb.Append("|");
					sb.Append(new string('-', totalWidth + 2));
					sb.AppendLine("|");
				};

				Action<string> writeTextLine = lineToWrite =>
				{
					sb.Append("| ");
					sb.Append(string.Format("{0,-" + (valueColumnWidth) + "}", lineToWrite));
					sb.AppendLine(" |");
				};

				writeDividerLine();
				writeTextLine(name);
				writeDividerLine();


				foreach (var dumpItem in dumpItems)
				{
					var eachRowInChildItem = dumpItem.Value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

					writeTextLine(eachRowInChildItem.First());

					foreach (var row in eachRowInChildItem.Skip(1))
					{
						writeTextLine(row.TrimEnd('\n'));
					}

					writeDividerLine();
				}

				return sb.ToString();

			});
		}


		public override string Value
		{
			get { return _value.Value; }
		}

		public IEnumerable<DumpItemBase> Children
		{
			get
			{
				foreach (var item in _items)
				{
					yield return ObjectTypeFactory.Create(item);
				}
			}
		}
	}
}