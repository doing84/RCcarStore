<%@ Page Title="" Language="C#" MasterPageFile="~/BalloonShop.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Users_Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headPlaceHolder" Runat="Server">
     <link rel="stylesheet" href="/Css/compiled.css?v=201404281344"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolder" Runat="Server">
    <section class="banners banners_home">

    
        <div class="panel clear narrow">
        <h2>Sign in to your Toy Car account</h2>

        <form action="/Account/Login" method="post">

            <input name="__RequestVerificationToken" type="hidden" value="CpGgiS7Rxc77VrPdGbD_NDaYajOKsCdY-2Net4_JalyVSyZixVTpRpouaIMj7G-rXbuGnyQBNXrLbQrbDLskKSKxn3g2fMcRVnV43ca8J4N989RUUU2tuz3jatqRGGz-iiGm6A2" width="120px" />            

            <input type="hidden" name="returnUrl" />
            <table>
             	<tr> 

             		<td valign="baseline">
                        <label for="UserName" style="font-size: x-large">Your ID</label>
                    </td>
                    <td>
                        <asp:TextBox ID="userID" runat="server" class="input"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter your ID" ForeColor="#CC0000" ControlToValidate="userID" ValidationGroup="login"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr> 
             		<td valign="baseline">
                        <label for="Password" style="font-size: x-large">Your password</label>
                    </td>
                    <td>
                        <asp:TextBox ID="pwd" class="input" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your password" ForeColor="#CC0000" ControlToValidate="pwd" ValidationGroup="login"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr> 
             		<td>
                        <label class="checkbox" style="font-size: large">
                        <input checked="checked" class="checkbox" data-val="true" data-val-required="The Remember me? field is required." id="Checkbox1" name="RememberMe" type="checkbox" value="true" /><input name="RememberMe" type="hidden" value="false" />
                         Remember me?
                           </label>
                    </td>
                    <td valign="baseline">
                        <asp:Button ID="login" OnClick="login_Click" runat="server" class="button button_large" Text="Login" ValidationGroup="login" />
                    </td>
                </tr>
                </table> 
                        <a href="/Account/Register.aspx" class="forgot">REGISTRATION</a>
        </form>

        
    </div>
    
</section>
</asp:Content>

