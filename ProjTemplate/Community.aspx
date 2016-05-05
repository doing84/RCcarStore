<%@ Page Language="C#" MasterPageFile="~/BalloonShop.master" AutoEventWireup="true" CodeFile="Community.aspx.cs" Inherits="_Default" Title="Welcome to RC-Car Shop!" %>

<%@ Register Src="UserControls/ProductsList.ascx" TagName="ProductsList" TagPrefix="uc1" %>
<asp:Content ID="content" ContentPlaceHolderID="contentPlaceHolder" Runat="server">

    <br />
    <asp:Label ID="Label2" runat="server" Text="Our Toy Car Community is here" Font-Bold="True" Font-Size="X-Large" ForeColor="#660033"></asp:Label>

       <html>
	<head>
        <div>
		<link type="text/css" href="pikachoose-96/styles/simple.css" rel="stylesheet" />
		<script type="text/javascript" src="/JS/jquery.js"></script>
        <script type="text/javascript" src="/pikachoose-96/lib/jquery.jcarousel.min.js"></script>
		<script type="text/javascript" src="/pikachoose-96/lib/jquery.pikachoose.min.js"></script>
		<script type="text/javascript" src="/pikachoose-96/lib/jquery.touchwipe.min.js"></script>
		<script language="javascript">
			$(document).ready(function (){
					$("#pikame").PikaChoose();
				});
		</script>
            </div>
		
</head>
<body>

<div class="pikachoose">
    Join us now

</div>

</body>
</html>
</br></br></br></br>



<section class="banners banners_home">

</section>
 



    </asp:Content>

