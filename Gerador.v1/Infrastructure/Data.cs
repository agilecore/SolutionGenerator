using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gerador.Enums;
using System.Configuration;
using System.IO;
using Gerador.Models;

namespace Gerador.Infrastructure
{
    /// <summary>
    /// Esta classe gera a camada de dados inteira conforme as tabelas e relacionadas do banco de dados
    /// a camada de dados é baseada no pattern Generic Repository com UnitOfWork.
    /// </summary>
    public class Data
    {
        private StringBuilder TextClass;
        private String SufixoModels = "Dto";
        private String SufixoModelsExtension = "DtoSpecialized";
        private String SufixoMapper = "Mapper";
        private String FilePath = ConfigurationManager.AppSettings["CamadaDeAcessos"].ToString();
        private String FilePathCommon = ConfigurationManager.AppSettings["CamadaComum"].ToString();
        private String ProjectName = ConfigurationManager.AppSettings["NomeDoProjeto"].ToString();
        private String DatabaseSchema = ConfigurationManager.AppSettings["NomeDoSchema"].ToString();

        private EDataBase DatabaseType = Gerador.Program.DatabaseType;

        public String BuildBase()
        {
            TextClass = new StringBuilder();
            TextClass.AppendLine("using System;");
            TextClass.AppendLine("using System.Linq;");
            TextClass.AppendLine("using System.Text;");
            TextClass.AppendLine("using System.Collections.Generic;");
            TextClass.AppendLine("using System.Threading.Tasks;");
            TextClass.AppendLine("");
            TextClass.AppendLine("namespace " + ProjectName + ".Common");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    public class Base");
            TextClass.AppendLine("    {");
            TextClass.AppendLine("        public Int32 ID { get; set; }");
            TextClass.AppendLine("    }");
            TextClass.AppendLine("}");

            return CreateFileCommom("Base", "Models");
        }

        public String BuildModels(KeyValuePair<String, ModelConfig> TableSetting)
        {
            var TableName = TableSetting.Key;
            var DataModel = TableSetting.Value;
            var TableSchema = Utils.GetTableSchema(TableName, DatabaseType);
            var ColumnDataType = String.Empty;

            TextClass = new StringBuilder();
            TextClass.AppendLine("using System;");
            TextClass.AppendLine("using System.Linq;");
            TextClass.AppendLine("using System.Text;");
            TextClass.AppendLine("using System.Collections.Generic;");
            TextClass.AppendLine("using System.Threading.Tasks;");
            TextClass.AppendLine("");
            TextClass.AppendLine("namespace " + ProjectName + ".Common");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    /// <summary>");
            TextClass.AppendLine("    /// Nao alterar essa classe pois ela é o objeto identico a tabela do banco de dados.");
            TextClass.AppendLine("    /// </summary>");
            TextClass.AppendLine("    public class " + String.Concat(DataModel.ClassName, SufixoModels) + " : Base");
            TextClass.AppendLine("    {");

            foreach (var ColumnMapper in TableSchema.CollectionColumn)
            {
                if (ColumnMapper.ColumnKey != "pk")
                {
                    if (ColumnMapper.DataType == "datetime")
                    {
                        if (ColumnMapper.IsNullable == "yes")
                        {
                            ColumnDataType = Utils.GetColumnType(ColumnMapper.DataType);
                            TextClass.AppendLine("        public Nullable<System.DateTime> " + ColumnMapper.ColumnName + " { get; set; }");
                        }
                        else
                        {
                            ColumnDataType = Utils.GetColumnType(ColumnMapper.DataType);
                            TextClass.AppendLine("        public " + ColumnDataType + " " + ColumnMapper.ColumnName + " { get; set; }");
                        }
                    }
                    else if (ColumnMapper.DataType == "int")
                    {
                        if (ColumnMapper.IsNullable == "yes")
                        {
                            ColumnDataType = Utils.GetColumnType(ColumnMapper.DataType);
                            TextClass.AppendLine("        public Nullable<System.Int32> " + ColumnMapper.ColumnName + " { get; set; }");
                        }
                        else
                        {
                            ColumnDataType = Utils.GetColumnType(ColumnMapper.DataType);
                            TextClass.AppendLine("        public " + ColumnDataType + " " + ColumnMapper.ColumnName + " { get; set; }");
                        }
                    }
                    else if (ColumnMapper.DataType == "decimal")
                    {
                        if (ColumnMapper.IsNullable == "yes")
                        {
                            ColumnDataType = Utils.GetColumnType(ColumnMapper.DataType);
                            TextClass.AppendLine("        public Nullable<System.Decimal> " + ColumnMapper.ColumnName + " { get; set; }");
                        }
                        else
                        {
                            ColumnDataType = Utils.GetColumnType(ColumnMapper.DataType);
                            TextClass.AppendLine("        public " + ColumnDataType + " " + ColumnMapper.ColumnName + " { get; set; }");
                        }
                    }
                    else if (ColumnMapper.DataType == "long")
                    {
                        if (ColumnMapper.IsNullable == "yes")
                        {
                            ColumnDataType = Utils.GetColumnType(ColumnMapper.DataType);
                            TextClass.AppendLine("        public Nullable<System.long> " + ColumnMapper.ColumnName + " { get; set; }");
                        }
                        else
                        {
                            ColumnDataType = Utils.GetColumnType(ColumnMapper.DataType);
                            TextClass.AppendLine("        public " + ColumnDataType + " " + ColumnMapper.ColumnName + " { get; set; }");
                        }
                    }
                    else if (ColumnMapper.DataType == "longblob")
                    {
                        if (ColumnMapper.IsNullable == "yes")
                        {
                            ColumnDataType = Utils.GetColumnType(ColumnMapper.DataType);
                            TextClass.AppendLine("        public Nullable<System.Byte> " + ColumnMapper.ColumnName + " { get; set; }");
                        }
                        else
                        {
                            ColumnDataType = Utils.GetColumnType(ColumnMapper.DataType);
                            TextClass.AppendLine("        public " + ColumnDataType + " " + ColumnMapper.ColumnName + " { get; set; }");
                        }
                    }
                    else
                    {
                        ColumnDataType = Utils.GetColumnType(ColumnMapper.DataType);
                        TextClass.AppendLine("        public " + ColumnDataType + " " + ColumnMapper.ColumnName + " { get; set; }");
                    }
                }
            }

            TextClass.AppendLine("    }");
            TextClass.AppendLine("}");

            return CreateFileMadatory(String.Concat(DataModel.ClassName, SufixoModels), FilePathCommon + @"\Models");
        }

        public String BuildModelsExtension(KeyValuePair<String, ModelConfig> TableSetting)
        {
            var TableName = TableSetting.Key;
            var DataModel = TableSetting.Value;
            var TableSchema = Utils.GetTableSchema(TableName, DatabaseType);
            var ColumnDataType = String.Empty;

            TextClass = new StringBuilder();
            TextClass.AppendLine("using System;");
            TextClass.AppendLine("using System.Linq;");
            TextClass.AppendLine("using System.Text;");
            TextClass.AppendLine("using System.Collections.Generic;");
            TextClass.AppendLine("using System.Threading.Tasks;");
            TextClass.AppendLine("");
            TextClass.AppendLine("namespace " + ProjectName + ".Common");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    public class " + String.Concat(DataModel.ClassName, SufixoModelsExtension) + " : Base");
            TextClass.AppendLine("    {");
            TextClass.AppendLine("    }");
            TextClass.AppendLine("}");

            return CreateFileCommom(String.Concat(DataModel.ClassName, SufixoModelsExtension), @"Models\" + DataModel.ClassName);
        }

        public String BuildMapper(KeyValuePair<String, ModelConfig> TableSetting)
        {
            var TableName = TableSetting.Key;
            var DataModel = TableSetting.Value;
            var TableSchema = Utils.GetTableSchema(TableName, DatabaseType);

            TextClass = new StringBuilder();
            TextClass.AppendLine("using System.Data.Entity.ModelConfiguration;");
            TextClass.AppendLine("using " + ProjectName + ".Common;");
            TextClass.AppendLine("");
            TextClass.AppendLine("namespace " + ProjectName + ".Data");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    public class " + String.Concat(DataModel.ClassName, SufixoMapper) + " : EntityTypeConfiguration<" + String.Concat(DataModel.ClassName, SufixoModels) + ">");
            TextClass.AppendLine("    {");
            TextClass.AppendLine("        public " + String.Concat(DataModel.ClassName, SufixoMapper) + "()");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            // Primary Key");
            TextClass.AppendLine("            this.HasKey(t => t.ID);");
            TextClass.AppendLine("");
            TextClass.AppendLine("            // Propertys Required");

            foreach (var ColumnMapper in TableSchema.CollectionColumn)
            {
                if (ColumnMapper.ColumnKey != "pk")
                {
                    // Trata somente campos de textos
                    if ((ColumnMapper.DataType == "char") || (ColumnMapper.DataType == "varchar") || (ColumnMapper.DataType == "varchar2") || (ColumnMapper.DataType == "nvarchar2") || (ColumnMapper.DataType == "text") || (ColumnMapper.DataType == "longtext") || (ColumnMapper.DataType == "clob") || (ColumnMapper.DataType == "long") || (ColumnMapper.DataType == "nchar") || (ColumnMapper.DataType == "nclob") || (ColumnMapper.DataType == "rowid"))
                    {
                        if (ColumnMapper.IsNullable == "no")
                        {
                            if ((ColumnMapper.MaxLenght != String.Empty))
                            {
                                if (ColumnMapper.DataType == "longtext")
                                    TextClass.AppendLine("            this.Property(_ => _." + ColumnMapper.ColumnName + ").IsRequired();");
                                else
                                    TextClass.AppendLine("            this.Property(_ => _." + ColumnMapper.ColumnName + ").IsRequired().HasMaxLength(" + ColumnMapper.MaxLenght + ");");
                            }
                            else
                            {
                                TextClass.AppendLine("            this.Property(_ => _." + ColumnMapper.ColumnName + ").IsRequired();");
                            }
                        }
                        else if (ColumnMapper.IsNullable == "yes")
                        {
                            if (ColumnMapper.MaxLenght != String.Empty)
                            {
                                if (ColumnMapper.DataType == "longtext")
                                    TextClass.AppendLine("            this.Property(_ => _." + ColumnMapper.ColumnName + ").IsRequired();");
                                else
                                    TextClass.AppendLine("            this.Property(_ => _." + ColumnMapper.ColumnName + ").IsRequired().HasMaxLength(" + ColumnMapper.MaxLenght + ");");
                            }
                            else
                                TextClass.AppendLine("            this.Property(_ => _." + ColumnMapper.ColumnName + ");");
                        }
                    }
                    // Trata somente campos de Data
                    else if ((ColumnMapper.DataType == "datetime") || (ColumnMapper.DataType == "date") || (ColumnMapper.DataType == "timestamp"))
                    {
                        if (ColumnMapper.IsNullable == "no")
                            TextClass.AppendLine("            this.Property(_ => _." + ColumnMapper.ColumnName + ").IsRequired();");
                        else if (ColumnMapper.IsNullable == "yes")
                            TextClass.AppendLine("            this.Property(_ => _." + ColumnMapper.ColumnName + ");");
                    }
                    // Trata somente campos de numeros
                    else if ((ColumnMapper.DataType == "decimal") || (ColumnMapper.DataType == "float") || (ColumnMapper.DataType == "integer") || (ColumnMapper.DataType == "double") || (ColumnMapper.DataType == "smallint") || (ColumnMapper.DataType == "int") || (ColumnMapper.DataType == "number") || (ColumnMapper.DataType == "bigint"))
                    {
                        if (ColumnMapper.IsNullable == "no")
                            TextClass.AppendLine("            this.Property(_ => _." + ColumnMapper.ColumnName + ").IsRequired();");
                        else if (ColumnMapper.IsNullable == "yes")
                            TextClass.AppendLine("            this.Property(_ => _." + ColumnMapper.ColumnName + ");");
                    }
                    else
                    {
                        if (ColumnMapper.IsNullable == "no")
                        {
                            if (ColumnMapper.MaxLenght != String.Empty)
                                TextClass.AppendLine("            this.Property(_ => _." + ColumnMapper.ColumnName + ").IsRequired().HasMaxLength(" + ColumnMapper.MaxLenght + ");");
                            else
                                TextClass.AppendLine("            this.Property(_ => _." + ColumnMapper.ColumnName + ").IsRequired();");
                        }
                        else if (ColumnMapper.IsNullable == "yes")
                        {
                            if (ColumnMapper.MaxLenght != String.Empty)
                                TextClass.AppendLine("            this.Property(_ => _." + ColumnMapper.ColumnName + ").HasMaxLength(" + ColumnMapper.MaxLenght + ");");
                            else
                                TextClass.AppendLine("            this.Property(_ => _." + ColumnMapper.ColumnName + ");");
                        }
                    }
                }
            }

            TextClass.AppendLine("");
            TextClass.AppendLine("            // Table & Column Mappings");
            TextClass.AppendLine("            this.ToTable(\"" + TableName + "\", \"" + DatabaseSchema.ToLower() + "\");");
            TextClass.AppendLine("");
            TextClass.AppendLine("            // Propertys Relationship Database Table Columns");

            foreach (var ColumnMapper in TableSchema.CollectionColumn)
            {
                if (ColumnMapper.ColumnKey == "pk")
                    TextClass.AppendLine("            this.Property(_ => _.ID).HasColumnName(\"" + ColumnMapper.ColumnName + "\");");
                else
                    TextClass.AppendLine("            this.Property(_ => _." + ColumnMapper.ColumnName + ").HasColumnName(\"" + ColumnMapper.ColumnName + "\");");
            }

            TextClass.AppendLine("        }");
            TextClass.AppendLine("    }");
            TextClass.AppendLine("}");

            return CreateFile(String.Concat(DataModel.ClassName, SufixoMapper), @"Mapper\" + DataModel.ClassName);
        }

        public String BuildUnitOfWork(Dictionary<String, ModelConfig> GroupTables)
        {
            var OutputClassDataContextName = ConfigurationManager.AppSettings["NomeDoContext"].ToString();

            TextClass = new StringBuilder();

            TextClass.AppendLine("using System;");
            TextClass.AppendLine("using System.Collections.Generic;");
            TextClass.AppendLine("using System.Linq;");
            TextClass.AppendLine("using System.Text;");
            TextClass.AppendLine("using System.Threading.Tasks;");
            TextClass.AppendLine("using " + ProjectName + ".Data;");
            TextClass.AppendLine("");
            TextClass.AppendLine("namespace " + ProjectName + ".Data");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    public class UnitOfWork                                                                                ");
            TextClass.AppendLine("    {                                                                                                      ");
            TextClass.AppendLine("        private " + OutputClassDataContextName + " _dbContext = new " + OutputClassDataContextName + "();  ");
            TextClass.AppendLine("        public Type _type { get; set; }                                                                    ");
            TextClass.AppendLine("        public Repository<TEntityType> GetRepository<TEntityType>() where TEntityType : class              ");
            TextClass.AppendLine("        {                                                                                                  ");
            TextClass.AppendLine("            return (new Repository<TEntityType>(this._dbContext));                                         ");
            TextClass.AppendLine("        }                                                                                                  ");
            TextClass.AppendLine("        public void SaveChage()                                                                            ");
            TextClass.AppendLine("        {                                                                                                  ");
            TextClass.AppendLine("            _dbContext.SaveChanges();                                                                      ");
            TextClass.AppendLine("        }                                                                                                  ");
            TextClass.AppendLine("        public void SaveChageAsync()                                                                       ");
            TextClass.AppendLine("        {                                                                                                  ");
            TextClass.AppendLine("            _dbContext.SaveChangesAsync();                                                                 ");
            TextClass.AppendLine("        }                                                                                                  ");
            TextClass.AppendLine("    }                                                                                                      ");
            TextClass.AppendLine("}                                                                                                          ");

            var FileName = String.Format(@"{0}.{1}", "UnitOfWork", "cs");
            var FullFile = String.Format(@"{0}\{1}", FilePath, FileName);
            var DirInfo = new DirectoryInfo(FilePath);

            if (!DirInfo.Exists) { DirInfo.Create(); }
            using (TextWriter Writer = File.CreateText(FullFile)) { Writer.WriteLine(TextClass.ToString()); }
            return FileName;
        }

        public String BuildContext(Dictionary<String, ModelConfig> GroupTables)
        {
            var OutputClassDataContextName = ConfigurationManager.AppSettings["NomeDoContext"].ToString();

            TextClass = new StringBuilder();
            TextClass.AppendLine("using System.Data.Entity;                                                               ");
            TextClass.AppendLine("using " + ProjectName + ".Common;                                                       ");
            TextClass.AppendLine("                                                                                        ");
            TextClass.AppendLine("namespace " + ProjectName + ".Data                                                      ");
            TextClass.AppendLine("{                                                                                       ");
            TextClass.AppendLine("    public class " + OutputClassDataContextName + " : DbContext                         ");
            TextClass.AppendLine("    {                                                                                   ");

            foreach (var item in GroupTables)
            {
                var Model = item.Value;
                TextClass.AppendLine("             public DbSet<" + String.Concat(Model.ClassName, SufixoModels) + "> " + Model.ClassName + " { get; set; }");
            }

            TextClass.AppendLine("                                                                                         ");
            TextClass.AppendLine("        static " + OutputClassDataContextName + "()                                      ");
            TextClass.AppendLine("        {                                                                                ");
            TextClass.AppendLine("             Database.SetInitializer<" + OutputClassDataContextName + ">(null);          ");
            TextClass.AppendLine("        }                                                                                ");
            TextClass.AppendLine("                                                                                         ");
            TextClass.AppendLine("        public " + OutputClassDataContextName + "() : base(\"Name = DefaultConnection\") ");
            TextClass.AppendLine("        {                                                                                ");
            TextClass.AppendLine("             this.Configuration.AutoDetectChangesEnabled = true;                         ");
            TextClass.AppendLine("             this.Configuration.ValidateOnSaveEnabled = false;                           ");
            TextClass.AppendLine("             this.Configuration.LazyLoadingEnabled = false;                              ");
            TextClass.AppendLine("             this.Configuration.ProxyCreationEnabled = false;                            ");
            TextClass.AppendLine("             this.Configuration.UseDatabaseNullSemantics = true;                         ");
            TextClass.AppendLine("        }                                                                                ");
            TextClass.AppendLine("                                                                                         ");
            TextClass.AppendLine("        protected override void OnModelCreating(DbModelBuilder ModelBuilder)             ");
            TextClass.AppendLine("        {                                                                                ");

            foreach (var item in GroupTables)
            {
                var Model = item.Value;
                TextClass.AppendLine("             ModelBuilder.Configurations.Add(new " + Model.ClassName + "Mapper());   ");
            }

            TextClass.AppendLine("        }                                                                                ");
            TextClass.AppendLine("    }                                                                                    ");
            TextClass.AppendLine("}                                                                                        ");

            return CreateFile("DbContext", "AppContext");

        }

        public String BuildRespository()
        {
            var OutputClassDataContextName = ConfigurationManager.AppSettings["NomeDoContext"].ToString();

            TextClass = new StringBuilder();
            TextClass.AppendLine("using System;																															");
            TextClass.AppendLine("using System.Collections.Generic;                                                                                                     ");
            TextClass.AppendLine("using System.Linq;                                                                                                                    ");
            TextClass.AppendLine("using System.Linq.Expressions;                                                                                                        ");
            TextClass.AppendLine("using System.Text;                                                                                                                    ");
            TextClass.AppendLine("using System.Threading.Tasks;                                                                                                         ");
            TextClass.AppendLine("using System.Data;                                                                                                                    ");
            TextClass.AppendLine("using System.Data.Entity;                                                                                                             ");
            TextClass.AppendLine("using " + ProjectName + ".Data.Interface;                                                                                                           ");
            TextClass.AppendLine("                                                                                                                                      ");
            TextClass.AppendLine("namespace " + ProjectName + ".Data                                                                                                                  ");
            TextClass.AppendLine("{                                                                                                                                     ");
            TextClass.AppendLine("    public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class                                        ");
            TextClass.AppendLine("    {                                                                                                                                 ");
            TextClass.AppendLine("        private DbSet<TEntity> _dbSet;                                                                                                ");
            TextClass.AppendLine("                                                                                                                                      ");
            TextClass.AppendLine("        private " + ProjectName + "Context _dbContext;                                                                                                 ");
            TextClass.AppendLine("                                                                                                                                      ");
            TextClass.AppendLine("        internal Boolean _preConfig = false;                                                                                          ");
            TextClass.AppendLine("                                                                                                                                      ");
            TextClass.AppendLine("        public Repository(" + ProjectName + "Context dbContext)                                                                                        ");
            TextClass.AppendLine("        {                                                                                                                             ");
            TextClass.AppendLine("            InitializePreConfiguration(dbContext);                                                                                    ");
            TextClass.AppendLine("        }                                                                                                                             ");
            TextClass.AppendLine("                                                                                                                                      ");
            TextClass.AppendLine("        public IQueryable<TEntity> GetAll()                                                                                           ");
            TextClass.AppendLine("        {                                                                                                                             ");
            TextClass.AppendLine("            return _dbContext.Set<TEntity>();                                                                                         ");
            TextClass.AppendLine("        }                                                                                                                             ");
            TextClass.AppendLine("                                                                                                                                      ");
            TextClass.AppendLine("        public IQueryable<TEntity> GetByFilters(Expression<Func<TEntity, bool>> predicate)                                            ");
            TextClass.AppendLine("        {                                                                                                                             ");
            TextClass.AppendLine("            return _dbContext.Set<TEntity>().Where(predicate);                                                                        ");
            TextClass.AppendLine("        }                                                                                                                             ");
            TextClass.AppendLine("                                                                                                                                      ");
            TextClass.AppendLine("        public TEntity Find(params object[] key)                                                                                      ");
            TextClass.AppendLine("        {                                                                                                                             ");
            TextClass.AppendLine("            return _dbContext.Set<TEntity>().Find(key);                                                                               ");
            TextClass.AppendLine("        }                                                                                                                             ");
            TextClass.AppendLine("                                                                                                                                      ");
            TextClass.AppendLine("        public TEntity First(Expression<Func<TEntity, bool>> predicate)                                                               ");
            TextClass.AppendLine("        {                                                                                                                             ");
            TextClass.AppendLine("            return _dbContext.Set<TEntity>().Where(predicate).FirstOrDefault();                                                       ");
            TextClass.AppendLine("        }                                                                                                                             ");
            TextClass.AppendLine("                                                                                                                                      ");
            TextClass.AppendLine("        public void Add(TEntity entity)                                                                                               ");
            TextClass.AppendLine("        {                                                                                                                             ");
            TextClass.AppendLine("            _dbContext.Set<TEntity>().Add(entity);                                                                                    ");
            TextClass.AppendLine("        }                                                                                                                             ");
            TextClass.AppendLine("                                                                                                                                      ");
            TextClass.AppendLine("        public void AddAll(List<TEntity> collection)                                                                                  ");
            TextClass.AppendLine("        {                                                                                                                             ");
            TextClass.AppendLine("            foreach (var item in collection)                                                                                          ");
            TextClass.AppendLine("            {                                                                                                                         ");
            TextClass.AppendLine("                _dbContext.Set<TEntity>().Add(item);                                                                                  ");
            TextClass.AppendLine("            }                                                                                                                         ");
            TextClass.AppendLine("        }                                                                                                                             ");
            TextClass.AppendLine("                                                                                                                                      ");
            TextClass.AppendLine("        public void Update(TEntity entity)                                                                                            ");
            TextClass.AppendLine("        {                                                                                                                             ");
            TextClass.AppendLine("            _dbContext.Entry(entity).State = EntityState.Modified;                                                                    ");
            TextClass.AppendLine("        }                                                                                                                             ");
            TextClass.AppendLine("                                                                                                                                      ");
            TextClass.AppendLine("        public void Delete(Expression<Func<TEntity, bool>> predicate)                                                                 ");
            TextClass.AppendLine("        {                                                                                                                             ");
            TextClass.AppendLine("            _dbContext.Set<TEntity>()                                                                                                 ");
            TextClass.AppendLine("           .Where(predicate).ToList()                                                                                                 ");
            TextClass.AppendLine("           .ForEach(del => _dbContext.Set<TEntity>().Remove(del));                                                                    ");
            TextClass.AppendLine("        }                                                                                                                             ");
            TextClass.AppendLine("                                                                                                                                      ");
            TextClass.AppendLine("        /// <summary>                                                                                                                 ");
            TextClass.AppendLine("        /// Método inicial principal que é iniciado mediante as configuracoes, isso deixa o entity framework mais rapido ou mais lento");
            TextClass.AppendLine("        /// Se \"_preConfig\" false nao entra aqui e continua com as configuracoes padrao do entity framework.                        ");
            TextClass.AppendLine("        /// </summary>                                                                                                                ");
            TextClass.AppendLine("        internal void InitializePreConfiguration(" + ProjectName + "Context dbContext)                                                       ");
            TextClass.AppendLine("        {                                                                                                                             ");
            TextClass.AppendLine("            this._dbSet = dbContext.Set<TEntity>();                                                                                   ");
            TextClass.AppendLine("            this._dbContext = dbContext;                                                                                              ");
            TextClass.AppendLine("                                                                                                                                      ");
            TextClass.AppendLine("            if (_preConfig)                                                                                                           ");
            TextClass.AppendLine("            {                                                                                                                         ");
            TextClass.AppendLine("                this._dbContext.Configuration.AutoDetectChangesEnabled = false;  // Atencão ao modificar!                             ");
            TextClass.AppendLine("                this._dbContext.Configuration.LazyLoadingEnabled = true;         // Atencão ao modificar!                             ");
            TextClass.AppendLine("                this._dbContext.Configuration.ProxyCreationEnabled = false;      // Atencão ao modificar!                             ");
            TextClass.AppendLine("                this._dbContext.Configuration.ValidateOnSaveEnabled = true;      // Atencão ao modificar!                             ");
            TextClass.AppendLine("                this._dbContext.Configuration.UseDatabaseNullSemantics = true;   // Atencão ao modificar!                             ");
            TextClass.AppendLine("            }                                                                                                                         ");
            TextClass.AppendLine("        }                                                                                                                             ");
            TextClass.AppendLine("                                                                                                                                      ");
            TextClass.AppendLine("        public void Dispose()                                                                                                         ");
            TextClass.AppendLine("        {                                                                                                                             ");
            TextClass.AppendLine("            if (_dbContext != null)                                                                                                   ");
            TextClass.AppendLine("            {                                                                                                                         ");
            TextClass.AppendLine("                _dbContext.Dispose();                                                                                                 ");
            TextClass.AppendLine("            }                                                                                                                         ");
            TextClass.AppendLine("            GC.SuppressFinalize(this);                                                                                                ");
            TextClass.AppendLine("        }                                                                                                                             ");
            TextClass.AppendLine("    }                                                                                                                                 ");
            TextClass.AppendLine("}                                                                                                                                     ");

            return CreateFileMadatory("Repository", FilePath + @"\Repository");
        }

        public String BuildIRespository()
        {
            TextClass = new StringBuilder();

            TextClass.AppendLine("using System;                                                                       ");            
            TextClass.AppendLine("using System.Collections.Generic;                                                   ");
            TextClass.AppendLine("using System.Linq;                                                                  ");
            TextClass.AppendLine("using System.Linq.Expressions;                                                      ");
            TextClass.AppendLine("using System.Text;                                                                  ");
            TextClass.AppendLine("using System.Threading.Tasks;                                                       ");
            TextClass.AppendLine("                                                                                    ");
            TextClass.AppendLine("namespace " + ProjectName + ".Data.Interface                                        ");
            TextClass.AppendLine("{                                                                                   ");
            TextClass.AppendLine("    public interface IRepository<TEntity> where TEntity : class                     ");
            TextClass.AppendLine("    {                                                                               ");
            TextClass.AppendLine("        IQueryable<TEntity> GetAll();                                               ");
            TextClass.AppendLine("        IQueryable<TEntity> GetByFilters(Expression<Func<TEntity, bool>> predicate);");
            TextClass.AppendLine("        TEntity Find(params object[] key);                                          ");
            TextClass.AppendLine("        TEntity First(Expression<Func<TEntity, bool>> predicate);                   ");
            TextClass.AppendLine("        void Add(TEntity entity);                                                   ");
            TextClass.AppendLine("        void Delete(Expression<Func<TEntity, bool>> predicate);                     ");
            TextClass.AppendLine("        void Dispose();                                                             ");
            TextClass.AppendLine("    }                                                                               ");
            TextClass.AppendLine("}                                                                                   ");

            return CreateFileMadatory("IRepository", FilePath + @"\Interface");

        }

        private String CreateFile(String ClassName, String Folder)
        {
            var FileName = String.Format(@"{0}.{1}", ClassName, "cs");
            var Diretory = String.Format(@"{0}\{1}", FilePath, Folder);
            var FullFile = String.Format(@"{0}\{1}", Diretory, FileName);
            var DirInfo = new DirectoryInfo(Diretory);

            if (!DirInfo.Exists) { DirInfo.Create(); }
            using (TextWriter Writer = File.CreateText(FullFile)) { Writer.WriteLine(TextClass.ToString()); }
            return FileName;
        }

        private String CreateFileCommom(String ClassName, String Folder)
        {
            var FileName = String.Format(@"{0}.{1}", ClassName, "cs");
            var Diretory = String.Format(@"{0}\{1}", FilePathCommon, Folder);
            var FullFile = String.Format(@"{0}\{1}", Diretory, FileName);
            var DirInfo = new DirectoryInfo(Diretory);

            if (!DirInfo.Exists)
            {
                DirInfo.Create();
                using (TextWriter Writer = File.CreateText(FullFile)) { Writer.WriteLine(TextClass.ToString()); }
            }
            else
            {
                if (!File.Exists(FullFile))
                {
                    using (TextWriter Writer = File.CreateText(FullFile)) { Writer.WriteLine(TextClass.ToString()); }
                }
                else
                {
                    FileName = FileName + " não sera sobre escrito...";
                }
            }
            
            return FileName;
        }

        private String CreateFileMadatory(String ClassName, String Folder)
        {
            var FullFile = String.Format(@"{0}\{1}.{2}", Folder, ClassName, "cs");

            if (File.Exists(FullFile))
            {
                //File.Delete(FullFile);
                using (TextWriter Writer = File.CreateText(FullFile)) { Writer.WriteLine(TextClass.ToString()); }
            }
            else
            {

                Directory.CreateDirectory(Folder);
                using (TextWriter Writer = File.CreateText(FullFile)) { Writer.WriteLine(TextClass.ToString()); }

            }

            return String.Format("{0}.{1}", ClassName, "cs"); ;
        }

    }
}