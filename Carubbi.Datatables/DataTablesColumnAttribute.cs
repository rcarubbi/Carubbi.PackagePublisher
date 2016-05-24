using System;
using System.ComponentModel.DataAnnotations;

namespace Carubbi.Datatables
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class DataTablesColumnAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        public int Order { get; set; }

        public string Header { get; set; }

        public string SortMap { get; set; }

        public bool PrimaryKey { get; set; }

        public bool Hidden { get; set; }

        public DataType DataType { get; set; }
    }
}