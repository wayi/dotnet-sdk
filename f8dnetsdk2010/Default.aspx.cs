using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace f8dnetsdk2010
{
    public partial class _Default : System.Web.UI.Page
    {
 
        //TO-DO
        string APP_ID = "545";                                  //YOUR_APP_ID
        string APP_SECRET = "d1496ee7308c409a27df5bcd6f4a482f";          //YOUR_APP_SECRET
        string REDIRECT_URI = "http://localhost/f8d/Default.aspx"; //redirect to your login check after login F8D


        protected void Page_Load(object sender, EventArgs e)
        {
            //1.基本設定
            NameValueCollection config = new NameValueCollection();
            config.Add("appId", APP_ID);
            config.Add("secret", APP_SECRET);
            config.Add("redirect_uri", REDIRECT_URI);            
            //config.Add("log_path", "d:\\data.log");

            //2.實體化
            FunBuddy f8d = null;
            try
            {
                f8d = new FunBuddy(config);
            }
            catch (ApiException ex)
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
                lblCode.Text = ex.code.ToString();
                lblMessage.Text = ex.message;
                return;
            }

          
            //3.取得access token (在session裡)           
            JObject session = f8d.getSession();
            if (session == null)
            {
                Response.Redirect(f8d.getLoginUrl());
                return;
            }

            //4.取得帳號資料
            try
            {
                JObject me = f8d.api("v1/me/account", "GET", new NameValueCollection());
                lblAccount.Text = me["pid"].ToString();
                lblName.Text = me["username"].ToString();
                lblAccessToken.Text = f8d.getAccessToken();
                lblUid.Text = me["uid"].ToString();
            }
            catch (ApiException ex)
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
                lblCode.Text = ex.code.ToString();
                lblMessage.Text = ex.message;
            }
           
            //5.調用api(ex. 取得好友)
            NameValueCollection param = new NameValueCollection();
            param.Add("start","0");
            param.Add("count","10");
            JObject result = f8d.api("v1/me/friends", "GET", param);
            var q = from p in result.Properties()
                    select p;

            System.Data.DataTable data = new System.Data.DataTable();
            data.Columns.Add("uid");
            data.Columns.Add("username");
            foreach (JProperty friend in q)
            {
                System.Data.DataRow tr = data.NewRow();
                tr["uid"] = result[friend.Name]["uid"].ToString();
                tr["username"] = result[friend.Name]["username"].ToString();
                data.Rows.Add(tr);
            }
            GridView1.DataSource = data;
            GridView1.DataBind();
            //logoutUrl = $fun->getLogoutUrl();
        }
    }
}