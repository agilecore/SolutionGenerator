using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gerador.Models;

namespace Gerador.Interface
{
    public interface IData
    {
        String BuildBase();
        String BuildModels(KeyValuePair<String, ModelConfig> TableSetting);
        String BuildMapper(KeyValuePair<String, ModelConfig> TableSetting);
        String BuildUnitOfWork(Dictionary<String, ModelConfig> GroupTables);
        String BuildContext(Dictionary<String, ModelConfig> GroupTables);
        String BuildRespository();
        String BuildIRespository();
        String CreateFile(String ClassName, String Folder);
    }
}
