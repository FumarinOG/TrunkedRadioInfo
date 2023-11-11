using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace DataLibrary
{
    public static class Extensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            var dataTableSkip = new DataTableSkip();

            foreach (PropertyDescriptor property in properties)
            {
                if (!property.Attributes.Contains(dataTableSkip))
                {
                    table.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                }
            }

            foreach (var item in data)
            {
                var row = table.NewRow();

                foreach (PropertyDescriptor property in properties)
                {
                    if (!property.Attributes.Contains(dataTableSkip))
                    {
                        row[property.Name] = property.GetValue(item) ?? DBNull.Value;
                    }
                }

                table.Rows.Add(row);
            }

            return table;
        }
    }
}
