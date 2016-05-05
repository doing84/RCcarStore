<%@ Page Language="C#" MasterPageFile="~/BalloonShop.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Title="Welcome to RC-Car Shop!" %>

<%@ Register Src="UserControls/ProductsList.ascx" TagName="ProductsList" TagPrefix="uc1" %>
<asp:Content ID="content" ContentPlaceHolderID="contentPlaceHolder" Runat="server">
    <span class="CatalogTitle">Welcome to RC Car Online Store! </span>
  <br />
  <br />
  <uc1:ProductsList ID="ProductsList1" runat="server" />

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:BalloonShopConnection %>" SelectCommand="SELECT [Name], [Price], [Thumbnail] FROM [Product] WHERE ([PromoFront] = @PromoFront)">
        <SelectParameters>
            <asp:Parameter Name="PromoFront" />
        </SelectParameters>
       
    </asp:SqlDataSource>

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
Our Store
	<ul id="pikame" >
		<li><img src="images/Toycarstore.jpg" /></a><span>We have amazing RC Cars.</span></li>
		<li><img src="images/store.jpg" /></a><span>Look! Choose! Enjoy!</span></li>
		<li><img src="images/store2.jpg" /></a><span>You Can Contect Me With <a href="http://www.centennialcollege.ca/">Centennialcollege.ca</a> for updates.</span></li>
			</ul>
</div>

</body>
</html>
<br>
</br>
<br></br>
<br></br>
<br></br>



<section class="banners banners_home">

</section>
 



    </asp:Content>

