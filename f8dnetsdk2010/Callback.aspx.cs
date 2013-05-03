// Copyright 2013-Wayi. All Rights Reserved.
/**
 *	there are two methods for payment
 *	1.buy item
 *		related func name are
 *			- payments_get_itemds
 *			- payments_completed
 *	2.exchange gamecash
 *		related func name are
 *			-payments_gamecash_completed
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace f8dnetsdk2010
{
    public partial class Callback : System.Web.UI.Page
    {
        //TO-DO
        //1.Enter your app information below
        String APP_SECRET = "d1496ee7308c409a27df5bcd6f4a482f";          //YOUR_APP_SECRET

        protected void Page_Load(object sender, EventArgs e)
        {
            //2.Prepare the return data array
            JObject data = new JObject();
            data.Add("content", new JObject());

            //3.Parse the signed_request to verify it's from f8d
            JObject request = Currency.parse_signed_request(System.Web.HttpContext.Current.Request["signed_request"], APP_SECRET);

            if (request == null) {
	            // Handle an unauthenticated request here
                Response.Write(Currency.make_error_report("unauthenticated"));
                return;
            }

            // Grab the payload
            JObject payload = JObject.Parse(request["credits"].ToString());

            // Retrieve all params passed in
            String func = System.Web.HttpContext.Current.Request["method"];

            if (func == "payments_completed") {

                // Grab the order status
	            String status = payload["status"].ToString();

	            // Write your apps logic here for validating and recording a
	            // purchase here.
	            Boolean success = true;
	            if(success){
		            // Generally you will want to move states from `placed` -> `settled`
		            // here, then grant the purchasing user's in-game item to them.
		            if (status == "placed") {
			            //give item here

			            //confirm transcation
			            String next_state = "settled";
			            data["content"]["status"] = next_state;
		            }

		            // Compose returning data array_change_key_case
		            String orderid = payload["orderid"].ToString();
		            data["content"]["orderid"] = orderid;
	            }else{
                    Response.Write(Currency.make_error_report("月維護", 501)); //you can defined your own error message
                    return;		
	            }

            } else if (func == "payments_get_items") {

	            // Per the credits api documentation, you should pass in an item
	            // reference and then query your internal DB for the proper
	            // information. Then set the item information here to be
	            // returned to facebook then shown to the user for confirmation.
                JObject items = new JObject();
                String itemid = payload["itemid"].ToString();
                switch (itemid)
                { 
                    case "ITEM0001":
                        items.Add("itemid", "ITEM0001");
                        items.Add("title", "1遊戲幣(測WGS)");
                        items.Add("price", "2");//wgs
                        items.Add("gamecash", "1");
                        items.Add("description", "2WGS = 1遊戲幣");
                        items.Add("image_url", "http://10.0.2.106/kevyu/projects/developer/currency/gold.gif");
                        break;
                    case "ITEM0002":
                        items.Add("itemid", "ITEM0002");
                        items.Add("title", "1000遊戲幣(測餘額不足)");
                        items.Add("price", "1000");//wgs
                        items.Add("gamecash", "1000");
                        items.Add("description", "1WGS = 1遊戲幣");
                        items.Add("image_url", "http://10.0.2.106/kevyu/projects/developer/currency/gold.gif");
                        break;
                    default:
                        Response.Write(Currency.make_error_report(String.Format("item[{0}] is not existed", itemid)));
                        return;
                }
                
	            // Put the associate array of item details in an array, and return in the
	            // 'content' portion of the callback payload.
                data["content"] = items;
            }else if (func == "payments_gamecash_completed") {

	            // Grab the order status
	            String status = payload["status"].ToString();

	            // Write your apps logic here for validating and recording a
	            // Generally you will want to move states from `placed` -> `settled`
	            if (status == "placed") {
		            //give gamecash here
		            String makeup = payload["makeup"].ToString();

                    //confirm transcation
		            String next_state = "settled";
		            data["content"]["status"] = next_state;
	            }

	            // Compose returning data array_change_key_case
	            //save
	            String orderid = payload["orderid"].ToString();
	            data["content"]["orderid"] = orderid;
            }else{
                Response.Write(Currency.make_error_report(String.Format("func {0} is not existed", func), 502));
                return;		
	        }

            // Required by api_fetch_response()
            data["method"] = func;

            // Send data back
            Response.Write(data.ToString());

        }
    }
}