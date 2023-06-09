<%@ Page Title="" Language="C#" MasterPageFile="~/Campbells2.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CampbellSupply.Login" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">

    <script type="text/javascript" src='https://www.google.com/recaptcha/api.js'></script>
    <main id="authentication" class="inner-bottom-md">
        <div class="container">
            <div class="row">

                <!-- RETURNING CUSTOMERS -->
                <div class="col-md-6 col-md-offset-3">
                    <asp:Panel ID="pnlLogin" DefaultButton="btnLogin" runat="server">
                        <section class="section sign-in inner-right-xs">
                            <h2 class="bordered">Login</h2>
                            <div class="field-row">
                                <label>Email</label>
                                <asp:TextBox ID="txtEmail" CssClass="le-input" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="text-danger bold" ControlToValidate="txtEmail" runat="server" ErrorMessage="*email required"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail" ErrorMessage="*invalid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </div>
                            <div class="field-row">
                                <label>Password</label>
                                <asp:TextBox ID="txtPassword" CssClass="le-input" TextMode="Password" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="text-danger bold" ControlToValidate="txtPassword" runat="server" ErrorMessage="*password required"></asp:RequiredFieldValidator>
                            </div>

                            <div class="field-row clearfix">
                                <%-- <span class="pull-left">
                                    <label class="content-color">
                                        <asp:CheckBox ID="chkRemember" runat="server" /><span class="bold">&nbsp;Remember me</span></label>
                                </span>--%>
                                <span class="pull-right">
                                    <a href="PasswordReset.aspx" class="content-color bold">Forgotten Password ?</a>
                                </span>
                            </div>

                            <div class="buttons-holder">
                                <br />
                                <asp:Label ID="lblError" CssClass="text-danger bold" runat="server"></asp:Label>
                                <div class="g-recaptcha" data-sitekey="6Lcf2RgUAAAAAHOBobBywiEtEJpiMmhTzDDlVaYm"></div>
                                <br />
                                <asp:Button ID="btnLogin" CssClass="le-button huge" Text="Login" runat="server" OnClick="btnLogin_Click" />
                            </div>

                        </section>
                    </asp:Panel>
                </div>

            </div>
        </div>
    </main>

</asp:Content>
