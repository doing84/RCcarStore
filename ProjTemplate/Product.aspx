<%@ Page Language="C#" MasterPageFile="~/BalloonShop.master" AutoEventWireup="true"
  CodeFile="Product.aspx.cs" Inherits="Product" Title="Untitled Page" %>

<%@ Register Src="UserControls/ProductRecommendations.ascx" TagName="ProductRecommendations"
  TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headPlaceHolder" Runat="Server">
    
          <script language="javascript" type="text/javascript">
           function submit_Click()
           {
                   //check validation

                var title = document.getElementById('<%=reviewTl.ClientID%>');
                var editor = document.getElementById('<%=editor1.ClientID%>');
                var info = '<%=Request.Cookies["userInfo"]%>';

                if (info == "") {
                    alert("You need to login first before writting review");
                    return false;
                }

                if (title.value == "") {
                    alert("Please enter your title");
                    title.focus();
                    return false;
                }

                if (editor.value == "") {
                    alert("Please enter your review");
                    editor.focus();
                    return false;
                }

                return true;
           }

           function del(obj)
           {

               if (confirm("Do you want to delete your review?")) {
                   return true;
               }

               return false;
           }
    </script>
       <style type="text/css">
           .thumb {
               width: 261px;
           }
       </style>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="contentPlaceHolder" runat="Server">
  <br />
  <asp:Label CssClass="ProductTitle" ID="titleLabel" runat="server" Text="Label"></asp:Label>
  <br />
  <br />
  <asp:Image ID="productImage" runat="server" />
  <br />
  <asp:Label CssClass="ProductDescription" ID="descriptionLabel" runat="server" Text="Label"></asp:Label>
  <br />
  <br />
  <span class="ProductDescription">Price:</span>&nbsp;
  <asp:Label CssClass="ProductPrice" ID="priceLabel" runat="server" Text="Label" />
  <br />
  <asp:Button ID="addToCartButton" runat="server" Text="Add to Cart" CssClass="SmallButtonText" OnClick="addToCartButton_Click" />
  <asp:Button ID="continueShoppingButton" CssClass="SmallButtonText" runat="server" Text="Continue Shopping" OnClick="continueShoppingButton_Click" />
   
  <br />
     <tr>
         <td>
     <asp:Label ID="lblError" runat="server"></asp:Label>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td style="height:30px" colspan="2"></td>
                            </tr>
                            <tr>
                                <td>Title : </td>
                                <td style="height:30px">
                                    <asp:TextBox ID="reviewTl" ValidationGroup="comm" runat="server" Width="300"></asp:TextBox> 
                                </td>
                            </tr>
                            <tr>
                                <td>Review : </td>
                                <td>
                                     <asp:TextBox ID="editor1" runat="server" ValidationGroup="comm" TextMode="MultiLine" Rows="6" Width="300" OnTextChanged="editor1_TextChanged"></asp:TextBox> 
  
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                   <asp:Button ID="addComm" ValidationGroup="comm" runat="server" Text="Write" OnClientClick="if (!submit_Click()) return false;" OnClick="addComm_Click" />
                                    
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
              </td>
            
             <td>
                 <asp:UpdatePanel ID="commListUp" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                    
                    <asp:ListView ID="commList" runat="server" ItemPlaceholderID="commItemList" OnItemDataBound="ListView_ItemDataBound">
                        <LayoutTemplate>
                            <p>&nbsp;</p>
                            <table cellpadding="0" cellspacing="3" border="0" width="800px">
	                            <caption><asp:Label ID="Label1" runat="server" /></caption>
	                            <asp:PlaceHolder ID="commItemList" runat="server" />
	                            <tfoot>
            	                   
	                            </tfoot>
            	                
	                         </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                                <tr>
                                    <td style="width:60px; padding:5px" align="center" valign="top">
                                        <%#Eval("UserID") %>
                                    </td>
                                    <td align="left" style="padding:5px">
                                        <b><%#Eval("Name") %></b><br />
                                        <%#Eval("Description") %>
                                    </td>
                                    <td align="right" style="width:30px" valign="top">
                                        <%#Eval("DateAdded") %>
                                        <asp:ImageButton ID="x" runat="server" Visible="False" CommandName="reviewDel" CommandArgument='<%# Eval("ProductID") %>' OnCommand="imgBtn_Click" OnClientClick="return del(this);" />
                                    </td>
                                </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table cellpadding="0" cellspacing="3" border="0" width="650">
	                            <caption><asp:Label ID="Label1" runat="server" /></caption>
	                         </table>
                        </EmptyDataTemplate>
                    </asp:ListView>
                    
                    </ContentTemplate>
                </asp:UpdatePanel>

            </td>
        </tr>

    
  <br />
  <uc1:ProductRecommendations id="ProductRecommendations1" runat="server">
  </uc1:ProductRecommendations>
  </asp:Content>
