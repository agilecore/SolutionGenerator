using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador.Models
{
    public sealed class ModelConfig
    {
        public string ClassName { get; set; }
        public string NameSpaceDto { get; set; }
        public string NameSpaceMapper { get; set; }
        public string NameSpaceDomain { get; set; }
        public string NameSpaceService { get; set; }
        public Boolean CreateController { get; set; }
    }
}
