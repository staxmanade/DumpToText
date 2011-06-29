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
		public ValueObject(object @object)
			: base(@object)
		{
		}

		public override string Value
		{
			get
			{
				if (_item == null)
					return "<NULL>";

				return _item.ToString();
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
		private readonly Lazy<string> _value;

		public ReferenceObject(object item)
			: base(item)
		{
			_value = new Lazy<string>(() =>
			{
				var sb = new StringBuilder();


				var maxPropertyNameWidth = Properties.Max(p => p.PropertyInfo.Name.Length);
				var maxPropertyValueWidth = Properties.Max(p => p.Value.ValueWidth);
				var totalWidth = new[] { Name.Length, (maxPropertyNameWidth + 3 + maxPropertyValueWidth) }.Max();
				Trace.WriteLine(Name);
				Trace.WriteLine("totalWidth=" + totalWidth);
				Trace.WriteLine("maxPropertyWidth=" + maxPropertyNameWidth);
				Trace.WriteLine("maxValueWidth=" + maxPropertyValueWidth);

				Action writeDividerLine = () =>
				{
					sb.Append("|");
					sb.Append(new string('-', totalWidth + 2));
					sb.AppendLine("|");
				};

				Action<string> writeTextLine = lineToWrite =>
				{
					sb.Append("| ");
					sb.Append(string.Format("{0,-" + (totalWidth) + "}", lineToWrite));
					sb.AppendLine(" |");
				};

				writeDividerLine();
				writeTextLine(Name);
				writeDividerLine();

				var valueColumnWidth = totalWidth - (maxPropertyNameWidth + 3);

				foreach (var child in Properties)
				{
					var eachRowInChildItem = child.Value.Value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

					sb.Append("| ");
					sb.Append(string.Format("{0," + maxPropertyNameWidth + "}", child.PropertyInfo.Name));
					sb.Append(" | ");
					sb.Append(string.Format("{0,-" + (valueColumnWidth) + "}", eachRowInChildItem.First()));
					sb.AppendLine(" |");

					foreach (var row in eachRowInChildItem.Skip(1))
					{
						sb.Append("| ");
						sb.Append(new string(' ', maxPropertyNameWidth));
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
		protected object _item;

		protected DumpItemBase(object item)
		{
			_item = item;
		}

		public const string NullValue = "<NULL>";
		protected string NullString
		{
			get { return NullValue; }
		}

		public abstract string Value { get; }

		public int ValueWidth
		{
			get
			{
				var childrenWidth = (Value ?? "").Split('\n').Select(s => s.Length).Max();
				return childrenWidth;
			}
		}

		public virtual string Name
		{
			get { return PrettifyTypeName(_item.GetType()); }
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
		private readonly Lazy<string> _value;
		public IEnumerable Items { get { return (IEnumerable)_item; } }

		public CollectionObject(IEnumerable items)
			: base(items)
		{

			_value = new Lazy<string>(() =>
			{
				var sb = new StringBuilder();

				var name = TextForCollectionOf(Items.GetType(), Children.Count());
				var dumpItems = (from object item in Items
								 select ObjectTypeFactory.Create(item)).ToList();

				var totalWidth = name.Length;
				var valueColumnWidth = dumpItems.Select(s => s.ValueWidth).Concat(new[] { totalWidth }).Max();

				Action writeDividerLine = () =>
				{
					sb.Append("|");
					sb.Append(new string('-', valueColumnWidth + 2));
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


				foreach (DumpItemBase dumpItem in dumpItems)
				{
					var eachRowInChildItem = dumpItem.Value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

					writeTextLine(eachRowInChildItem.First());

					foreach (string row in eachRowInChildItem.Skip(1))
					{
						writeTextLine(row);
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
				foreach (var item in Items)
				{
					yield return ObjectTypeFactory.Create(item);
				}
			}
		}
	}
}