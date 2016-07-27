using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;     
using System.IO;
using Gerador.Enums;
using Gerador.Models;


namespace Gerador.Infrastructure
{
    public class Domain
    {
        private StringBuilder TextClass;
        private String FilePath = ConfigurationManager.AppSettings["CamadaDeDominio"].ToString();
        private String ProjectName = ConfigurationManager.AppSettings["NomeDoProjeto"].ToString();
        private EDataBase DatabaseType = Gerador.Program.DatabaseType;

        public string BuildModels(KeyValuePair<String, ModelConfig> TableSetting)
        {
            var TableName = TableSetting.Key;
            var DataModel = TableSetting.Value;
            var TableSchema = Utils.GetTableSchema(TableName, DatabaseType);
            var Sufixo = "Dto";

            //--  
            TextClass = new StringBuilder();       
            TextClass.AppendLine("using System;");
            TextClass.AppendLine("using System.Text;");
            TextClass.AppendLine("using System.Collections;");
            TextClass.AppendLine("using System.Collections.Generic;");
            TextClass.AppendLine("using System.Linq;");
            TextClass.AppendLine("using System.Threading.Tasks;");
            TextClass.AppendLine("using System.Linq.Expressions;");
            TextClass.AppendLine("using " + ProjectName + ".Data;");
            TextClass.AppendLine("using " + ProjectName + ".Data.Model;"); 
            TextClass.AppendLine("");
            TextClass.AppendLine("/// <summary>");
            TextClass.AppendLine("/// Esta classe abstrata não pode ser instanciada. O objetivo desta classe é fornecer uma definição de metodos");
            TextClass.AppendLine("/// base comuns para que várias outras classes derivadas desta possam compartilhar metodos por 'override'.");
            TextClass.AppendLine("/// </summary>");
            TextClass.AppendLine("namespace " + ProjectName + ".Domain.Model");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    public abstract class " + DataModel.ClassName);
            TextClass.AppendLine("    {");
            TextClass.AppendLine("        public " + ProjectName + ".Data.UnitOfWork _unitOfWork = new " + ProjectName + ".Data.UnitOfWork();");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Salva um objeto<T>");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public virtual void Save(" + String.Concat(DataModel.ClassName, Sufixo) + " model)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            _unitOfWork." + DataModel.ClassName + "Repository.Add(model);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Salva e retorna o objeto<T> salvo");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public virtual " + String.Concat(DataModel.ClassName, Sufixo) + " SaveGetItem(" + String.Concat(DataModel.ClassName, Sufixo) + " model)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("           var retorno = _unitOfWork." + DataModel.ClassName + "Repository.AddGetItem(model);");
            TextClass.AppendLine("           return (retorno);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Salva uma lista de objetos List<T>");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public virtual void SaveAll(List<" + String.Concat(DataModel.ClassName, Sufixo) + "> model)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            _unitOfWork." + DataModel.ClassName + "Repository.AddAll(model);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Salva a edição de um objeto<T>");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public virtual void Edit(" + String.Concat(DataModel.ClassName, Sufixo) + " model)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            _unitOfWork." + DataModel.ClassName + "Repository.Edit(model);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Retorna um único objeto<T> buscado por expressão Lambda");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public virtual " + String.Concat(DataModel.ClassName, Sufixo) + " GetItem(Expression<Func<" + String.Concat(DataModel.ClassName, Sufixo) + ", bool>> filter)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            " + String.Concat(DataModel.ClassName, Sufixo) + " model;");
            TextClass.AppendLine("            model = _unitOfWork." + DataModel.ClassName + "Repository.GetItem(filter);");
            TextClass.AppendLine("            return (model);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Deleta um objeto");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public virtual void Delete(Expression<Func<" + String.Concat(DataModel.ClassName, Sufixo) + ", bool>> filter)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            var Collection = _unitOfWork." + DataModel.ClassName + "Repository.GetByFilter(filter);");
            TextClass.AppendLine("            if (Collection.ToList().Count > 0)");
            TextClass.AppendLine("            {");
            TextClass.AppendLine("                foreach (var item in Collection)");
            TextClass.AppendLine("                {");
            TextClass.AppendLine("                    _unitOfWork." + DataModel.ClassName + "Repository.Delete(item);");
            TextClass.AppendLine("                }");
            TextClass.AppendLine("            }");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Deleta uma lista de objetos");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public virtual void DeleteAll(List<" + String.Concat(DataModel.ClassName, Sufixo) + "> collection)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            foreach (var item in collection) { _unitOfWork." + DataModel.ClassName + "Repository.Delete(item); }");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Retorna uma lista List(T) de objetos buscados pela expressão Lambda");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public virtual List<" + String.Concat(DataModel.ClassName, Sufixo) + "> GetList(Expression<Func<" + String.Concat(DataModel.ClassName, Sufixo) + ", bool>> filter)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            List<" + String.Concat(DataModel.ClassName, Sufixo) + "> collection;");
            TextClass.AppendLine("            if (filter == null) { collection = _unitOfWork." + DataModel.ClassName + "Repository.GetByFilter(filter).Take(5000).ToList(); } else { collection = _unitOfWork." + DataModel.ClassName + "Repository.GetByFilter(filter).ToList(); }");
            TextClass.AppendLine("            return (collection);");  
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Retorna uma lista por filtro e ordenada.");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        /// <param name=\"Filter\">Filtro exemplo: GetByFilter(obj => obj.ID, null).</param>");
            TextClass.AppendLine("        /// <param name=\"OrderBy\">Ordenação exemplo: GetByFilter(null, obj => obj.OrderBy(obj => obj.TITULO))</param>");
            TextClass.AppendLine("        /// <returns>Retorna uma lista de objeto</returns>");
            TextClass.AppendLine("        public List<" + DataModel.ClassName + "Dto> GetByFilter(Expression<Func<" + DataModel.ClassName + "Dto, bool>> Filter = null, Func<IQueryable<" + DataModel.ClassName + "Dto>, IOrderedQueryable<" + DataModel.ClassName + "Dto>> OrderBy = null)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            var Collection = _unitOfWork." + DataModel.ClassName + "Repository.GetByFilter(Filter, OrderBy);");
            TextClass.AppendLine("            return (Collection.ToList());");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");        
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Retorna um objeto IQueryable manipulavel em tempo de execução.");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        /// <param name=\"Filter\">Filtro exemplo: GetByFilter(obj => obj.ID, null).</param>");
            TextClass.AppendLine("        /// <param name=\"OrderBy\">Ordenação exemplo: GetByFilter(null, obj => obj.OrderBy(obj => obj.TITULO))</param>");
            TextClass.AppendLine("        /// <returns>Retorna um objeto IQueryable</returns>");
            TextClass.AppendLine("        public IQueryable<" + DataModel.ClassName + "Dto> GetByFilterAsQueryable(Expression<Func<" + DataModel.ClassName + "Dto, bool>> Filter = null, Func<IQueryable<" + DataModel.ClassName + "Dto>, IOrderedQueryable<" + DataModel.ClassName + "Dto>> OrderBy = null)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            var Collection = _unitOfWork." + DataModel.ClassName + "Repository.GetByFilter(Filter, OrderBy);");
            TextClass.AppendLine("            return (Collection.AsQueryable<" + DataModel.ClassName + "Dto>());");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");        
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Retorna um objeto IQueryable manipulavel em tempo de execução.");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        /// <returns>Retorna um objeto IQueryable</returns>");
            TextClass.AppendLine("        public IQueryable<" + DataModel.ClassName + "Dto> GetAll()");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            var model = _unitOfWork." + DataModel.ClassName + "Repository.GetAll().AsQueryable();");
            TextClass.AppendLine("            return (model);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine(""); 
                                 
            foreach (var ColumnMapper in TableSchema.CollectionColumn)
            {
                if (ColumnMapper.ColumnName == "STATUS")
                {
                    TextClass.AppendLine("        /// <summary>");
                    TextClass.AppendLine("        /// Inativa um objeto para visualização");
                    TextClass.AppendLine("        /// </summary>");
                    TextClass.AppendLine("        public virtual void ToInactive(int Id)");
                    TextClass.AppendLine("        {");
                    TextClass.AppendLine("            " + String.Concat(DataModel.ClassName, Sufixo) + " model = _unitOfWork." + DataModel.ClassName + "Repository.GetItem(_ => _.ID == Id && _.STATUS == \"A\");");
                    TextClass.AppendLine("            model.STATUS = \"I\";");
                    TextClass.AppendLine("            _unitOfWork." + DataModel.ClassName + "Repository.Edit(model);");
                    TextClass.AppendLine("        }");
                    TextClass.AppendLine("");
                    TextClass.AppendLine("        /// <summary>");
                    TextClass.AppendLine("        /// Ativa um objeto para visualização");
                    TextClass.AppendLine("        /// </summary>");
                    TextClass.AppendLine("        public virtual void ToActive(int Id)");
                    TextClass.AppendLine("        {");
                    TextClass.AppendLine("            " + String.Concat(DataModel.ClassName, Sufixo) + " model = _unitOfWork." + DataModel.ClassName + "Repository.GetItem(_ => _.ID == Id && _.STATUS == \"I\");");
                    TextClass.AppendLine("            model.STATUS = \"A\";");
                    TextClass.AppendLine("            _unitOfWork." + DataModel.ClassName + "Repository.Edit(model);");
                    TextClass.AppendLine("        }");
                    TextClass.AppendLine("");
                }
            }   

            TextClass.AppendLine("    }");
            TextClass.AppendLine("}");
            //--  

            return CreateFile(DataModel.ClassName);
        }

        public String BuildModelsSpecialized(String ClassName)
        {
            var Sufixo = "Specialized";

            TextClass = new StringBuilder();
            TextClass.AppendLine("using System;");
            TextClass.AppendLine("using System.Text;");
            TextClass.AppendLine("using System.Collections;");
            TextClass.AppendLine("using System.Collections.Generic;");
            TextClass.AppendLine("using System.Linq;");
            TextClass.AppendLine("using System.Threading.Tasks;");
            TextClass.AppendLine("using System.Linq.Expressions;");
            TextClass.AppendLine("using " + ProjectName + ".Data;");
            TextClass.AppendLine("using " + ProjectName + ".Data.Model;");
            TextClass.AppendLine("");
            TextClass.AppendLine("namespace " + ProjectName + ".Domain.Model");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    public partial class " + String.Concat(ClassName, Sufixo) + " : " + ClassName);
            TextClass.AppendLine("    {");
            TextClass.AppendLine("");
            TextClass.AppendLine("    }");
            TextClass.AppendLine("}");

            var FileName = String.Format(@"{0}.{1}", String.Concat(ClassName, Sufixo), "cs");
            var Diretory = String.Format(@"{0}\{1}\{2}", FilePath, "Models", ClassName);
            var FullFile = String.Format(@"{0}\{1}", Diretory, FileName);

            if (!File.Exists(FullFile))
            {
                using (System.IO.TextWriter Writer = File.CreateText(FullFile))
                {
                    Writer.WriteLine(TextClass.ToString());
                }
            }

            return FileName;
        }

        public String BuildModelsValidations(KeyValuePair<String, ModelConfig> TableSetting)
        {
            var TableName = TableSetting.Key;
            var DataModel = TableSetting.Value;
            var TableSchema = Utils.GetTableSchema(TableName, DatabaseType);
            var SufixoValidation = "Annotation";

            var TextClass = new StringBuilder();
            TextClass.AppendLine("using System;");
            TextClass.AppendLine("using System.Text;");
            //TextClass.AppendLine("using System.Web.Mvc;");
            TextClass.AppendLine("using System.Collections;");
            TextClass.AppendLine("using System.Collections.Generic;");
            TextClass.AppendLine("using System.Linq;");
            TextClass.AppendLine("using System.ComponentModel.DataAnnotations;");
            TextClass.AppendLine("using System.Threading.Tasks;");
            TextClass.AppendLine("");
            TextClass.AppendLine("namespace " + ProjectName + ".Domain.Annotation");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    public class " + String.Concat(DataModel.ClassName,SufixoValidation));
            TextClass.AppendLine("    {");

            var ColumnDataType = String.Empty;

            foreach (var ColumnMapper in TableSchema.CollectionColumn)
            {
                if (ColumnMapper.ColumnKey != "pk")
                {
                    ColumnDataType = Utils.GetColumnType(ColumnMapper.DataType);

                    if (ColumnMapper.IsNullable == "no")
                    {
                        //if ((ColumnMapper.DataType == "text") || (ColumnMapper.DataType == "longtext") || (ColumnMapper.DataType == "tinytext") || (ColumnMapper.DataType == "mediumtext"))
                        //{
                        //    TextClass.AppendLine("        [AllowHtml]");
                        //}

                        TextClass.AppendLine("        [Required(ErrorMessage = \"Campo " + ColumnMapper.ColumnName + " é obrigatório.\")]");

                        if (ColumnMapper.MaxLenght != String.Empty)
                        {
                            if ((ColumnMapper.DataType == "text") || (ColumnMapper.DataType == "longtext") || (ColumnMapper.DataType == "tinytext") || (ColumnMapper.DataType == "mediumtext"))
                            {
                                TextClass.AppendLine("        [StringLength(65535, ErrorMessage = \"O campo " + ColumnMapper.ColumnName + " deve ter no máximo 65535 caracteres.\")]");
                            }
                            else
                            {
                                TextClass.AppendLine("        [StringLength(" + ColumnMapper.MaxLenght + ", ErrorMessage = \"O campo " + ColumnMapper.ColumnName + " deve ter no máximo " + ColumnMapper.MaxLenght + " caracteres.\")]");
                            }
                        }

                        TextClass.AppendLine("        public " + ColumnDataType + " " + ColumnMapper.ColumnName + " { get; set; }");
                        TextClass.AppendLine("");
                    }
                    else
                    {
                        TextClass.AppendLine("        public " + ColumnDataType + " " + ColumnMapper.ColumnName + " { get; set; }");
                        TextClass.AppendLine("");
                    }
                }
            }    

            TextClass.AppendLine("    }");
            TextClass.AppendLine("}");
            
            var FileName = String.Format(@"{0}.{1}", String.Concat(DataModel.ClassName, String.Empty), "cs");
            var Diretory = String.Format(@"{0}\{1}\{2}", FilePath, "Annotation", DataModel.ClassName);
            var FullFile = String.Format(@"{0}\{1}", Diretory, FileName);
            var DirInfo = new DirectoryInfo(Diretory);

            if (!DirInfo.Exists) { DirInfo.Create(); }
            using (TextWriter Writer = File.CreateText(FullFile)) { Writer.WriteLine(TextClass.ToString()); }
            TextClass = null;
                                                                        
            return FileName;
        }

        public String BuildUnitOfWork(Dictionary<String, ModelConfig> GroupTables)
        {
            TextClass = new StringBuilder();
            TextClass.AppendLine("using System;");
            TextClass.AppendLine("using System.Collections.Generic;");
            TextClass.AppendLine("using System.Linq;");
            TextClass.AppendLine("using System.Text;");
            TextClass.AppendLine("using System.Threading.Tasks;");
            TextClass.AppendLine("using " + ProjectName + ".Domain.Model;");
            TextClass.AppendLine("");
            TextClass.AppendLine("namespace " + ProjectName + ".Domain");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    public class UnitOfWork : IDisposable");
            TextClass.AppendLine("    {");
            TextClass.AppendLine("        private bool _Disposed = false;");
            TextClass.AppendLine("");

            foreach (var item in GroupTables)
            {
                var Model = item.Value;

                TextClass.AppendLine("        private " + Model.ClassName + " _" + Model.ClassName.ToLower() + "Domain;");
            }

            TextClass.AppendLine("");

            foreach (var item in GroupTables)
            {
                var Model = item.Value;

                TextClass.AppendLine("        public " + Model.ClassName + " " + Model.ClassName + "Domain");
                TextClass.AppendLine("        {");
                TextClass.AppendLine("            get");
                TextClass.AppendLine("            {");
                TextClass.AppendLine("                if (this._" + Model.ClassName.ToLower() + "Domain == null)");
                TextClass.AppendLine("                {");
                TextClass.AppendLine("                    this._" + Model.ClassName.ToLower() + "Domain = new " + Model.ClassName + "Specialized();");
                TextClass.AppendLine("                }");
                TextClass.AppendLine("                return _" + Model.ClassName.ToLower() + "Domain;");
                TextClass.AppendLine("            }");
                TextClass.AppendLine("        }");
            }

            TextClass.AppendLine("");
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

            foreach (var item in GroupTables)
            {
                var Model = item.Value;
                TextClass.AppendLine("                    _" + Model.ClassName.ToLower() + "Domain = null;");
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
            var Diretory = String.Format(@"{0}\{1}", FilePath, FileName);
            var DirInfo = new DirectoryInfo(FilePath);

            if (!DirInfo.Exists) { DirInfo.Create(); }
            using (TextWriter Writer = File.CreateText(Diretory)) { Writer.WriteLine(TextClass.ToString()); }
            return FileName;
        }

        private string CreateFile(String ClassName)
        {
            var FileName = String.Format(@"{0}.{1}", ClassName, "cs");
            var Diretory = String.Format(@"{0}\{1}\{2}", FilePath, "Models", ClassName);
            var FullFile = String.Format(@"{0}\{1}", Diretory, FileName);
            var DirInfo = new System.IO.DirectoryInfo(Diretory);

            if (!DirInfo.Exists) { DirInfo.Create(); }

            using (System.IO.TextWriter Writer = System.IO.File.CreateText(FullFile))
            {
                Writer.WriteLine(TextClass.ToString());
            }

            return FileName;
        } 
    }
}
