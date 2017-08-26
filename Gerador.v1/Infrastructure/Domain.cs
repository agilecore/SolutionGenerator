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
            TextClass.AppendLine("using " + ProjectName + ".Common;");
            TextClass.AppendLine("");
            TextClass.AppendLine("/// <summary>");
            TextClass.AppendLine("/// Esta classe contem metodos de negocios (domain) principais, prontos para uso em controllers, porem se esse objeto");
            TextClass.AppendLine("/// necessita de um metodo customizado (particular), fazer a implementacao na classe Specialized, e nao aqui.");
            TextClass.AppendLine("/// </summary>");
            TextClass.AppendLine("namespace " + ProjectName + ".Domain");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    public class " + DataModel.ClassName);
            TextClass.AppendLine("    {");
            TextClass.AppendLine("        public UnitOfWork _unitOfWork {get; set;}");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Construtor");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public " + DataModel.ClassName + "()");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            _unitOfWork = new UnitOfWork();");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Salva um objeto<T>");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public virtual void Save(" + String.Concat(DataModel.ClassName, Sufixo) + " model)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            _unitOfWork.GetRepository<" + String.Concat(DataModel.ClassName, Sufixo) + ">().Add(model);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Salva e retorna o objeto<T> salvo");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public virtual " + String.Concat(DataModel.ClassName, Sufixo) + " SaveGetItem(" + String.Concat(DataModel.ClassName, Sufixo) + " model)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("           _unitOfWork.GetRepository<" + String.Concat(DataModel.ClassName, Sufixo) + ">().Add(model);");
            TextClass.AppendLine("           return (model);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Salva uma lista de objetos List<T>");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public virtual void SaveAll(List<" + String.Concat(DataModel.ClassName, Sufixo) + "> model)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            _unitOfWork.GetRepository<" + String.Concat(DataModel.ClassName, Sufixo) + ">().AddAll(model);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Salva a edição de um objeto<T>");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public virtual void Update(" + String.Concat(DataModel.ClassName, Sufixo) + " model)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            _unitOfWork.GetRepository<" + String.Concat(DataModel.ClassName, Sufixo) + ">().Update(model);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Retorna um único objeto<T> buscado por expressão Lambda");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public virtual " + String.Concat(DataModel.ClassName, Sufixo) + " GetItem(Expression<Func<" + String.Concat(DataModel.ClassName, Sufixo) + ", bool>> filter)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            " + String.Concat(DataModel.ClassName, Sufixo) + " model;");
            TextClass.AppendLine("            model = _unitOfWork.GetRepository<" + String.Concat(DataModel.ClassName, Sufixo) + ">().GetByFilters(filter).FirstOrDefault();");
            TextClass.AppendLine("            return (model);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Deleta um ou uma lista de objetos");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public virtual void Delete(Expression<Func<" + String.Concat(DataModel.ClassName, Sufixo) + ", bool>> filter)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("             _unitOfWork.GetRepository<" + String.Concat(DataModel.ClassName, Sufixo) + ">().Delete(filter);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Retorna uma lista List(T) de objetos buscados pela expressão Lambda");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        public List<" + DataModel.ClassName + Sufixo +"> GetByFilters(Expression<Func<" + DataModel.ClassName + Sufixo + ", bool>> Filter = null)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            var Collection = _unitOfWork.GetRepository<" + String.Concat(DataModel.ClassName, Sufixo) + ">().GetByFilters(Filter);");
            TextClass.AppendLine("            return (Collection.ToList());");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");        
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Retorna um objeto IQueryable manipulavel em tempo de execução.");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        /// <param name=\"Filter\">Filtro exemplo: GetByFilter(obj => obj.ID, null).</param>");
            TextClass.AppendLine("        /// <returns>Retorna um objeto IQueryable</returns>");
            TextClass.AppendLine("        public IQueryable<" + DataModel.ClassName + Sufixo + "> GetByFilterAsQueryable(Expression<Func<" + DataModel.ClassName + Sufixo + ", bool>> Filter = null)");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            var Collection = _unitOfWork.GetRepository<" + String.Concat(DataModel.ClassName, Sufixo) + ">().GetByFilters(Filter);");
            TextClass.AppendLine("            return (Collection.AsQueryable<" + DataModel.ClassName + Sufixo + ">());");
            TextClass.AppendLine("        }");
            TextClass.AppendLine("");        
            TextClass.AppendLine("        /// <summary>");
            TextClass.AppendLine("        /// Retorna um objeto IQueryable manipulavel em tempo de execução.");
            TextClass.AppendLine("        /// </summary>");
            TextClass.AppendLine("        /// <returns>Retorna um objeto IQueryable</returns>");
            TextClass.AppendLine("        public IQueryable<" + DataModel.ClassName + Sufixo + "> GetAll()");
            TextClass.AppendLine("        {");
            TextClass.AppendLine("            var model = _unitOfWork.GetRepository<" + String.Concat(DataModel.ClassName, Sufixo) + ">().GetAll().AsQueryable();");
            TextClass.AppendLine("            return (model);");
            TextClass.AppendLine("        }");
            TextClass.AppendLine(""); 
                                 
            //foreach (var ColumnMapper in TableSchema.CollectionColumn)
            //{
            //    if (ColumnMapper.ColumnName == "STATUS")
            //    {
            //        TextClass.AppendLine("        /// <summary>");
            //        TextClass.AppendLine("        /// Inativa um objeto para visualização");
            //        TextClass.AppendLine("        /// </summary>");
            //        TextClass.AppendLine("        public virtual void ToInactive(int Id)");
            //        TextClass.AppendLine("        {");
            //        TextClass.AppendLine("            " + String.Concat(DataModel.ClassName, Sufixo) + " model = _unitOfWork." + DataModel.ClassName + "Repository.GetItem(_ => _.ID == Id && _.STATUS == \"A\");");
            //        TextClass.AppendLine("            model.STATUS = \"I\";");
            //        TextClass.AppendLine("            _unitOfWork.GetRepository<" + String.Concat(DataModel.ClassName, Sufixo) + ">().Edit(model);");
            //        TextClass.AppendLine("        }");
            //        TextClass.AppendLine("");
            //        TextClass.AppendLine("        /// <summary>");
            //        TextClass.AppendLine("        /// Ativa um objeto para visualização");
            //        TextClass.AppendLine("        /// </summary>");
            //        TextClass.AppendLine("        public virtual void ToActive(int Id)");
            //        TextClass.AppendLine("        {");
            //        TextClass.AppendLine("            " + String.Concat(DataModel.ClassName, Sufixo) + " model = _unitOfWork." + DataModel.ClassName + "Repository.GetItem(_ => _.ID == Id && _.STATUS == \"I\");");
            //        TextClass.AppendLine("            model.STATUS = \"A\";");
            //        TextClass.AppendLine("            _unitOfWork.GetRepository<" + String.Concat(DataModel.ClassName, Sufixo) + ">().Edit(model);");
            //        TextClass.AppendLine("        }");
            //        TextClass.AppendLine("");
            //    }
            //}   

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
            TextClass.AppendLine("");
            TextClass.AppendLine("namespace " + ProjectName + ".Domain");
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
