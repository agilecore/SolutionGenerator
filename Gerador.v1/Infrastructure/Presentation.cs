using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gerador.Enums;
using Gerador.Models;
using System.IO;

namespace Gerador.Infrastructure
{
    public class Presentation
    {
        private StringBuilder TextClass;
        private String FilePath = ConfigurationManager.AppSettings["CamadaDeFrontend"].ToString();
        private String ProjectName = ConfigurationManager.AppSettings["NomeDoProjeto"].ToString();
        private EDataBase DatabaseType = Gerador.Program.DatabaseType;
        private String Sufixo = "Controller";

        public String BuildBase()
        {
            TextClass = new StringBuilder();
            TextClass.AppendLine("using System;");
            TextClass.AppendLine("using System.Collections.Generic;");
            TextClass.AppendLine("using System.Linq;");
            TextClass.AppendLine("using System.Web;");
            TextClass.AppendLine("using System.Web.Mvc;");
            TextClass.AppendLine("");
            TextClass.AppendLine("namespace " + ProjectName + ".WebUI.Controllers");
            TextClass.AppendLine("{");
            TextClass.AppendLine("    public class Base : Controller");
            TextClass.AppendLine("    {");
            TextClass.AppendLine("        public int PageSize = 50;");
            TextClass.AppendLine("    }");
            TextClass.AppendLine("}");

            return CreateFile("Base", "Controllers");
        }

        public string BuildController(String ClassName)
        {
            TextClass = new StringBuilder();
            TextClass.AppendLine("using System;                                                                       ");
            TextClass.AppendLine("using System.Collections.Generic;								                      ");
            TextClass.AppendLine("using System.Linq;                                                                  ");
            TextClass.AppendLine("using System.Web;                                                                   ");
            TextClass.AppendLine("using System.Web.Mvc;                                                               ");
            TextClass.AppendLine("using " + ProjectName + ".Common;                                                   ");
            TextClass.AppendLine("using " + ProjectName + ".Domain;                                                   ");
            TextClass.AppendLine("                                                                                    ");
            TextClass.AppendLine("namespace " + ProjectName + ".WebUI.Controllers                                     ");
            TextClass.AppendLine("{                                                                                   ");
            TextClass.AppendLine("    public class " + ClassName + "Controller : Base                                 ");
            TextClass.AppendLine("    {                                                                               ");
            TextClass.AppendLine("        internal " + ClassName + " _" + ClassName + " { get; set; }                 ");
            TextClass.AppendLine("                                                                                    ");
            TextClass.AppendLine("        public " + ClassName + "Controller()                                        ");
            TextClass.AppendLine("        {                                                                           ");
            TextClass.AppendLine("            _" + ClassName + " = new " + ClassName + "();                           ");
            TextClass.AppendLine("        }                                                                           ");
            TextClass.AppendLine("                                                                                    ");
            TextClass.AppendLine("        public ActionResult List()                                                  ");
            TextClass.AppendLine("        {                                                                           ");
            TextClass.AppendLine("            var collection" + ClassName + " = _" + ClassName + ".GetAll().ToList(); ");
            TextClass.AppendLine("            return View(collection" + ClassName + ");                               ");
            TextClass.AppendLine("        }                                                                           ");
            TextClass.AppendLine("                                                                                    ");
            TextClass.AppendLine("        public ActionResult Create()                                                ");
            TextClass.AppendLine("        {                                                                           ");
            TextClass.AppendLine("            return View(new " + ClassName + "Dto());                                ");
            TextClass.AppendLine("        }                                                                           ");
            TextClass.AppendLine("                                                                                    ");
            TextClass.AppendLine("        [HttpPost]                                                                  ");
            TextClass.AppendLine("        public ActionResult Save(" + ClassName + "Dto model)                        ");
            TextClass.AppendLine("        {                                                                           ");
            TextClass.AppendLine("            if (ModelState.IsValid)                                                 ");
            TextClass.AppendLine("            {                                                                       ");
            TextClass.AppendLine("                var " + ClassName + " = new " + ClassName + "();                    ");
            TextClass.AppendLine("                _" + ClassName + ".Save(model);                                     ");
            TextClass.AppendLine("                return RedirectToAction(\"List\");                                  ");
            TextClass.AppendLine("            }                                                                       ");
            TextClass.AppendLine("            else                                                                    ");
            TextClass.AppendLine("            {                                                                       ");
            TextClass.AppendLine("                return RedirectToAction(\"Create\", model);                         ");
            TextClass.AppendLine("            }                                                                       ");
            TextClass.AppendLine("        }                                                                           ");
            TextClass.AppendLine("    }                                                                               ");
            TextClass.AppendLine("}                                                                                   ");

            var FileName = String.Format(@"{0}.{1}", String.Concat(ClassName, Sufixo), "cs");
            var Diretory = String.Format(@"{0}\{1}\{2}", FilePath, "Controllers", ClassName);
            var FullFile = String.Format(@"{0}\{1}", Diretory, FileName);
            var DirInfo = new DirectoryInfo(Diretory);

            if (!DirInfo.Exists) { DirInfo.Create(); }

            if (!File.Exists(FullFile))
            {
                using (System.IO.TextWriter Writer = File.CreateText(FullFile))
                {
                    Writer.WriteLine(TextClass.ToString());
                }
            }

            return FileName;
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
