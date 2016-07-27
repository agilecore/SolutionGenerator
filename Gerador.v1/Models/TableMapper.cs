using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gerador.Models
{
    public sealed class TableMapper
    {
        public TableMapper() { }
        public string TableName { get; set; }
        public List<ColumnMapper> CollectionColumn { get; set; }
    }

    public sealed class ColumnMapper
    {
        public string ColumnName { get; set; }
        public string IsNullable { get; set; }
        public string DataType { get; set; }
        public string MaxLenght { get; set; }
        public string ColumnKey { get; set; }
    }
}
