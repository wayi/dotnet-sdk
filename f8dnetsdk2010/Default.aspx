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
</head>
<body>
    <form id="form1" runat="server">
    <div style="background-color: #FFFF00">
    

        <h2>Fun Buddy .NET SDK範例</h2>
        <br />
        <p>
            範例 <a href="https://github.com/wayi/dotnet-sdk" title="F8D .NET SDK">https://github.com/wayi/dotnet-sdk</a>。 線上文件 
            <a href="http://developers.fun.wayi.com.tw/component/asp" 
                title="ASP.NET Website">
            http://developers.fun.wayi.com.tw/component/asp</a>。 
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
    <br />
    </form>
    <br />
</body>
</html>
