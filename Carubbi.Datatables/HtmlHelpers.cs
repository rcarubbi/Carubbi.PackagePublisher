using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace Carubbi.Datatables
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString Table(this HtmlHelper helper, string name, IEnumerable items, object htmlAttributes)
        {
            IDictionary<string, object> htmlAttributesDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (string.IsNullOrEmpty(name))
            {
                return MvcHtmlString.Empty;
            }

            return MvcHtmlString.Create(BuildTable(name, items, htmlAttributesDictionary));
        }

        private static string BuildTable(string name, IEnumerable items, IDictionary<string, object> attributes)
        {
            StringBuilder sb = new StringBuilder();

            Type itemType = items.GetType().GetGenericArguments()[0];

            var properties = itemType.GetProperties()
               .Where(pr => pr.IsDefined(typeof(DataTablesColumnAttribute), false))
               .OrderBy(pr => ((DataTablesColumnAttribute)pr.GetCustomAttributes(typeof(DataTablesColumnAttribute), false).Single()).Order)
               .ToList();

            var allowEdit = attributes.ContainsKey("data-allow-edit") && Convert.ToBoolean(attributes["data-allow-edit"]);
            var allowDelete = attributes.ContainsKey("data-allow-delete") && Convert.ToBoolean(attributes["data-allow-delete"]);
            var allowNew = attributes.ContainsKey("data-allow-new") && Convert.ToBoolean(attributes["data-allow-new"]);


            BuildTableHeader(sb, properties, allowEdit || allowDelete);
            sb.AppendLine("\t<tbody>");
            foreach (var item in items)
            {
                BuildTableRow(sb, item, properties);
            }
            sb.AppendLine("\t</tbody>");

            TagBuilder builder = new TagBuilder("table");
            builder.MergeAttributes(attributes);
            if (allowEdit || allowDelete) builder.MergeAttribute("data-key-properties", GetKeys(properties));
            builder.MergeAttribute("name", name);
            builder.InnerHtml = sb.ToString();

            TagBuilder div = new TagBuilder("div");
            div.InnerHtml = builder.ToString(TagRenderMode.Normal);

            if (allowNew)
            {
                TagBuilder addButton = new TagBuilder("a");
                addButton.AddCssClass("btn btn-success datatables_novo");
                addButton.SetInnerText("Novo");
                div.InnerHtml += addButton.ToString(TagRenderMode.Normal);
            }

            return div.ToString(TagRenderMode.Normal);
        }

        private static string GetKeys(List<PropertyInfo> properties)
        {
            StringBuilder stb = new StringBuilder();
            foreach (var property in properties)
            {
                var attr = (DataTablesColumnAttribute)property.GetCustomAttributes(typeof(DataTablesColumnAttribute), false).Single();
                if (attr.PrimaryKey)
                    stb.AppendFormat("{0}&", property.Name);
            }
            stb = stb.Remove(stb.Length - 1, 1);
            return stb.ToString();
        }

        private static void BuildTableRow(StringBuilder sb, object obj, List<PropertyInfo> properties)
        {
            string tr = "\t<tr>";
            sb.AppendFormat(tr);
            foreach (var property in properties)
            {
                string value = property.GetValue(obj, null).ToString();
                sb.AppendFormat("\t\t<td>{0}</td>\n", value);
            }

            sb.AppendLine("\t</tr>");
        }

        private static void BuildTableHeader(StringBuilder sb, List<PropertyInfo> properties, bool hasButtons)
        {
            sb.AppendLine("\t<thead>\t<tr>");


            foreach (var property in properties)
            {
                var attr = (DataTablesColumnAttribute)property.GetCustomAttributes(typeof(DataTablesColumnAttribute), false).Single();
                sb.AppendFormat("\t\t<th data-property-name='{1}' data-property-visible='{2}' data-type='{3}'>{0}</th>\n",
                    attr.Header ?? property.Name,
                    property.Name,
                    !attr.Hidden,
                    attr.DataType);
            }

            if (hasButtons)
            {
                sb.Append("<th></th>");
            }

            sb.AppendLine("\t</tr>\t</thead>");
        }

    }
}
