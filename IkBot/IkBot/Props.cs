using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace IkBot
{
    class Props
    {
        private static Object obj = new Object();
        private static CookieContainer cookies;
        public static CookieContainer Cookies
        {
            get
            {
                lock (obj)
                {
                    return cookies;
                }
            }
            set
            {
                lock (obj)
                {
                    cookies = value;
                }
            }
        }
    }
}
