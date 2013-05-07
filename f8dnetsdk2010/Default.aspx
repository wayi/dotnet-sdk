<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="f8dnetsdk2010._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">


hgroup.title h1 {
    display: inline;
}

h1 {
    font-size: 2em;
}

h1 {
    color: #000;
    margin-bottom: 0;
    padding-bottom: 0;
}

hgroup.title h2 {
    font-weight: normal;
    margin-left: 3px;
}

hgroup.title h2 {
    display: inline;
}

h2 {
    font-size: 1.75em;
}

h2 {
    color: #000;
    margin-bottom: 0;
    padding-bottom: 0;
}

    a:link {
        color: #333;
    }

    a {
    color: #333;
    outline: none;
    padding-left: 3px;
    padding-right: 3px;
    text-decoration: underline;
}

    </style>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
	<script type="text/javascript" src="//api.funbuddy.com/assets/jqplugin/f8d.js?v=20121017"></script>
    
</head>
<body>
    
<h1>Wayi金流 儲值遊戲幣範例</h1>
直接從玩家帳號扣除WGS點數，進而購買商品。 
<h4>1.<a target="_blank" href="http://developers.funbuddy.com/guides/currency_sdk#商品模式(廠商實作介面)">商品模式</a></h4>
<table border=1>
	<caption>儲值金額</caption>
	<th>遊戲幣</th><th>點數</th><th></th>
	<tr>
		<td style="text-align:right;">1 <img src="gold.gif" /></td>
		<td >2 WGS points (測WGS)</td>
		<td>
<a onclick="place_order('ITEM0001'); return false;" type="button" name="fun_share" class="fun_share_button">Pay with F8D</a>
	</tr>
	<tr>
		<td style="text-align:right;">1000 <img src="gold.gif" /></td>
		<td>1000 WGS points (測餘額不足)</td>
		<td>
<a onclick="place_order('ITEM0002'); return false;" type="button" name="fun_share" class="fun_share_button">Pay with F8D</a>
	</tr>

</table>
<link rel="stylesheet" type="text/css" href="//api.funbuddy.com/assets/socialplugin/css/fun_share.css">
  
<br/>
<h4>2.<a target="_blank" href="http://developers.funbuddy.com/guides/currency_sdk#儲值模式(廠商實作介面)">儲值模式</a></h4>
儲值模式提供直接儲值遊戲幣功能。User選擇儲值管道後，直接選擇兌換額度，並全部轉成遊戲幣，即不依附商品。<br/>
<input value="儲值模式" type="button" onclick="javascript:gamecash_mode();">
<hr>
Reply
<div id="output" style="border:1px solid;background-color:#FFFFCC;height:300px;"></div>
    <form id="form1" runat="server">
    <div style="background-color: #FFFF00">    
    
        <h2>Fun Buddy .NET SDK範例</h2>
        <br />
        <p>
            範例 <a href="https://github.com/wayi/dotnet-sdk" title="F8D .NET SDK">https://github.com/wayi/dotnet-sdk</a>。 線上文件 
            <a href="http://developers.funbuddy.com/component/asp" 
                title="ASP.NET Website">
            http://developers.funbuddy.com/component/asp</a>。 
        </p>

    </div>
    <asp:Panel ID="Panel2" runat="server" Visible="False">
        Error Code:
        <asp:Label ID="lblCode" runat="server" ForeColor="Red" Text="[Code]"></asp:Label>
        <br />
        Error Message:
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text="[Message]"></asp:Label>
    </asp:Panel>
    <asp:Panel ID="Panel1" runat="server">
        <br />
        Hi,
        <asp:Label ID="lblName" runat="server" ForeColor="Red" Text="[name]"></asp:Label>
        &nbsp;<br /> Your ID is
        <asp:Label ID="lblUid" runat="server" ForeColor="Red" Text="[uid]"></asp:Label>
        <br />
        Your Account is
        <asp:Label ID="lblAccount" runat="server" ForeColor="Red" Text="[account]"></asp:Label>
        <br />
        Your Access Token is
        <asp:Label ID="lblAccessToken" runat="server" ForeColor="Red" 
            Text="[access token]"></asp:Label>
        <br />
        The following is your friends:<br /> <asp:GridView ID="GridView1" runat="server" 
            BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
            CellPadding="4" EnableModelValidation="True" ForeColor="Black" 
            GridLines="Vertical">
            <AlternatingRowStyle BackColor="White" />
            <FooterStyle BackColor="#CCCC99" />
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <RowStyle BackColor="#F7F7DE" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
    </asp:Panel>
    <br />
    </form>
<script type="text/javascript">
    $(function () {
        $('body').F8D.init({ appid: "<%= Get_Appid() %>", access_token: "<%= Get_AccessToken() %>" });

        //resize
        $(this).fun.iframe.setAutoResize();
    });


    //place an order
    function place_order(itemid) {
        // Only send param data for sample. These parameters should be set
        // in the callback.
        var order_info = {
            itemid: itemid,
            test: 'testdata1'
        };

        // calling the API ...
        var obj = {
            method: 'payment_product_mode',
            order_info: order_info
        };


        $('body').F8D.ui(obj, callback);
    }

    function gamecash_mode() {
        var order_info = {
            test: 'testdata2'
        };

        // calling the API ...
        var obj = {
            method: 'payment_gamecash_mode',
            order_info: order_info
        };


        $('body').F8D.ui(obj, callback);

    }

    function callback(data) {
        if (data['orderid']) {
            writeback("Transaction Completed! </br></br>"
			+ "Data returned from F8D: </br>"
			+ "<b>Order ID: </b>" + data['orderid'] + "</br>"
			+ "<b>Status: </b>" + data['status'] + "</br>"
			+ "<b>All Info: </b>" + JSON.stringify(data));
        } else if (data.error_code) {
            writeback("Transaction Failed! </br></br>"
			+ "Error message returned from F8D:</br>"
			+ "<b>Error code: </b>" + data['error_code'] + '<br/>'
			+ "<b>Error Message: </b>" + data['error_message'] + '<br/>'
			+ "<b>All Info: </b>" + JSON.stringify(data));
        } else {
            writeback("Transaction failed! </br>"
			+ JSON.stringify(data.error_message)
		);
        }
    }

    function writeback(str) {
        document.getElementById('output').innerHTML = str;
    }

</script>
    <br />
    </form>
    <br />
</body>
</html>
