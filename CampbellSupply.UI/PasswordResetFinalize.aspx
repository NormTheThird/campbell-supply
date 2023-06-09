<%@ Page Title="" Language="C#" MasterPageFile="~/Campbells2.Master" AutoEventWireup="true" CodeBehind="PasswordResetFinalize.aspx.cs" Inherits="CampbellSupply.PasswordResetFinalize" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">

    <div class="container inner-top-xs inner-bottom-sm">

        <!-- ERROR MESSAGE -->
        <div id="spnInvalid" runat="server">
            <div class="col-lg-6 col-sm-12 col-xs-12">
                <asp:Label ID="lblErrorInvalid" CssClass="text-danger bold" runat="server">Oops! Were sorry you have incorrectly accessed this page.</asp:Label>
            </div>
        </div>

        <!-- PASSWORD RESET -->
        <div id="spnReset" runat="server">

            <!-- HEADER -->
            <div class="row">
                <div class="col-xs-12 col-md-10 col-lg-10 col-sm-6">

                    <section id="who-we-are" class="section m-t-0">
                        <h2>Reset your password</h2>
                        <p>Please enter your new password below.</p>
                    </section>
                </div>
            </div>

            <!-- NEW PASSWORDS -->
            <div class="row">
                <div class="col-lg-6 col-sm-12 col-xs-12">
                    <label>New Password:</label>
                    <asp:TextBox ID="txtPassword" CssClass="le-input col-lg-6 col-xs-12" TextMode="Password" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="red" runat="server" ControlToValidate="txtPassword" ErrorMessage="*Oops! Password Required!" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <div class="col-lg-6 col-sm-12 col-xs-12">
                    <label>Confirm New Password:</label>
                    <asp:TextBox ID="txtPasswordConfirm" CssClass="le-input col-lg-6 col-xs-12" TextMode="Password" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="red" runat="server" ControlToValidate="txtPasswordConfirm" ErrorMessage="*Oops! Password Required!" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" CssClass="red" ControlToCompare="txtPassword" ControlToValidate="txtPasswordConfirm" ErrorMessage="*Oops! Passwords don't match!"></asp:CompareValidator>
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
    </div>

</asp:Content>
