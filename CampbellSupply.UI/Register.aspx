<%@ Page Title="" Language="C#" MasterPageFile="~/Campbells2.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CampbellSupply.Register" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">
        <!-- SCRIPTS -->
    <script src="assets/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="assets/js/checkout.js" type="text/javascript"></script>
    <script src="assets/js/jquery.maskedinput.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(function ($) {
            $("input[id$='txtPhone']").mask("(999) 999-9999");
        });
    </script>
    <div class="container">
        <div class="col-xs-12 no-margin">
            <section class="section sign-in inner-right-xs">
                <h2 class="bordered">Register</h2>
                <asp:Label ID="lblError" CssClass="text-danger" Visible="false" runat="server"></asp:Label>
                <div class="row field-row">
                    <div class="col-xs-12 col-sm-6">
                        <label>First Name</label>
                        <asp:TextBox ID="txtFname" CssClass="le-input" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="text-danger bold" runat="server" ControlToValidate="txtFname" ErrorMessage="*Oops! First Name Required!"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <label>Last Name</label>
                        <asp:TextBox ID="txtLname" CssClass="le-input" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="text-danger bold" runat="server" ControlToValidate="txtLname" ErrorMessage="*Oops! Last Name Required!"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="field-row">
                </div>
                <div class="row field-row">
                    <div class="col-xs-12 col-sm-6">
                        <label>Email</label>
                        <asp:TextBox ID="txtEmail" CssClass="le-input" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" CssClass="text-danger bold" runat="server" ControlToValidate="txtEmail" ErrorMessage="*Oops! Email Required!"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" CssClass="text-danger bold" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" runat="server" ErrorMessage="*Oops! Invalid Email Address."></asp:RegularExpressionValidator>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <label>Phone</label>
                        <asp:TextBox ID="txtPhone" CssClass="le-input" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" CssClass="text-danger bold" runat="server" ControlToValidate="txtPhone" ErrorMessage="*Oops! Phone Required!"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row field-row">
                    <div class="col-xs-12 col-sm-6">
                        <label>Password</label> <!-- TODO Needs asterisks when entering password -->
                        <asp:TextBox ID="txtPassword" CssClass="le-input" TextMode="Password" runat="server"></asp:TextBox>
                        <asp:Label ID="lblPassError" CssClass="text-danger bold" runat="server"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="text-danger bold" runat="server" ControlToValidate="txtPassword" ErrorMessage="*Oops! Password Required!"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <label>Confirm Password</label> <!-- TODO Needs asterisks when entering password -->
                        <asp:TextBox ID="txtPasswordConfirm" CssClass="le-input" TextMode="Password" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" CssClass="text-danger bold" runat="server" ControlToValidate="txtPasswordConfirm" ErrorMessage="*Oops! Please confirm password."></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator2" CssClass="text-danger bold" ControlToCompare="txtPassword" runat="server" ErrorMessage="*Oops! Passwords don't match!" ControlToValidate="txtPasswordConfirm"></asp:CompareValidator>
                    </div>
                </div>
                <!-- /.field-row -->
                <br />
                <div class="buttons-holder">
                    <asp:Button ID="btnRegister" CssClass="le-button huge pull-right" Text="Register & Continue" runat="server" OnClick="btnLogin_Register" />
                </div>
                <br />
                <br />
                <br />
                <!-- /.buttons-holder -->
            </section>
        </div>
        <!-- /.cf-style-1 -->
        <!-- /.sign-in -->
    </div>
    <!-- /.col -->

    <!-- /.container -->
</asp:Content>