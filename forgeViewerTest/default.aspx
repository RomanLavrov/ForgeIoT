<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="forgeViewerTest._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, minimum-scale=1.0, initial-scale=1, user-scalable=no" />
    <meta charset="utf-8" />
    <link rel="stylesheet" href="https://developer.api.autodesk.com/modelderivative/v2/viewers/style.min.css" type="text/css" />
    <title>Forge Viewer</title>
    <script src="https://developer.api.autodesk.com/modelderivative/v2/viewers/three.min.js"></script>
    <script src="https://developer.api.autodesk.com/modelderivative/v2/viewers/viewer3D.min.js"></script>

    <script src="/Scripts/ForgeViewer.js"></script>   
    <script src="/Scripts/ForgeApplication.js"></script>
    <script src="/Scripts/Server.js"></script>
    <script src="/Scripts/SmokeDetectorsExtension.js"></script>

    <%--Bootstrap--%>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="Header" style="height: 70px; background-color: dimgray; color: white;">
                <div style="display: table-cell; margin-left: 40px;">
                    <h1 style="margin-left: 40px;">Forge Viewer</h1>
                </div>

                <div style="display: table-cell; vertical-align: middle;">
                    <asp:FileUpload ID="FileUpload" runat="server" Style="margin-left: 40px; float: left;" />
                </div>
                <div style="display: table-cell; vertical-align: middle;">
                    <asp:Button ID="Upload" runat="server" OnClick="Upload_Click" Text="Upload and translate" Style="margin-left: 40px; color: black;" />
                </div>
            </div>
        </div>
    </form>


    <div id="MyViewerDiv" class="viewer" style="width: auto; height: 800px; background-color: red; position: relative; margin: 0 auto"></div>

    <%--<button id="MyLockButton" style="z-index: 10">Lock it!</button>
    <button id="MyUnlockButton" style="z-index: 10">Unlock it!</button>--%>


</body>
</html>
