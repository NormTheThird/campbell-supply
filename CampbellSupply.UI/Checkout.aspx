<%@ Page Title="" Language="C#" MasterPageFile="~/Campbells2.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="CampbellSupply.Checkout" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">
    <style>
        .pull-left {
            float: left !important;
        }
    </style>

    <!-- SCRIPTS -->
    <script src="assets/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="assets/js/checkout.js" type="text/javascript"></script>
    <script src="assets/js/jquery.maskedinput.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(function ($) {
            $("input[id$='txtPhone']").mask("(999) 999-9999");
            $("input[id$='txtBillPhone']").mask("(999) 999-9999");
        });
    </script>

    <!-- CHECKOUT SECTION -->
    <section id="cart-page">
        <div class="container">
            <div class="col-xs-12 no-margin">

                <!-- SHIPPING ADDRESS  -->
                <div class="shipping-address">
                    <h2 class="border h1">shipping address</h2>

                    <div class="row field-row">
                        <div class="col-xs-12 col-sm-6">
                            <label>First name*</label>
                            <asp:TextBox ID="txtFname" CssClass="le-input" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="red" ControlToValidate="txtFname" runat="server" ErrorMessage="*Oops! First Name Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 col-sm-6">
                            <label>Last name*</label>
                            <asp:TextBox ID="txtLname" CssClass="le-input" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="red" ControlToValidate="txtLname" runat="server" ErrorMessage="*Oops! Last Name Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="row field-row">
                        <div class="col-xs-12 col-sm-4">
                            <label>address*</label>
                            <asp:TextBox ID="txtAddress1" CssClass="le-input" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="red" ControlToValidate="txtAddress1" runat="server" ErrorMessage="*Oops! Address Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>address 2</label>
                            <asp:TextBox ID="txtAddress2" CssClass="le-input" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>city*</label>
                            <asp:TextBox ID="txtCity" CssClass="le-input" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" CssClass="red" ControlToValidate="txtCity" runat="server" ErrorMessage="*Oops! City Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="row field-row">
                        <div class="col-xs-12 col-sm-4">
                            <label>State*</label>
                            <asp:DropDownList ID="ddlStates" CssClass="le-input" runat="server" />
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>Zip*</label>
                            <asp:TextBox ID="txtZip" CssClass="le-input" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" CssClass="red" ControlToValidate="txtZip" runat="server" ErrorMessage="*Oops! Zip Required." Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="*Oops! Invalid Zip." Display="Dynamic" CssClass="red" ControlToValidate="txtZip" ValidationExpression="\d{5}"></asp:RegularExpressionValidator>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>phone number*</label>
                            <asp:TextBox ID="txtPhone" CssClass="le-input" runat="server" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" CssClass="red" ControlToValidate="txtPhone" runat="server" ErrorMessage="*Oops! Phone Required." Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="*Oops! Invalid Phone." ControlToValidate="txtPhone" CssClass="red" Display="Dynamic" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <!--TODO Wire in email:  if logged in customer just populate email-->
                    <div class="row field-row">
                        <div class="col-xs-12 col-sm-4">
                            <label>Email</label>
                            <asp:TextBox ID="txtEmail" CssClass="le-input" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" CssClass="red" runat="server" ControlToValidate="txtEmail" ErrorMessage="*Oops! Email Required!" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" CssClass="red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" ControlToValidate="txtEmail" runat="server" ErrorMessage="*Oops! Invalid Email Address."></asp:RegularExpressionValidator>
                        </div>
                    </div>
                </div>

                <!-- BILLING SAME A SHIPPING -->
                <section id="billing-address">
                    <div class="row field-row">
                        <div id="create-account" class="col-xs-12">
                            <asp:CheckBox ID="chkBillShip" CssClass="pull-left" AutoPostBack="true" BackColor="white" Width="25px" runat="server" OnCheckedChanged="chkBillShip_CheckedChanged" />
                            <asp:Label ID="lblBillShip" CssClass="bold" AssociatedControlID="chkBillShip" Text="&nbsp;My shipping and billing address are the same" runat="server"></asp:Label>
                        </div>
                    </div>
                </section>

                <!-- BILLING ADDRESS -->
                <section id="secBillingInfo" runat="server">
                    <h2 class="border h1">Billing address</h2>

                    <div class="row field-row">
                        <div class="col-xs-12 col-sm-6">
                            <label>First name*</label>
                            <asp:TextBox ID="txtBillFname" CssClass="le-input" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" CssClass="red" ControlToValidate="txtBillFname" runat="server" ErrorMessage="*Oops! First Name Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 col-sm-6">
                            <label>Last name*</label>
                            <asp:TextBox ID="txtBillLname" CssClass="le-input" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" CssClass="red" ControlToValidate="txtBillLname" runat="server" ErrorMessage="*Oops! Last Name Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="row field-row">
                        <div class="col-xs-12 col-sm-4">
                            <label>address*</label>
                            <asp:TextBox ID="txtBillAddress1" CssClass="le-input" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" CssClass="red" ControlToValidate="txtBillAddress1" runat="server" ErrorMessage="*Oops! Address Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>address 2</label>
                            <asp:TextBox ID="txtBillAddress2" CssClass="le-input" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>city*</label>
                            <asp:TextBox ID="txtBillCity" CssClass="le-input" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" CssClass="red" ControlToValidate="txtBillCity" runat="server" ErrorMessage="*Oops! City Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="row field-row">
                        <div class="col-xs-12 col-sm-4">
                            <label>state*</label>
                            <asp:DropDownList ID="ddlBillStates" CssClass="le-input" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBillStates_SelectedIndexChanged" />
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>Zip*</label>
                            <asp:TextBox ID="txtBillZip" CssClass="le-input" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" CssClass="red" ControlToValidate="txtBillZip" runat="server" ErrorMessage="*Oops! Zip Required." Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="*Oops! Invalid Zip." Display="Dynamic" CssClass="red" ControlToValidate="txtBillZip" ValidationExpression="\d{5}"></asp:RegularExpressionValidator>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>phone number*</label>
                            <asp:TextBox ID="txtBillPhone" CssClass="le-input" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" CssClass="red" ControlToValidate="txtBillPhone" runat="server" ErrorMessage="*Oops! Phone Required." Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="*Oops! Invalid Phone." ControlToValidate="txtBillPhone" CssClass="red" Display="Dynamic" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator>
                        </div>
                    </div>

                </section>

                <!-- ORDER SUMMARY -->
                <section id="your-order">
                    <h2 class="border h1">Order Summary</h2>
                    <asp:Repeater ID="repShoppingCartItems" runat="server">
                        <ItemTemplate>
                            <div class="row no-margin order-item">
                                <div class="col-xs-12 col-sm-1">
                                    <a href="#" class="qty"><%# Eval("Quantity")%></a>
                                </div>

                                <div class="col-xs-12 col-sm-9 no-margin ">
                                    <div class="title"><a href="#"><%# Eval("Name")%></a></div>
                                    <div class="sub-label"><%# Eval("Brand")%></div>
                                    <div class="sub-label"><%# Eval("ManufacturerUPC") %></div>
                                </div>

                                <div class="col-xs-12 col-sm-2 no-margin">
                                    <div class="price pull-right"><%# "$" + string.Format("{0:n2}", Eval("Price"))%></div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </section>

                <!-- ORDER TOTALS -->
                <div id="total-area" class="row no-margin">
                    <div class="col-xs-12 col-lg-4 col-lg-offset-8 no-margin-right">
                        <div id="subtotal-holder">

                            <!-- PRE TOTALS LIST -->
                            <ul class="tabled-data inverse-bold no-border">
                                <li>
                                    <!-- SUBTOTAL -->
                                    <label>Order subtotal</label>
                                    <div class="value pull-right">
                                        <asp:Label ID="lblSubTotal" runat="server" />
                                    </div>
                                </li>
                                <li>
                                    <!-- COUPON -->
                                    <div id="divCoupon" runat="server">
                                        <asp:Label ID="lblCouponText" runat="server" />
                                        <div class="value pull-right">
                                            <asp:Label ID="lblCouponAmount" ForeColor="Red" runat="server" />
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <!-- SHIPPING TOTAL -->
                                    <div runat="server">
                                        <asp:Label ID="lblShippingText" runat="Server">Shipping</asp:Label>
                                        <div class="value pull-right">
                                            <asp:Label ID="lblShippingTotal" runat="server" />
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <!-- SALES TAX -->
                                    <label>Sales Tax</label>
                                    <div class="value pull-right">
                                        <asp:Label ID="lblSalesTax" runat="server" />
                                    </div>
                                </li>
                            </ul>

                            <!-- ORDER TOTAL LIST -->
                            <ul id="total-field" class="tabled-data inverse-bold ">
                                <li>
                                    <!-- ORDER TOTAL -->
                                    <label>order total</label>
                                    <div class="value pull-right">
                                        <asp:Label ID="lblOrderTotal" runat="server" />
                                    </div>
                                </li>
                            </ul>

                        </div>
                    </div>
                </div>

                <!-- ORDER BUTTON -->
                <div class="buttons-holder pull-right">
                    <asp:Button ID="btnContinue" CssClass="le-button size-medium" Text="continue to payment" runat="server" OnClick="btnPayment_Click" />
                    <asp:Button ID="btnCancleOrder" CssClass="le-button size-medium" Text="cancel order" runat="server" OnClick="btnCancleOrder_Click" CausesValidation="false" />
                </div>

            </div>
        </div>
    </section>

</asp:Content>
