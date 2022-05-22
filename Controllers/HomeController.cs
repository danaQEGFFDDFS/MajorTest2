using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProgressCheck.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProgressCheck.Controllers
{
    public class HomeController : Controller
    {
        private int fibbo(int n)
        {
            if (n == 1 || n == 2)
                return 1;
            return fibbo(n - 2) + fibbo(n - 1); //1 1 2 3 5 8 
        }

        public static class InternetTime
        {
            public static DateTimeOffset? GetCurrentTime()
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        var result = client.GetAsync("https://google.com",
                              HttpCompletionOption.ResponseHeadersRead).Result;
                        return result.Headers.Date;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]

        public IActionResult Fibo(int n)

        {
            var result = fibbo(n);
            //return Ok(result);
            return new JsonResult(new { ResultFinal = result });
        }
        public IActionResult time()
        {
                     
            DateTime dateTime = DateTime.MinValue;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://nist.time.gov/actualtime.cgi?lzbc=siqm9b");
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore); //No caching
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader stream = new StreamReader(response.GetResponseStream());
                string html = stream.ReadToEnd();//<timestamp time=\"1395772696469995\" delay=\"1395772696469995\"/>
                string time = Regex.Match(html, @"(?<=\btime="")[^""]*").Value;
                double milliseconds = Convert.ToInt64(time) / 1000.0;
                dateTime = new DateTime(1970, 1, 1).AddMilliseconds(milliseconds).ToLocalTime();
            }
            //DateTime dateTime = new DateTime();
            // dateTime = dateTime.ToLocalTime();
            return new JsonResult(new { TimeResult = dateTime });
        }


    }
}
