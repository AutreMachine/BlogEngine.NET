<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportBackupData.aspx.cs" Inherits="BlogEngine.NET.admin.ImportBackupData" %>

<html ng-app="blogAdmin">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, user-scalable=no">
    <title></title>
    <link href="https://fonts.googleapis.com/css?family=Roboto:400,500,700" rel="stylesheet">
    <link href="/admin/themes/standard/css/styles.css?ver=4" rel="stylesheet">
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=no" />
</head>
<body>
    <h1>Import Backup Data</h1>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <p>
            <p style="background-color: red; color: white">Warning : no undo !</p>
            <p>
                <input type="file" name="FileUpload" value="Choose the archive to import" />

                <asp:Button ID="Button1" Text="Upload" runat="server" OnClick="Upload" />
            </p>
            <br />
            <p>
                <asp:Label ID="lblMessage" Text="Data imported successfully." runat="server" ForeColor="Green" Visible="false" />
            </p>
        </p>

    </form>
</body>
</html>
