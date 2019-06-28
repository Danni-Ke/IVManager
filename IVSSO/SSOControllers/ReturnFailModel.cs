using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVSSO.SSOControllers
{
    public class ReturnFailModel
    {
        public bool result { get; set; } = false;
        public string error { get; set; }
        public ReturnFailModel(string errormessage)
        {
            error = errormessage;
        }
    }

}
