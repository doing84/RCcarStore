<%@ Page Language="C#" MasterPageFile="~/BalloonShop.master" 
AutoEventWireup="true" CodeFile="Catalog.aspx.cs" Inherits="Catalog" 
Title="Welcome to Toy Car World" %>

<%@ Register Src="UserControls/ProductsList.ascx" TagName="ProductsList" TagPrefix="uc1" %>
<asp:Content ID="content" ContentPlaceHolderID="contentPlaceHolder" Runat="Server">
    <div style="width: 644px"><asp:Label ID="ProuctsOnline" runat="server" Text="Proucts Online"></asp:Label></div>
  <asp:Label ID="catalogTitleLabel" CssClass="CatalogTitle" Runat="server" />
  <br />
  <asp:Label ID="catalogDescriptionLabel" CssClass="CatalogDescription" Runat="server" />
  <br />
  <uc1:ProductsList ID="ProductsList1" runat="server" />
</asp:Content>
