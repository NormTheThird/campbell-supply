<%@ Page Title="" Language="C#" MasterPageFile="~/Campbells2.Master" AutoEventWireup="true" CodeBehind="PasswordReset.aspx.cs" Inherits="CampbellSupply.PasswordReset" %>

<asp:Content ContentPlaceHolderID="main" runat="server">

    <div class="container inner-top-xs inner-bottom-sm">

        <!-- HEADER -->
        <div class="row">
            <div class="col-xs-12 col-md-10 col-lg-10 col-sm-6">
                <section id="who-we-are" class="section m-t-0">
                    <h2>Reset your password</h2>
                    <p>Forgot your password?  Please enter your email below and we'll send you a password reset link.</p>
                </section>
            </div>
        </div>

        <!-- EMAIL FIELD -->
        <div class="row">
            <div class="col-lg-6 col-sm-12 col-xs-12">
                <label>Email Address:</label>
                <asp:TextBox ID="txtEmailAddress" CssClass="le-input col-lg-6 col-xs-12" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="red" runat="server" ControlToValidate="txtEmailAddress" ErrorMessage="*Oops! Email Required!" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" CssClass="red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmailAddress" runat="server" ErrorMessage="*Oops! Invalid Password." Display="Dynamic"></asp:RegularExpressionValidator>

            </div>
        </div>

        <!-- MESSAGES AND BUTTON -->
        <div class="buttons-holder">
            <br />
            <asp:Label ID="lblMessage" CssClass="text-danger bold" Text="Password reset sent!" Visible="false" runat="server"></asp:Label>
            <asp:Label ID="lblError" CssClass="text-danger bold" Visible="false" runat="server"></asp:Label>
            <br />
            <br />
            <asp:Button ID="btnReset" CssClass="le-button huge" Text="Submit" runat="server" OnClick="btnReset_Click" />
        </div>

    </div>

</asp:Content>
