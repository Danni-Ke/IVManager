using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManager.DeveloperControllers
{
    public class ReturnFail
    {
        public bool result { get; set; } = false;
        public string error { get; set; }
        public ReturnFail(string errormessage)
        {
            error = errormessage;
        }
    }

}
