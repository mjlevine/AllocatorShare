using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService
{
    public class ConfigurationHelper
    {
        public static string ClientSecret 
        {
            get { return ConfigurationManager.AppSettings["ShareFileClientSecret"]; }
        }

        public static string ClientId
        {
            get { return ConfigurationManager.AppSettings["ShareFileClientId"]; }
        }
    }
}
