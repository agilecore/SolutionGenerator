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
        private String SufixoMapper = "Mapper";
        private String FilePath = ConfigurationManager.AppSettings["CamadaDeAcessos"].ToString();
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
            TextClass.AppendLine("namespace " + ProjectName + ".Data.Model");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    public class Base");
            TextClass.AppendLine("    {");
            TextClass.AppendLine("        public Int32 ID { get; set; }");
            TextClass.AppendLine("    }");
            TextClass.AppendLine("}");

            return CreateFile("Base", "Models"); 
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
            TextClass.AppendLine("namespace " + ProjectName + ".Data.Model");
            TextClass.AppendLine("{");
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

            return CreateFile(String.Concat(DataModel.ClassName, SufixoModels), @"Models\" + DataModel.ClassName); 
        }

        public String BuildMapper(KeyValuePair<String, ModelConfig> TableSetting)
        {
            var TableName = TableSetting.Key;
            var DataModel = TableSetting.Value;
            var TableSchema = Utils.GetTableSchema(TableName, DatabaseType);

            TextClass = new StringBuilder();
            TextClass.AppendLine("using System;");
            TextClass.AppendLine("using System.Collections.Generic;");
            TextClass.AppendLine("using System.Data.Entity.ModelConfiguration;");
            TextClass.AppendLine("using System.Linq;");
            TextClass.AppendLine("using System.Text;");
            TextClass.AppendLine("using System.Threading.Tasks;");
            TextClass.AppendLine("");
            TextClass.AppendLine("namespace " + ProjectName + ".Data.Model");
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

            return CreateFile(String.Concat(DataModel.ClassName, SufixoMapper), @"Models\" + DataModel.ClassName);
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
            TextClass.AppendLine("using " + ProjectName + ".Data.Model;");
            TextClass.AppendLine("using " + ProjectName + ".Data.Repository;");
            TextClass.AppendLine("using " + ProjectName + ".Data.ApplicationContext;");
            TextClass.AppendLine("");
            TextClass.AppendLine("namespace " + ProjectName + ".Data");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    public class UnitOfWork : IDisposable");
            TextClass.AppendLine("    {");
            TextClass.AppendLine("        private " + OutputClassDataContextName + " _Context = new " + OutputClassDataContextName + "();");
            TextClass.AppendLine("        private bool _Disposed = false;");
            TextClass.AppendLine("");

            foreach (var item in GroupTables)
            {
                var Model = item.Value;

                TextClass.AppendLine("        private RepositoryGeneric<" + String.Concat(Model.ClassName, SufixoModels) + "> _" + Model.ClassName.ToLower() + "Repository;");
            }

            TextClass.AppendLine("");

            foreach (var item in GroupTables)
            {
                var Model = item.Value;

                TextClass.AppendLine("        public RepositoryGeneric<" + String.Concat(Model.ClassName, SufixoModels) + "> " + Model.ClassName + "Repository");
                TextClass.AppendLine("        {");
                TextClass.AppendLine("            get");
                TextClass.AppendLine("            {");
                TextClass.AppendLine("                if (this._" + Model.ClassName.ToLower() + "Repository == null)");
                TextClass.AppendLine("                {");
                TextClass.AppendLine("                    this._" + Model.ClassName.ToLower() + "Repository = new RepositoryGeneric<" + String.Concat(Model.ClassName, SufixoModels) + ">(_Context);");
                TextClass.AppendLine("                }");
                TextClass.AppendLine("                return _" + Model.ClassName.ToLower() + "Repository;");
                TextClass.AppendLine("            }");
                TextClass.AppendLine("        }");
                TextClass.AppendLine("");
            }

            TextClass.AppendLine("        public void Dispose()");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            Clear(true);");
            TextClass.AppendLine("            GC.SuppressFinalize(this);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        private void Clear(bool disposing)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            if (!this._Disposed)");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                if (disposing)");
            TextClass.AppendLine("                {");
            TextClass.AppendLine("                    _Context.Dispose();");

            foreach (var item in GroupTables)
            {
                var Model = item.Value;
                TextClass.AppendLine("                    _" + Model.ClassName.ToLower() + "Repository = null;");
            }

            TextClass.AppendLine("                }");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("            _Disposed = true;");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        ~UnitOfWork()");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            Clear(false);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("    }");
            TextClass.AppendLine("}");

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
            TextClass.AppendLine("using System;");
            TextClass.AppendLine("using System.Text;");
            TextClass.AppendLine("using System.Collections.Generic;");
            TextClass.AppendLine("using System.Data.Entity;");
            TextClass.AppendLine("using System.Data.Entity.Infrastructure;");
            TextClass.AppendLine("using System.Data.Entity.ModelConfiguration.Conventions;");
            TextClass.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            TextClass.AppendLine("using System.Linq;");
            TextClass.AppendLine("using System.Configuration;");
            TextClass.AppendLine("using System.Threading.Tasks;");
            TextClass.AppendLine("using " + ProjectName + ".Data.Model;");
            TextClass.AppendLine("");
            TextClass.AppendLine("namespace " + ProjectName + ".Data.ApplicationContext");
            TextClass.AppendLine("");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    public class " + OutputClassDataContextName + " : DbContext");
            TextClass.AppendLine("    {");

            foreach (var item in GroupTables)
            {
                var Model = item.Value;
                TextClass.AppendLine("        public DbSet<" + String.Concat(Model.ClassName, SufixoModels) + "> " + Model.ClassName + " { get; set; }");
            }

            TextClass.AppendLine("");
            TextClass.AppendLine("        static " + OutputClassDataContextName + "()");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            Database.SetInitializer<" + OutputClassDataContextName + ">(null);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        public " + OutputClassDataContextName + "() : base(\"Name=DefaultConnection\")");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("             this.Configuration.AutoDetectChangesEnabled = true;");
            TextClass.AppendLine("             this.Configuration.ValidateOnSaveEnabled = false;");
            TextClass.AppendLine("             this.Configuration.LazyLoadingEnabled = false;");
            TextClass.AppendLine("             this.Configuration.ProxyCreationEnabled = false;");
            TextClass.AppendLine("             //this.Configuration.EnsureTransactionsForFunctionsAndCommands = false;");
            TextClass.AppendLine("             this.Configuration.ProxyCreationEnabled = false;");
            TextClass.AppendLine("             this.Configuration.UseDatabaseNullSemantics = true;");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        protected override void OnModelCreating(DbModelBuilder ModelBuilder)");
            TextClass.AppendLine("        {");

            foreach (var item in GroupTables)
            {
                var Model = item.Value;
                TextClass.AppendLine("             ModelBuilder.Configurations.Add(new " + Model.ClassName + "Mapper());");
            }

            TextClass.AppendLine("        }");
            TextClass.AppendLine("    }");
            TextClass.AppendLine("}");

            return CreateFile("DbContext", "AppContext"); 
        }

        public String BuildRespository()
        {
            var OutputClassDataContextName = ConfigurationManager.AppSettings["NomeDoContext"].ToString();

            TextClass = new StringBuilder();
            TextClass.AppendLine("using System;");
            TextClass.AppendLine("using System.Collections.Generic;");
            TextClass.AppendLine("using System.Linq;");
            TextClass.AppendLine("using System.Text;");
            TextClass.AppendLine("using System.Threading.Tasks;");     
            TextClass.AppendLine("using System.Data.Entity;");
            TextClass.AppendLine("using System.Linq.Expressions;");
            TextClass.AppendLine("using System.Configuration;");
            TextClass.AppendLine("using " + ProjectName + ".Data.Repository;");
            TextClass.AppendLine("using " + ProjectName + ".Data.ApplicationContext;");
            TextClass.AppendLine("");
            TextClass.AppendLine("namespace " + ProjectName + ".Data.Repository");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    public class RepositoryGeneric<TEntity> : IDisposable, IRepository<TEntity> where TEntity : class");
            TextClass.AppendLine("    {");
            TextClass.AppendLine("        private DbSet<TEntity> _DbSet;");
            TextClass.AppendLine("        private " + OutputClassDataContextName + " _Context;");
            TextClass.AppendLine("");
            TextClass.AppendLine("        public RepositoryGeneric(" + OutputClassDataContextName + " Context)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            this._Context = Context;");
            TextClass.AppendLine("            this._DbSet = Context.Set<TEntity>();");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        public void Add(TEntity Model)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            try");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                _DbSet.Add(Model);");
            TextClass.AppendLine("                _Context.SaveChanges();");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("            catch (System.Data.Entity.Validation.DbEntityValidationException Ex)");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                CreateFileLog(Ex.InnerException.Message);");
            TextClass.AppendLine("                throw ExceptionValidationError(Ex);");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        public TEntity AddGetItem(TEntity Model)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            try");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                _DbSet.Add(Model);");
            TextClass.AppendLine("                _Context.SaveChanges();");
            TextClass.AppendLine("                return (Model);");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("            catch (System.Data.Entity.Validation.DbEntityValidationException Ex)");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                CreateFileLog(Ex.InnerException.Message);");
            TextClass.AppendLine("                throw ExceptionValidationError(Ex);");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        public void AddAll(List<TEntity> Collection)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            foreach (var model in Collection) { this.Add(model); }");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        public void Edit(TEntity Model)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            //_DbSet.Attach(Model);");
            TextClass.AppendLine("            //_Context.SaveChanges();");
            TextClass.AppendLine("            // Or -->...");
            TextClass.AppendLine("            try");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                var entry = _Context.Entry<TEntity>(Model);");
            TextClass.AppendLine("                var pkey = _DbSet.Create().GetType().GetProperty(\"ID\").GetValue(Model);");
            TextClass.AppendLine("");
            TextClass.AppendLine("                if (entry.State == EntityState.Detached)");
            TextClass.AppendLine("                {");
            TextClass.AppendLine("                    var set = _Context.Set<TEntity>();");
            TextClass.AppendLine("                    TEntity AttachedEntity = set.Find(pkey);");
            TextClass.AppendLine("");
            TextClass.AppendLine("                    if (AttachedEntity != null)");
            TextClass.AppendLine("                    {");
            TextClass.AppendLine("                        var AttachedEntry = _Context.Entry(AttachedEntity);");
            TextClass.AppendLine("                        AttachedEntry.CurrentValues.SetValues(Model);");
            TextClass.AppendLine("                    }");
            TextClass.AppendLine("                    else");
            TextClass.AppendLine("                    {");
            TextClass.AppendLine("                        entry.State = EntityState.Modified;");
            TextClass.AppendLine("                    }");
            TextClass.AppendLine("                }");
            TextClass.AppendLine("");
            TextClass.AppendLine("                _Context.SaveChanges();");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("            catch (System.Data.Entity.Validation.DbEntityValidationException Ex)");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                CreateFileLog(Ex.InnerException.Message);");
            TextClass.AppendLine("                throw ExceptionValidationError(Ex);");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        public TEntity UpdateGetItem(TEntity Model)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            //_DbSet.Attach(Model);");
            TextClass.AppendLine("            //_Context.SaveChanges();");
            TextClass.AppendLine("            // Or -->...");
            TextClass.AppendLine("            try");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                var entry = _Context.Entry<TEntity>(Model);");
            TextClass.AppendLine("                var pkey = _DbSet.Create().GetType().GetProperty(\"ID\").GetValue(Model);");
            TextClass.AppendLine("");
            TextClass.AppendLine("                if (entry.State == EntityState.Detached)");
            TextClass.AppendLine("                {");
            TextClass.AppendLine("                    var set = _Context.Set<TEntity>();");
            TextClass.AppendLine("                    TEntity AttachedEntity = set.Find(pkey);");
            TextClass.AppendLine("");
            TextClass.AppendLine("                    if (AttachedEntity != null)");
            TextClass.AppendLine("                    {");
            TextClass.AppendLine("                        var AttachedEntry = _Context.Entry(AttachedEntity);");
            TextClass.AppendLine("                        AttachedEntry.CurrentValues.SetValues(Model);");
            TextClass.AppendLine("                    }");
            TextClass.AppendLine("                    else");
            TextClass.AppendLine("                    {");
            TextClass.AppendLine("                        entry.State = EntityState.Modified;");
            TextClass.AppendLine("                    }");
            TextClass.AppendLine("                }");
            TextClass.AppendLine("");
            TextClass.AppendLine("                _Context.SaveChanges();");
            TextClass.AppendLine("                return (Model);");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("            catch (System.Data.Entity.Validation.DbEntityValidationException Ex)");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                CreateFileLog(Ex.InnerException.Message);");
            TextClass.AppendLine("                throw ExceptionValidationError(Ex);");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        public void Delete(TEntity Model)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            try");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                _DbSet.Remove(Model);");
            TextClass.AppendLine("                _Context.SaveChanges();");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("            catch (System.Data.Entity.Validation.DbEntityValidationException Ex)");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                CreateFileLog(Ex.InnerException.Message);");
            TextClass.AppendLine("                throw ExceptionValidationError(Ex);");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        public TEntity GetItem(Expression<Func<TEntity, bool>> Filter = null)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            try");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                IQueryable<TEntity> Query = _DbSet;");
            TextClass.AppendLine("                if (Filter != null) { Query = Query.Where(Filter); }");
            TextClass.AppendLine("                return Query.ToList().FirstOrDefault();");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("            catch (System.Data.Entity.Validation.DbEntityValidationException Ex)");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                CreateFileLog(Ex.InnerException.Message);");
            TextClass.AppendLine("                throw ExceptionValidationError(Ex);");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        public virtual IQueryable<TEntity> GetByFilter(Expression<Func<TEntity, bool>> Filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy = null)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            try");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                IQueryable<TEntity> Query = _DbSet;");
            TextClass.AppendLine("                if (Filter != null) { Query = Query.Where(Filter); }");
            TextClass.AppendLine("                if (OrderBy != null) { return OrderBy(Query); } else { return Query; }");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("            catch (System.Data.Entity.Validation.DbEntityValidationException Ex)");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                CreateFileLog(Ex.InnerException.Message);");
            TextClass.AppendLine("                throw ExceptionValidationError(Ex);");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");  
            TextClass.AppendLine("        public virtual IQueryable<TEntity> GetAll()");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("           return (_DbSet.AsQueryable<TEntity>());");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        public void DeleteAll(Expression<Func<TEntity, bool>> Filter = null)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            try");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                IQueryable<TEntity> Query = _DbSet;");
            TextClass.AppendLine("                List<TEntity> CollectionTEntity = Query.Where(Filter).ToList();");
            TextClass.AppendLine("");
            TextClass.AppendLine("                foreach (var ItemTEntity in CollectionTEntity)");
            TextClass.AppendLine("                {");
            TextClass.AppendLine("                    _DbSet.Remove(ItemTEntity);");
            TextClass.AppendLine("                }");
            TextClass.AppendLine("");
            TextClass.AppendLine("                _Context.SaveChanges();");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("            catch (Exception Ex)");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                CreateFileLog(Ex.InnerException.Message);");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        private Exception ExceptionValidationError(System.Data.Entity.Validation.DbEntityValidationException Ex)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            Exception Raise = Ex;");
            TextClass.AppendLine("            foreach (var ValidationErrors in Ex.EntityValidationErrors)");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                foreach (var ValidationError in ValidationErrors.ValidationErrors)");
            TextClass.AppendLine("                {");
            TextClass.AppendLine("                    var Message = String.Format(\"{0}:{1}\", ValidationErrors.Entry.Entity.ToString(), ValidationError.ErrorMessage);");
            TextClass.AppendLine("                    Raise = new InvalidOperationException(Message, Raise);");
            TextClass.AppendLine("                }");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("            return (Raise);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        private void CreateFileLog(String ErrorMessage, String PathLog = \"\")");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            var Path = (String.IsNullOrEmpty(PathLog) ? String.Format(ConfigurationManager.AppSettings[\"Log\"].ToString()) : PathLog);");
            TextClass.AppendLine("");
            TextClass.AppendLine("            if (!System.IO.File.Exists(Path))");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                using (System.IO.StreamWriter SWriter = System.IO.File.CreateText(Path))");
            TextClass.AppendLine("                {");
            TextClass.AppendLine("                    SWriter.WriteLine(ErrorMessage);");
            TextClass.AppendLine("                }");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        public void Dispose()");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            _DbSet = null;");
            TextClass.AppendLine("            _Context.Dispose();");
            TextClass.AppendLine("            GC.SuppressFinalize(this);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("    }");
            TextClass.AppendLine("}");  

            return CreateFile("RepositoryGeneric", "Repository"); 
        }

        public String BuildIRespository()
        {
            TextClass = new StringBuilder();
            TextClass.AppendLine("using System;");
            TextClass.AppendLine("using System.Collections.Generic;");
            TextClass.AppendLine("using System.Linq;");
            TextClass.AppendLine("using System.Linq.Expressions;");
            TextClass.AppendLine("using System.Text;");
            TextClass.AppendLine("using System.Threading.Tasks;");
            TextClass.AppendLine("");
            TextClass.AppendLine("namespace " + ProjectName + ".Data.Repository");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    public interface IRepository<TEntity>");
            TextClass.AppendLine("    {");
            TextClass.AppendLine("        void Add(TEntity Model);");
            TextClass.AppendLine("        void Edit(TEntity Model);");
            TextClass.AppendLine("        void Delete(TEntity Model);");
            TextClass.AppendLine("        void DeleteAll(Expression<Func<TEntity, bool>> Filter = null);");
            TextClass.AppendLine("        TEntity AddGetItem(TEntity Model);");
            TextClass.AppendLine("        TEntity UpdateGetItem(TEntity Model);");
            TextClass.AppendLine("        TEntity GetItem(Expression<Func<TEntity, bool>> Filter = null);");
            TextClass.AppendLine("        IQueryable<TEntity> GetByFilter(Expression<Func<TEntity, bool>> Filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy = null);");
            TextClass.AppendLine("        IQueryable<TEntity> GetAll();");
            TextClass.AppendLine("    }");
            TextClass.AppendLine("}");

            return CreateFile("IRepository", "Repository"); 
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

    }
}