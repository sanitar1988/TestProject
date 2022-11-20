using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient.Models
{
    [Serializable]
    public class UserFirstInfo
    { 
        public string Username { get; set; }
        public string Userpassword { get; set; }
        public string Useremail { get; set; }
    }
}
