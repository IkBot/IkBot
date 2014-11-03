using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
namespace IkBot
{
    class Http
    {
        public static HttpWebRequest GetNewRequest(string targetUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(targetUrl);
            request.CookieContainer = Props.Cookies;
            request.AllowAutoRedirect = false;
            return request;
        }
        public async static Task<HttpWebResponse> MakeRequest(HttpWebRequest request, Dictionary<string, string> parameters = null)
        {
            HttpWebResponse response;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.5 (KHTML, like Gecko) Chrome/19.0.1084.52 Safari/536.5Accept: */*";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.CookieContainer = Props.Cookies;
            request.AllowAutoRedirect = false;
            if (parameters != null)
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                string s = "";
                foreach (KeyValuePair<string, string> pair in parameters)
                {
                    if (s.Length == 0)
                    {
                        s = s + string.Format("{0}={1}", pair.Key, pair.Value);
                    }
                    else
                    {
                        s = s + string.Format("&{0}={1}", pair.Key, pair.Value);
                    }
                }
                byte[] bytes = Encoding.UTF8.GetBytes(s);
                using (Stream stream = await request.GetRequestStreamAsync())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            request.Method = "GET";
            response = await request.GetResponseAsync() as HttpWebResponse;
            Props.Cookies.Add(response.Cookies);
            while (response.StatusCode == HttpStatusCode.Found)
            {
                response.Close();
                request = GetNewRequest(response.Headers["Location"]);
                response = await request.GetResponseAsync() as HttpWebResponse;
                Props.Cookies.Add(response.Cookies);
            }
            return response;
        }
    }
}
