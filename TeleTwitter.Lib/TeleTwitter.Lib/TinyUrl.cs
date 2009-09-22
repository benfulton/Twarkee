using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Web;

namespace TeleTwitter.Lib
{
    public class TinyUrl
    {
        public static string GetTinyURL(string url)
        {
            string tinyurl = null;

            try
            {
                // Build & submit the POST request to tinyurl.com.
                //

                url = HttpUtility.HtmlDecode(url);
                byte[] requestData = Encoding.UTF8.GetBytes("url=" + HttpUtility.UrlEncode(url));

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://tinyurl.com/create.php");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = requestData.Length;

                // Without the following, the POST packet will include an "Expect"
                // header equal to "100-continue".  This causes the tinyurl.com
                // server to reply with a "417 Expectation Failed" reply when
                // GetRequestStream is called.
                //
                ServicePointManager.Expect100Continue = false;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(requestData, 0, requestData.Length);
                }

                // Grab the response from tinyurl.com.
                //
                StreamReader response = new StreamReader(request.GetResponse().GetResponseStream());
                string responseData = response.ReadToEnd();

                // Locate the super, special, secret hidden input tag that tinyurl.com
                // includes in its response (they do this so that on >= IE4 some javascript
                // on the response page can copy the URL to the clipboard).  Once this <input> tag
                // is located, grab the value attribute.
                //
                Regex regexp = new Regex("<input +type=hidden +name=tinyurl +value=[\"]?(?<tinyurl>[^\"]+)[\"]? *>", RegexOptions.IgnoreCase);
                Match match = regexp.Match(responseData);

                if (match.Success)
                {
                    // Got it!
                    //
                    tinyurl = match.Groups["tinyurl"].Value;
                }
            }
            catch
            {
            }
            return tinyurl;
        }
    }
}
