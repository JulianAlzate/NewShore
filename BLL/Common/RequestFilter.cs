using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Common
{
   public class RequestFilter
    {
        public string origin { get; set; }
        public string destination { get; set; }
        public bool conMultiple { get; set; }
        public bool conRetorno { get; set; }
    }
}
