using System;
using System.Collections.Generic;
using System.Web;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace f8dnetsdk2010
{
    public class FunBuddy
    {
        //const
        string API_SDK = "F8D .NET SDK";
        string API_VERSION = "1.1.2";
        string URL_API = "https://api.fun.wayi.com.tw/";

        //attribute
        int appId;
        string appSecret;
        string redirectUri;
        JObject session;

        //error code
        const int GET_ENV_SERVER_NOT_RESPONSE = 1000;
        const int INIT_APPID_IS_NOT_SET = 2000;
        const int INIT_APPID_IS_NOT_A_NUMBER = 2001;
        const int INIT_APP_SECRET_INVALID = 2002;

        const int GET_ENV_INVALID_JSON_FORMAT = 2101;
        const int GET_ENV_RESPONSE_INVALID_FORMAT = 2102;

        const int API_INVALID_JSON_FORMAT = 2201;
        protected NameValueCollection config;
        public FunBuddy(NameValueCollection config)
        {
            this.config = config;
            log("<<<<<<< F8D SDK start");
            this.setConfig(config);

           
            URL_API = getAppEnv();
        }

        private void setConfig(NameValueCollection config)
        {
            
            //precondition
            if (config["appid"] == null)
            {
                NameValueCollection code = new NameValueCollection();
                code.Add("error_code", INIT_APPID_IS_NOT_SET.ToString());
                throw new ApiException(INIT_APPID_IS_NOT_SET, "appid is not set.");
            }
            int appid;
            if (!int.TryParse(config["appId"], out appid))
            {
                throw new ApiException(INIT_APPID_IS_NOT_A_NUMBER, "appid is not a number.");
            }

            if (config["secret"] == null)
            {
                throw new ApiException(INIT_APP_SECRET_INVALID, "app secret is invalid.");
            }

            //1.setup
            this.AppId = appid;
            this.AppSecret = config["secret"];
            this.redirectUri = config["redirect_uri"];
        }

        private string getAppEnv()
        {
            this.log("[getAppEnv]");
            string url = string.Format("{0}dispatcher/{1}", this.URL_API, this.AppId);
            NameValueCollection param = new NameValueCollection();
            param.Add("sdk", this.API_SDK);
            param.Add("version", this.API_VERSION);

            JObject app_env = this.makeRequest(url, param, "GET");
            if (app_env == null)
            {
                throw new ApiException(GET_ENV_INVALID_JSON_FORMAT, "Invalid json format.");
            }

            if (app_env["error"] != null)
            {
                throw new ApiException(app_env);
            }

            if ((app_env["api"] == null))
            {
                throw new ApiException(GET_ENV_RESPONSE_INVALID_FORMAT, "server reponses invalid format.");
            }
         
            this.log(String.Format("env: {0} url: {1}" , app_env["env"].ToString(), app_env["api"].ToString()));
            return app_env["api"].ToString();
        }

        public JObject getSession()
        {
            this.log("[getSession]");

            if (this.session != null)
                return this.session;

            string session = System.Web.HttpContext.Current.Request["session"];
            string code = System.Web.HttpContext.Current.Request["code"];
            if (!(string.IsNullOrEmpty(session)))
            {
                //get token from session
                this.log("get Session from url");
                this.session = JObject.Parse(session);
            }
            else if (!(string.IsNullOrEmpty(code)))
            {
                this.log("get code: " + code);
                //get code
                NameValueCollection param = new NameValueCollection();
                param.Add("code", code);
                param.Add("grant_type", "authorization_code");
                param.Add("redirect_uri", this.redirectUri);
                param.Add("client_id", this.appId.ToString());
                param.Add("client_secret", this.appSecret);

                JObject result = this.makeRequest(this.URL_API + "oauth/token", param, "GET");
                if ((int)result["error"] > 0)
                {
                    //HttpContext.Current.Response.Write(result.ToString());
                    throw new ApiException(result);                    
                }
                this.log("get session: " + result.ToString());
                this.session = result;
            }
            log(">>>>>>> ready to access f8d api");
            return this.session;
        }

        public string getAccessToken(){
            if( this.session == null)
                return "";
            if(this.session["access_token"] == null)
                return "";
            return this.session["access_token"].ToString();
        }

        #region "setter & getter"
        public int AppId
        {
            set { this.appId = value; }
            get { return this.appId; }
        }
        public string AppSecret
        {
            set { this.appSecret = value; }
            get { return this.appSecret; }
        }
        #endregion

        public string getLoginUrl()
        {
            //0.validate
            string test = this.config["redirect_uri"];
            string redirect_uri = this.redirectUri;// (string.IsNullOrEmpty(this.config["redirect_uri"])) ? this.config["redirect_uri"] : "";
            string scope = (string.IsNullOrEmpty(this.config["scope"])) ? "" : this.config["scope"];

            NameValueCollection param = new NameValueCollection();
            param.Add("response_type", "code");
            param.Add("redirect_uri", redirect_uri);
            param.Add("client_id", this.appId.ToString());
            param.Add("scope", scope);
            return this.URL_API + "oauth/authorize?" + ConstructQueryString(param);
        }

        public String ConstructQueryString(NameValueCollection parameters)
        {
            List<String> items = new List<String>();

            foreach (String name in parameters)
            {
                items.Add(String.Concat(name, "=", System.Web.HttpUtility.UrlEncode(parameters[name])));
            }

            return String.Join("&", items.ToArray());
        }

        void log(string message)
        {
            if (config["log_path"] == null)
                return;
            message = String.Format("{0:yyyy-M-d H:m:s} {1}", DateTime.Now, message);

            TextWriter tw = new StreamWriter(config["log_path"], true);
            tw.WriteLine(message);
            tw.Close();
        }

        public JObject api(string path, string method, NameValueCollection param)
        {
            method = (string.IsNullOrEmpty(method)) ? "GET" : method;

            if (string.IsNullOrEmpty(param["access_token"]))
            {
                param["access_token"] = this.session["access_token"].ToString();
            }
           
            NameValueCollection param2 = new NameValueCollection();
            foreach (String name in param)
            {
                param2[name] = JsonConvert.SerializeObject(param[name]).Replace("\"", "");
            }

            JObject result = this.makeRequest(this.URL_API + path, param2, method);
            if (result["error"] != null)
            {
                throw new ApiException((JObject)result);
            }

            return result;
        }
      
        public JObject makeRequest(string url, NameValueCollection param, string method)
        {
            string querystring = ConstructQueryString(param);
            log(string.Format("connect to: {0} with parameters: {1}", url, querystring));

            string result = GetRequest(url, querystring);
            log(string.Format("reply: {0}",result));

            JObject test = null;
            try
            {
                test = JObject.Parse(result);
            }
            catch (Exception ex)
            {
                throw new ApiException(API_INVALID_JSON_FORMAT, result);
            }
            return test;
        }

        public string GetRequest(string Url, string Param)
        {
            Url = Url + "?" + Param;

            try
            {
                HttpWebRequest Request = (HttpWebRequest)HttpWebRequest.Create(Url);
                Request.Method = "GET";
                Request.ContentType = "application/x-www-form-urlencoded";
                WebResponse myResponse = Request.GetResponse();
                StreamReader RecStream = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string ResponseHTML = RecStream.ReadToEnd();
                RecStream.Close();
                myResponse.Close();
                return ResponseHTML;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string PostRequest(string Url, string Param)
        {
            byte[] postData = Encoding.ASCII.GetBytes(Param);
            try
            {
                HttpWebRequest Request = WebRequest.Create(Url) as HttpWebRequest;
                Request.Method = "POST";
                Request.KeepAlive = false;  //是否保持連線  
                Request.ContentType = "application/x-www-form-urlencoded;charset=UTF8";
                //request.CookieContainer = cookieContainer;   //server 才知道你是誰 
                Request.ContentLength = postData.Length;

                System.IO.Stream outputStream = Request.GetRequestStream();
                outputStream.Write(postData, 0, postData.Length);
                outputStream.Close();

                HttpWebResponse response = Request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }              
       
    }
    public class ApiException : Exception, System.Runtime.Serialization.ISerializable
    {
        public int code;
        public string message;
        public ApiException(JObject error)
        {
            this.code = (int)(error["error"]["code"]);
            this.message = (string)error["error"]["message"];
        }
        public ApiException(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
    }
}