using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Models;

namespace UtilityTools.Core.Infrastructure
{
    public class HttpHelper
    {

        static HttpClient httpClient;
        static HttpHelper()
        {
            //https://dev.to/tswiftma/switching-from-httpclienthandler-to-socketshttphandler-17h3
            //https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient-guidelines
            var sslOptions = new SslClientAuthenticationOptions
            {
                // Leave certs unvalidated for debugging
                RemoteCertificateValidationCallback = delegate { return true; },
            };

            var handler = new SocketsHttpHandler()
            {
                UseProxy = false,
                Proxy= null,
                SslOptions = sslOptions,
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
            };
            string customUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";
            httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("User-Agent", customUserAgent);

        }

        public static async void DownloadFileAsync(string uri
        , string outputPath)
        {
            Uri uriResult;

            if (!Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
                throw new InvalidOperationException("URI is invalid.");

            if (!File.Exists(outputPath))
                throw new FileNotFoundException("File not found."
                   , nameof(outputPath));

            byte[] fileBytes = await httpClient.GetByteArrayAsync(uri);
            File.WriteAllBytes(outputPath, fileBytes);
        }

        /// <summary>  
        /// Unicode字符串转为正常字符串  
        /// </summary>  
        /// <param name="srcText"></param>  
        /// <returns></returns>  
        public static string UnicodeToString(string text)
        {
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(text, "\\\\u([\\w]{4})");
            if (mc != null && mc.Count > 0)
            {
                foreach (System.Text.RegularExpressions.Match m2 in mc)
                {
                    string v = m2.Value;
                    string word = v.Substring(2);
                    byte[] codes = new byte[2];
                    int code = Convert.ToInt32(word.Substring(0, 2), 16);
                    int code2 = Convert.ToInt32(word.Substring(2), 16);
                    codes[0] = (byte)code2;
                    codes[1] = (byte)code;
                    text = text.Replace(v, Encoding.Unicode.GetString(codes));
                }
            }
            else
            {

            }
            return text;
        }

        public static async Task<string> GetUrlContentAsync(string uriString,  Encoding encoding,  string authorization="",  bool cds=false,Dictionary<string,string> header=null, string refer="")
        {
            string result = string.Empty;
            HttpWebRequest request = null;
            HttpWebResponse webresponse = null;
            if (!string.IsNullOrEmpty(uriString))
            {
                try
                {
                    request = (HttpWebRequest)WebRequest.Create(uriString);
                    request.Headers.Add("Accept-Encoding", "gzip, deflate");
                    if (!string.IsNullOrEmpty(authorization))
                    {
                        request.Headers.Add("Authorization", authorization);
                    }
                    if (cds)
                    {
                        request.Headers.Add("Prefer", "return=representation");
                    }

                    if (header != null)
                    {
                        foreach(var item in header)
                        {
                            request.Headers.Add(item.Key, item.Value);
                        }
                    }

                    if (!string.IsNullOrEmpty(refer))
                    {
                        request.Headers.Add("Referer", refer);
                    }
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    request.Headers.Add("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3");
                    request.UserAgent = "Mozilla/5.0 (Windows NT 5.2; rv:12.0) Gecko/20100101 Firefox/12.0";

                    request.KeepAlive = true;
                    request.Timeout = 20000;
                    request.Method = "GET";
                    request.AllowAutoRedirect = true;

                    webresponse = (System.Net.HttpWebResponse)(await request.GetResponseAsync());


                    if (webresponse.ContentEncoding?.ToLower()?.Contains("gzip")??false)
                    {
                        Stream streamReceive = webresponse.GetResponseStream();

                        //gzip格式
                        streamReceive = new GZipStream(streamReceive, CompressionMode.Decompress);

                        using (StreamReader reader = new StreamReader(streamReceive, encoding))
                        {
                            result = reader.ReadToEnd();
                        }
                        streamReceive.Close();
                        streamReceive.Dispose();
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(webresponse.GetResponseStream(), encoding))
                        {
                            result = reader.ReadToEnd();
                        }
                    }

                }
                catch (WebException )
                {
                    throw ;
                }
                finally
                {
                    if (request != null)
                        request.Abort();
                    if (webresponse != null)
                    {
                        webresponse.Close();
                        webresponse.Dispose();
                    }
                }
            }
            return result;
        }

        public static async Task<string> GetUrlContentAsync2(string url)
        {

            //            var httpClientHandler = new HttpClientHandler();

            //#if DEBUG
            //            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            //#endif

            //https://dev.to/tswiftma/switching-from-httpclienthandler-to-socketshttphandler-17h3
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            var sslOptions = new SslClientAuthenticationOptions
            {
                // Leave certs unvalidated for debugging
                RemoteCertificateValidationCallback = delegate { return true; },
            };

            var handler = new SocketsHttpHandler()
            {
                SslOptions = sslOptions,
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
            };

            using (HttpClient client = new HttpClient(handler))
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get,
                };
                var response = await client.SendAsync(request);


                return await response.Content.ReadAsStringAsync();

            }
        }
        public static async Task<string> GetDynamic365ContentAsync(string uriString, Encoding encoding, string authorization = "")
        {
            string result = string.Empty;
            HttpWebRequest request = null;
            HttpWebResponse webresponse = null;
            if (!string.IsNullOrEmpty(uriString))
            {
                try
                {
                    request = (HttpWebRequest)WebRequest.Create(uriString);
                    request.Headers.Add("Accept-Encoding", "gzip, deflate");
                    if (!string.IsNullOrEmpty(authorization))
                    {
                        request.Headers.Add("Authorization", authorization);
                    }

                    request.Headers.Add("Prefer", @"odata.include-annotations="" * """);
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    request.Headers.Add("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3");
                    request.UserAgent = "Mozilla/5.0 (Windows NT 5.2; rv:12.0) Gecko/20100101 Firefox/12.0";

                    request.KeepAlive = true;
                    request.Timeout = 20000;
                    request.Method = "GET";
                    request.AllowAutoRedirect = true;

                    webresponse = (System.Net.HttpWebResponse)(await request.GetResponseAsync());


                    if (webresponse.ContentEncoding?.ToLower()?.Contains("gzip") ?? false)
                    {
                        Stream streamReceive = webresponse.GetResponseStream();

                        //gzip格式
                        streamReceive = new GZipStream(streamReceive, CompressionMode.Decompress);

                        using (StreamReader reader = new StreamReader(streamReceive, encoding))
                        {
                            result = reader.ReadToEnd();
                        }
                        streamReceive.Close();
                        streamReceive.Dispose();
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(webresponse.GetResponseStream(), encoding))
                        {
                            result = reader.ReadToEnd();
                        }
                    }

                }
                catch (WebException )
                {
                    throw ;
                }
                finally
                {
                    if (request != null)
                        request.Abort();
                    if (webresponse != null)
                    {
                        webresponse.Close();
                        webresponse.Dispose();
                    }
                }
            }
            return result;
        }

        public static async Task<string> PostDataToUrl(string url, string postData = "", string authorization = "")
        {
            //创建httpWebRequest对象
            WebRequest webRequest = System.Net.WebRequest.Create(url);
            HttpWebRequest httpRequest = webRequest as System.Net.HttpWebRequest;
            httpRequest.Method = "POST";
            if (httpRequest == null)
            {
                throw new Exception(string.Format("Invalid url string: {0}", url));
            }
            string result;

            Stream requestStream = null;
            Stream responseStream = null;
            try
            {

                var sp = httpRequest.ServicePoint;
                var prop = sp.GetType().GetProperty("HttpBehaviour",
                                        BindingFlags.Instance | BindingFlags.NonPublic);
                //httpRequest.Headers.Set("Accept-Language", "zh-CN,zh;q=0.9");
                httpRequest.Headers.Set("Accept-Encoding", "gzip, deflate");


                if (!string.IsNullOrEmpty(authorization))
                {
                    httpRequest.PreAuthenticate = true;
                    httpRequest.Headers.Add("Authorization", authorization);
                }

                httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
                httpRequest.Accept = "*/*";

                httpRequest.ContentType = "application/json";

                httpRequest.Timeout = 20000;

                var data = Encoding.ASCII.GetBytes(postData);
                httpRequest.ContentLength = data.Length;
                using (var stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }



                var webresponse = (System.Net.HttpWebResponse)await httpRequest.GetResponseAsync();
                if (webresponse?.ContentEncoding?.ToLower()?.Contains("gzip") ?? false)
                {
                    Stream streamReceive = webresponse.GetResponseStream();

                    //gzip格式
                    streamReceive = new GZipStream(streamReceive, CompressionMode.Decompress);

                    using (StreamReader reader = new StreamReader(streamReceive, Encoding.UTF8))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                else
                {
                    using (StreamReader reader = new StreamReader(webresponse.GetResponseStream(), Encoding.UTF8))
                    {
                        result = reader.ReadToEnd();
                    }
                }


            }
            catch (Exception )
            {
                throw ;
            }
            finally
            {
                if (requestStream != null)
                    requestStream.Close();

                if (responseStream != null)
                    responseStream.Close();
            }
            return result;
        }

        public static async Task<string> PostDataToUrlWithText(string url, string postData = "", string authorization = "")
        {
            //创建httpWebRequest对象
            WebRequest webRequest = System.Net.WebRequest.Create(url);
            HttpWebRequest httpRequest = webRequest as System.Net.HttpWebRequest;
            httpRequest.Method = "POST";
            if (httpRequest == null)
            {
                throw new Exception(string.Format("Invalid url string: {0}", url));
            }
            string result;

            Stream requestStream = null;
            Stream responseStream = null;
            try
            {

                var sp = httpRequest.ServicePoint;
                var prop = sp.GetType().GetProperty("HttpBehaviour",
                                        BindingFlags.Instance | BindingFlags.NonPublic);
                //httpRequest.Headers.Set("Accept-Language", "zh-CN,zh;q=0.9");
                httpRequest.Headers.Set("Accept-Encoding", "gzip, deflate");


                if (!string.IsNullOrEmpty(authorization))
                {
                    httpRequest.PreAuthenticate = true;
                    httpRequest.Headers.Add("Authorization", authorization);
                }

                httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
                httpRequest.Accept = "*/*";

                httpRequest.ContentType = "application/xhtml+xml";

                httpRequest.Timeout = 20000;

                var data = Encoding.ASCII.GetBytes(postData);
                httpRequest.ContentLength = data.Length;
                using (var stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }



                var webresponse = (System.Net.HttpWebResponse)await httpRequest.GetResponseAsync();
                if (webresponse?.ContentEncoding?.ToLower()?.Contains("gzip") ?? false)
                {
                    Stream streamReceive = webresponse.GetResponseStream();

                    //gzip格式
                    streamReceive = new GZipStream(streamReceive, CompressionMode.Decompress);

                    using (StreamReader reader = new StreamReader(streamReceive, Encoding.UTF8))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                else
                {
                    using (StreamReader reader = new StreamReader(webresponse.GetResponseStream(), Encoding.UTF8))
                    {
                        result = reader.ReadToEnd();
                    }
                }


            }
            catch (Exception )
            {
                throw ;
            }
            finally
            {
                if (requestStream != null)
                    requestStream.Close();

                if (responseStream != null)
                    responseStream.Close();
            }
            return result;
        }

        public static async Task<string> PostFormData(string url, Dictionary<string, string> postParameters)
        {
            //创建httpWebRequest对象
            WebRequest webRequest = System.Net.WebRequest.Create(url);
            HttpWebRequest httpRequest = webRequest as System.Net.HttpWebRequest;
            httpRequest.Method = "POST";
            if (httpRequest == null)
            {
                throw new Exception(string.Format("Invalid url string: {0}", url));
            }
            string result;

            Stream requestStream = null;
            Stream responseStream = null;
            try
            {

                string postData = "";

                foreach (string key in postParameters.Keys)
                {
                    postData += HttpUtility.UrlEncode(key) + "="
                          + HttpUtility.UrlEncode(postParameters[key]) + "&";
                }

                var sp = httpRequest.ServicePoint;
                var prop = sp.GetType().GetProperty("HttpBehaviour",
                                        BindingFlags.Instance | BindingFlags.NonPublic);
                httpRequest.Headers.Set("Accept-Encoding", "gzip, deflate");


                httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
                httpRequest.Accept = "*/*";


                byte[] data = Encoding.ASCII.GetBytes(postData);

                httpRequest.ContentType = "application/x-www-form-urlencoded";

                httpRequest.Headers.Add("Origin", "http://localhost");
                httpRequest.ContentLength = data.Length;

                httpRequest.Timeout = 20000;

                using (var stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }


                var webresponse = (System.Net.HttpWebResponse)await httpRequest.GetResponseAsync();
                if (webresponse.ContentEncoding?.ToLower()?.Contains("gzip") ?? false)
                {
                    Stream streamReceive = webresponse.GetResponseStream();

                    //gzip格式
                    streamReceive = new GZipStream(streamReceive, CompressionMode.Decompress);

                    using (StreamReader reader = new StreamReader(streamReceive, Encoding.UTF8))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                else
                {
                    using (StreamReader reader = new StreamReader(webresponse.GetResponseStream(), Encoding.UTF8))
                    {
                        result = reader.ReadToEnd();
                    }
                }


            }
            catch (Exception )
            {
                throw ;
            }
            finally
            {
                if (requestStream != null)
                    requestStream.Close();

                if (responseStream != null)
                    responseStream.Close();
            }
            return result;
        }

        public static async Task<string> Patch(string url, string authorization, string postData = "")
        {

            //创建httpWebRequest对象
            WebRequest webRequest = System.Net.WebRequest.Create(url);
            HttpWebRequest httpRequest = webRequest as System.Net.HttpWebRequest;
            httpRequest.Method = "PATCH";
            if (httpRequest == null)
            {
                throw new Exception(string.Format("Invalid url string: {0}", url));
            }
            string result;

            Stream requestStream = null;
            Stream responseStream = null;
            try
            {

                var sp = httpRequest.ServicePoint;
                var prop = sp.GetType().GetProperty("HttpBehaviour",
                                        BindingFlags.Instance | BindingFlags.NonPublic);
                httpRequest.Headers.Set("Accept-Language", "zh-CN,zh;q=0.9");
                httpRequest.Headers.Set("Accept-Encoding", "gzip, deflate");

                httpRequest.PreAuthenticate = true;
                httpRequest.Headers.Add("Authorization", authorization);

                httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
                httpRequest.Accept = "*/*";

                httpRequest.ContentType = "application/json";

                httpRequest.Timeout = 20000;

                var data = Encoding.ASCII.GetBytes(postData);
                httpRequest.ContentLength = data.Length;
                using (var stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var webresponse = (System.Net.HttpWebResponse)await httpRequest.GetResponseAsync();

                var statuCode = webresponse.StatusCode;
                result = statuCode.ToString();
            }
            catch (Exception )
            {
                throw;
            }
            finally
            {
                if (requestStream != null)
                    requestStream.Close();

                if (responseStream != null)
                    responseStream.Close();
            }
            return result;

        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受
        }

        //public static bool Download(string urlString, string fileName)
        //{
        //    try
        //    {
        //        using (WebClient client = new WebClient())
        //        {
        //            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;
        //            client.DownloadFile(urlString, fileName);
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //}

        public async static Task<bool> DownloadAsync(string urlString, string fileName)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                //if (!urlString.StartsWith("http://"))
                //    urlString = "http://" + urlString;
                #region 处理URL
                Uri uri = new Uri(urlString);

                //string paq = uri.PathAndQuery; // need to access PathAndQuery
                ////得到私有字段
                //FieldInfo flagsFieldInfo = typeof(Uri).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);
                ////取值
                //ulong flags = (ulong)flagsFieldInfo.GetValue(uri);
                //// Flags.PathNotCanonical|Flags.QueryNotCanonical
                //flags &= ~((ulong)0x30);
                ////设置值
                //flagsFieldInfo.SetValue(uri, flags);
                #endregion

                #region 请求数据

                if (urlString.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback
                        = new RemoteCertificateValidationCallback(CheckValidationResult);
                    request = WebRequest.Create(urlString) as HttpWebRequest;
                    request.ProtocolVersion = HttpVersion.Version10;
                }
                else { request = WebRequest.Create(urlString) as HttpWebRequest; }


                //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                request.AllowAutoRedirect = false;
                request.Accept = "*/*";

                request.Headers.Add("Accept-Language", "zh-cn");
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                request.KeepAlive = true;
                request.Timeout = 20000;
                request.Method = "GET";

                ////获取服务器回应
                response = (HttpWebResponse)await request.GetResponseAsync();

                #endregion

                //检查服务器是否支持断点续传
                bool supportrange = false;
                if (response != null)
                    supportrange = (response.Headers[HttpResponseHeader.AcceptRanges] == "bytes");

                byte[] buffer = new byte[1024];

                Stream deflate = null; //Deflate/gzip 解压流
                BufferedStream bs; //缓冲流

                //确定缓冲长度//?

                //获取下载流
                using (Stream st = response.GetResponseStream())
                {
                    //设置gzip/deflate解压缩
                    if (response.ContentEncoding == "gzip")
                    {
                        deflate = new GZipStream(st, CompressionMode.Decompress);
                    }
                    else if (response.ContentEncoding == "deflate")
                    {
                        deflate = new DeflateStream(st, CompressionMode.Decompress);
                    }
                    else
                    {
                        deflate = st;
                    }
                    //Loger.Info(fileName);

                    //打开文件流
                    using (Stream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read, 8))
                    {
                        //使用缓冲流(好像有错误)
                        bs = new BufferedStream(fs, 8 * 1024);//8KB

                        try
                        {
                            //读取第一块数据
                            Int32 osize = deflate.Read(buffer, 0, buffer.Length);
                            //开始循环
                            while (osize > 0)
                            {
                                #region 判断是否取消下载
                                //如果用户终止则返回false

                                #endregion

                                //写文件(缓存)
                                bs.Write(buffer, 0, osize);
                                osize = deflate.Read(buffer, 0, buffer.Length);

                            } //end while
                        } //end bufferedstream
                        catch (Exception )
                        {
                            //Loger.Error(fileName);
                            throw ;
                        }
                        finally
                        {
                            bs.Close();
                            if (deflate != null)
                            {
                                deflate.Close();
                            }
                        }
                    }
                }

                return true;

            }
            catch (Exception e)
            {
                //Loger.Error(e);

                return false;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (response != null)
                {
                    response.Close();
                    response.Dispose();
                }
            }
        }

        public async static Task DownloadFileAsync(string url,string filePath, IProgress<float> progress = null, CancellationToken cancellationToken = default)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
               await httpClient.DownloadAsync(url, fs, progress, cancellationToken);
                fs.Flush();
            }
        }


        public static string GetHtmlContentWithCookie(string url, CookieContainer ccs, int timeout)
        {
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            string result = "";
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);

                var sp = request.ServicePoint;
                var prop = sp.GetType().GetProperty("HttpBehaviour",
                                        BindingFlags.Instance | BindingFlags.NonPublic);
                prop.SetValue(sp, (byte)0, null);


                request.ServicePoint.ConnectionLimit = 10;
                request.Method = "GET";
                request.Timeout = timeout;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
                request.KeepAlive = true;

                //request.ProtocolVersion = HttpVersion.Version11;
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                //request.Referer = "http://cpquery.sipo.gov.cn";
                //request.Host = "cpquery.sipo.gov.cn";
                request.CookieContainer = ccs; //暂存到新实例
                request.Headers.Set("Upgrade-Insecure-Requests", "1");
                request.Headers.Set("Accept-Language", "zh-CN,zh;q=0.9");
                request.Headers.Set("Accept-Encoding", "gzip, deflate");
                request.ContentType = "text/html; charset=utf-8";

                //request.AllowAutoRedirect = true;

                response = (HttpWebResponse)request.GetResponse();

                Encoding encoding = Encoding.GetEncoding("UTF-8");

                request.KeepAlive = true;

                var cookies = request.CookieContainer; //保存cookies

                var webresponse = (System.Net.HttpWebResponse)request.GetResponse();

                if (webresponse.ContentEncoding.ToLower().Contains("gzip"))
                {
                    Stream streamReceive = webresponse.GetResponseStream();

                    //gzip格式
                    streamReceive = new GZipStream(streamReceive, CompressionMode.Decompress);
                    using (StreamReader reader = new StreamReader(streamReceive))
                    {
                        result = reader.ReadToEnd();
                        Console.WriteLine(response.StatusCode);
                        return result;
                    }
                }
                else
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                    {
                        result = reader.ReadToEnd();
                        Console.WriteLine(response.StatusCode);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                    request.Abort();
            }
            return result;
        }

    }
}
