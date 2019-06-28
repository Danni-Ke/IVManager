using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVSSO.SSOControllers
{
    public class LoginReturnModel
    {
        public bool result { get; set; }
        public string userName { get; set; }
        public string userId { get; set; }
        public int userType { get; set; }
        public string error { get; set; }
        public string access_Token { get; set;}

        public LoginReturnModel() { }
        public LoginReturnModel(bool result, string userName, string userId, int userType, string access_Token)
        {
            //成功返回的
            this.result = result;
            this.userName = userName;
            this.userId = userId;
            this.userType = userType;
            this.access_Token = access_Token;
        }
       
        public LoginReturnModel(bool result,string errormessage)
        {
            //失败返回的
            this.result = result;
            error = errormessage;
        }
    }
}
