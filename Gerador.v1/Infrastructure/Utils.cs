using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gerador.Models;
using Gerador.Connections;
using Gerador.Enums;

namespace Gerador.Infrastructure
{
	public class Utils
	{
		public static TableMapper GetTableSchema(String TableName, EDataBase DataBaseType)
		{
			try
			{
				var Retorno = new TableMapper();
				var Query = String.Empty;

				switch (DataBaseType)
				{
					case EDataBase.MySql:
						//Retorno = LoadMySql(TableName);
						break;
					case EDataBase.SqlServer:
						Retorno = LoadSQLServer(TableName);
						break;
					case EDataBase.Oracle:
						//Retorno = LoadOracle(TableName);
						break;
				}

				return (Retorno);
			}
			catch (Exception Ex)
			{
				throw Ex;
			}
		}

		public static String GetColumnType(String ColumnDataType)
		{
			var DataType = String.Empty;

			if ((ColumnDataType == "boolean") || (ColumnDataType == "bit"))
				DataType = "System.Boolean";

			if ((ColumnDataType == "tinyint") || (ColumnDataType == "unsigned"))
				DataType = "System.Byte";

			if ((ColumnDataType == "binary") || (ColumnDataType == "varbinary") || (ColumnDataType == "blob") || (ColumnDataType == "bfile") || (ColumnDataType == "longblob") || (ColumnDataType == "raw"))
				DataType = "System.Byte[]";

			if ((ColumnDataType == "datetime") || (ColumnDataType == "date") || (ColumnDataType == "timestamp"))
				DataType = "System.DateTime";

			if ((ColumnDataType == "decimal") || (ColumnDataType == "float") || (ColumnDataType == "integer") || (ColumnDataType == "money"))
				DataType = "System.Decimal";


			if ((ColumnDataType == "double"))
				DataType = "System.Double";

			if ((ColumnDataType == "char"))
				DataType = "System.Guid";

			if ((ColumnDataType == "smallint"))
				DataType = "System.Int16";

			if ((ColumnDataType == "int") || (ColumnDataType == "number") || (ColumnDataType == "numeric"))
				DataType = "System.Int32";

			if ((ColumnDataType == "bigint"))
				DataType = "System.Int64";

			if ((ColumnDataType == "tinyint"))
				DataType = "System.SByte";

			if ((ColumnDataType == "float"))
				DataType = "System.Single";

			if ((ColumnDataType == "char") || (ColumnDataType == "varchar") || (ColumnDataType == "nvarchar") || (ColumnDataType == "varchar2") || (ColumnDataType == "nvarchar2") || (ColumnDataType == "text") || (ColumnDataType == "longtext") || (ColumnDataType == "clob") || (ColumnDataType == "long") || (ColumnDataType == "nchar") || (ColumnDataType == "nclob") || (ColumnDataType == "rowid"))
				DataType = "System.String";

			if ((ColumnDataType == "time"))
				DataType = "System.TimeSpan";

			return (DataType);
		}

		private static TableMapper LoadSQLServer(String TableName)
		{
			var TableMapper = new TableMapper();

			var Query = "SELECT " +
								"UPPER(A.COLUMN_NAME) COLUMN_NAME, " +
								"LOWER(A.IS_NULLABLE) IS_NULLABLE, " +
								"LOWER(A.DATA_TYPE) DATA_TYPE, " +
								"LOWER(A.CHARACTER_MAXIMUM_LENGTH) MAXIMUM_LENGTH, " +
								"B.COLUMN_KEY " +
						"FROM " +
								"INFORMATION_SCHEMA.COLUMNS A LEFT JOIN " +
								"(SELECT UPPER(KCU.TABLE_NAME) AS TABLE_NAME, " +
								"      UPPER(KCU.COLUMN_NAME) AS COLUMN_NAME, " +
								"      CASE WHEN LOWER(TC.CONSTRAINT_TYPE) = 'PRIMARY KEY' THEN 'pk' " +
								"           WHEN LOWER(TC.CONSTRAINT_TYPE) = 'FOREIGN KEY' THEN 'fk' " +
								"           WHEN LOWER(TC.CONSTRAINT_TYPE) = 'UNIQUE' THEN NULL " +
								"           WHEN LOWER(TC.CONSTRAINT_TYPE) = NULL THEN NULL " +
								"      END AS COLUMN_KEY " +
								" FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU " +
								"      LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC " +
								"      ON KCU.TABLE_NAME = TC.TABLE_NAME " +
								"      AND KCU.TABLE_SCHEMA = TC.TABLE_SCHEMA " +
								"      AND KCU.TABLE_CATALOG = TC.TABLE_CATALOG " +
								"      AND KCU.CONSTRAINT_CATALOG = TC.CONSTRAINT_CATALOG " +
								"      AND KCU.CONSTRAINT_NAME = TC.CONSTRAINT_NAME " +
								"      LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC " +
								"      ON RC.CONSTRAINT_SCHEMA = TC.CONSTRAINT_SCHEMA " +
								"      AND RC.CONSTRAINT_CATALOG = TC.CONSTRAINT_CATALOG " +
								"      AND RC.CONSTRAINT_NAME = TC.CONSTRAINT_NAME " +
								"      LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CCU " +
								"      ON RC.UNIQUE_CONSTRAINT_SCHEMA = CCU.CONSTRAINT_SCHEMA " +
								"      AND RC.UNIQUE_CONSTRAINT_CATALOG = CCU.CONSTRAINT_CATALOG " +
								"      AND RC.UNIQUE_CONSTRAINT_NAME = CCU.CONSTRAINT_NAME) B " +
								"      ON A.TABLE_NAME = B.TABLE_NAME AND A.COLUMN_NAME = B.COLUMN_NAME " +
						"WHERE " +
								"A.TABLE_NAME = '" + TableName.ToLower() + "'";

			using (var Conexao = new ConnectionSqlServer(Query))
			{
				if (Conexao.Open())
				{
					TableMapper.TableName = TableName;
					var CollectionColumnMapper = new List<ColumnMapper>();

					while (Conexao.LerRegistro())
					{
						var Column = new ColumnMapper();

						Column.ColumnName = Conexao.Ler("COLUMN_NAME").ToString();
						Column.IsNullable = Conexao.Ler("IS_NULLABLE").ToString();
						Column.DataType = Conexao.Ler("DATA_TYPE").ToString();
						Column.MaxLenght = Conexao.Ler("MAXIMUM_LENGTH").ToString();
						Column.ColumnKey = Conexao.Ler("COLUMN_KEY").ToString();

						CollectionColumnMapper.Add(Column);
					}

					TableMapper.CollectionColumn = CollectionColumnMapper;
				}
			}
			return (TableMapper);
		}


		//private static TableMapper LoadOracle(String TableName)
		//{
		//    var TableMap = new TableMapper();
		//    var Query = String.Format(@"SELECT a.column_name column_name, 
		//                                       CASE a.nullable         
		//                                            WHEN 'Y' then 'yes' 
		//                                            WHEN 'N' then 'no' 
		//                                            ELSE ''
		//                                       END is_nullable,       
		//                                       LOWER(a.data_type) data_type,        
		//                                       a.data_length maximum_length,
		//                                       CASE b.constraint_type 
		//                                            WHEN 'P' THEN 'pk' 
		//                                            ELSE ''
		//                                       END column_key  
		//                                  FROM all_tab_columns a,       
		//                                       (SELECT w.column_name, 
		//                                               g.constraint_type,
		//                                               w.table_name
		//                                          FROM all_cons_columns w, all_constraints g  
		//                                         WHERE w.constraint_name = g.constraint_name(+) 
		//                                           AND g.constraint_type = 'P' ORDER BY 1) b  
		//                                 WHERE a.table_name  = b.table_name(+) 
		//                                   AND a.column_name = b.column_name(+) 
		//                                   AND a.table_name  = '{0}'", TableName);
		//
		//    using (var Conexao = new ConnectionOracle(Query, string.Empty))
		//    {
		//        if (Conexao.Open())
		//        {
		//            var Collection = new List<ColumnMapper>();
		//            TableMap.TableName = TableName;
		//
		//            while (Conexao.LerRegistro())
		//            {
		//                var Column = new ColumnMapper();
		//
		//                Column.ColumnName = Conexao.Ler("COLUMN_NAME").ToString();
		//                Column.IsNullable = Conexao.Ler("IS_NULLABLE").ToString();
		//                Column.DataType = Conexao.Ler("DATA_TYPE").ToString();
		//                Column.MaxLenght = Conexao.Ler("MAXIMUM_LENGTH").ToString();
		//                Column.ColumnKey = Conexao.Ler("COLUMN_KEY").ToString();
		//
		//                Collection.Add(Column);
		//            }
		//
		//            TableMap.CollectionColumn = Collection;
		//        }
		//    }
		//    return (TableMap);
		//}

		//private static TableMapper LoadMySql(String TableName)
		//{
		//    var TableMapper = new TableMapper();
		//
		//    var Query = string.Format(@"SELECT UPPER(COLUMN_NAME) COLUMN_NAME,
		//                                       LOWER(IS_NULLABLE) IS_NULLABLE,
		//                                       LOWER(DATA_TYPE)   DATA_TYPE,
		//                                       LOWER(CHARACTER_MAXIMUM_LENGTH) MAXIMUM_LENGTH, 
		//                                       CASE WHEN LOWER(COLUMN_KEY) = 'pri' THEN 'pk' 
		//                                            WHEN LOWER(COLUMN_KEY) = 'mul' THEN 'fk'    
		//		                                    WHEN LOWER(COLUMN_KEY) = NULL THEN NULL 
		//                                       END AS COLUMN_KEY 
		//                                  FROM INFORMATION_SCHEMA.COLUMNS A
		//                                 WHERE A.TABLE_NAME = '{0}' 
		//                                 ORDER BY A.ORDINAL_POSITION ASC", TableName);
		//
		//    using (var Conexao = new ConnectionMySql(Query))
		//    {
		//        if (Conexao.Open())
		//        {
		//            TableMapper.TableName = TableName;
		//            var CollectionColumnMapper = new List<ColumnMapper>();
		//
		//            while (Conexao.LerRegistro())
		//            {
		//                var Column = new ColumnMapper();
		//
		//                Column.ColumnName = Conexao.Ler("COLUMN_NAME").ToString();
		//                Column.IsNullable = Conexao.Ler("IS_NULLABLE").ToString();
		//                Column.DataType = Conexao.Ler("DATA_TYPE").ToString();
		//                Column.MaxLenght = Conexao.Ler("MAXIMUM_LENGTH").ToString();
		//                Column.ColumnKey = Conexao.Ler("COLUMN_KEY").ToString();
		//
		//                CollectionColumnMapper.Add(Column);
		//            }
		//
		//            TableMapper.CollectionColumn = CollectionColumnMapper;
		//        }
		//    }
		//    return (TableMapper);
		//}
	}
}
