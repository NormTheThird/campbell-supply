<%@ Page Title="" Language="C#" MasterPageFile="~/Campbells2.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="CampbellSupply.Payment" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">

    <!-- SCRIPTS -->
    <script src="assets/js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="assets/js/checkout.js" type="text/javascript"></script>
    <script src="assets/js/jquery.maskedinput.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(function ($) {
            $("input[id$='txtBillPhone']").mask("(999) 999-9999");
            $(".submitButton").click(function () {
                if (typeof (Page_ClientValidate) == 'function') {
                    Page_ClientValidate();
                }

                if (Page_IsValid) {
                    $(".wait-container").show();
                }
            });
        });
    </script>

    <!-- PAYMENT SECTION -->
    <section id="cart-page">
        <div class="container">
            <div class="col-xs-12 no-margin">

                <!-- PAYMENT -->
                <div class="shipping-address">
                    <h2 class="border h1">Payment Method</h2>

                    <div class="row field-row">
                        <div class="col-xs-12 col-sm-4">
                            <label>Card Type*</label>
                            <img class="img img-responsive pull-left" src="assets/images/visa-straight-32px.png" runat="server" id="imgVisa" />
                            <img class="img img-responsive pull-left" src="assets/images/visa-straight-32px.png" runat="server" id="imgGrayVisa" visible="false" style="filter: grayscale(100%)" />
                            <img class="img img-responsive pull-left" src="assets/images/mastercard-straight-32px.png" runat="server" id="imgMasterCard" />
                            <img class="img img-responsive pull-left" src="assets/images/mastercard-straight-32px.png" runat="server" id="imgGrayMasterCard" visible="false" style="filter: grayscale(100%)" />
                            <img class="img img-responsive pull-left" src="assets/images/discover-straight-32px.png" runat="server" id="imgDiscover" />
                            <img class="img img-responsive pull-left" src="assets/images/discover-straight-32px.png" runat="server" id="imgGrayDiscover" visible="false" style="filter: grayscale(100%)" />
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>Card Number*</label>
                            <asp:TextBox ID="txtCCnumber" CssClass="le-input" runat="server" OnTextChanged="txtCCnumber_TextChanged" AutoPostBack="true" TabIndex="1"></asp:TextBox>
                            <asp:Label runat="server" ID="lblCCnumber" CssClass="label label-danger"></asp:Label>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="red" ControlToValidate="txtCCnumber" runat="server" ErrorMessage="*Oops! Last Name Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>name on card*</label>
                            <asp:TextBox ID="txtName" CssClass="le-input" runat="server" TabIndex="5"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" CssClass="red" ControlToValidate="txtName" runat="server" ErrorMessage="*Oops! Cardholder Name Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="row field-row">
                        <div class="col-xs-12 col-sm-4">
                            <label>expiration date*</label>
                            <asp:DropDownList CssClass="le-input" ID="ddExperation" runat="server" TabIndex="2">
                                <asp:ListItem Selected="True" Value="01">Jan</asp:ListItem>
                                <asp:ListItem Value="02">Feb</asp:ListItem>
                                <asp:ListItem Value="03">Mar</asp:ListItem>
                                <asp:ListItem Value="04">Apr</asp:ListItem>
                                <asp:ListItem Value="05">May</asp:ListItem>
                                <asp:ListItem Value="06">Jun</asp:ListItem>
                                <asp:ListItem Value="07">Jul</asp:ListItem>
                                <asp:ListItem Value="08">Aug</asp:ListItem>
                                <asp:ListItem Value="09">Sep</asp:ListItem>
                                <asp:ListItem Value="10">Oct</asp:ListItem>
                                <asp:ListItem Value="11">Nov</asp:ListItem>
                                <asp:ListItem Value="12">Dec</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="red" ControlToValidate="ddExperation" runat="server" ErrorMessage="*Oops! Address Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>Year*</label>
                            <asp:DropDownList ID="ddYear" CssClass="le-input" runat="server" TabIndex="3"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" CssClass="red" ControlToValidate="ddYear" runat="server" ErrorMessage="*Oops! City Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>Security Code*</label>
                            <asp:TextBox ID="txtCVV" CssClass="le-input" runat="server" TabIndex="4"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" CssClass="red" ControlToValidate="txtCVV" runat="server" ErrorMessage="*Oops! CVV Required." Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="*Oops! Last 3 digits on the back of your card." Display="Dynamic" CssClass="red" ControlToValidate="txtCVV" ValidationExpression="\d{3}"></asp:RegularExpressionValidator>
                        </div>
                    </div>

                </div>

                <!-- BILLING ADDRESS -->
                <section id="secBillingInfo" runat="server">
                    <h2 class="border h1">Billing address
                        <asp:LinkButton ID="lnkEdit" CssClass="" Text="- edit" runat="server" OnClick="lnkEdit_Click" CausesValidation="false"></asp:LinkButton></h2>

                    <div class="row field-row">
                        <div class="col-xs-12 col-sm-6">
                            <label>First name*</label>
                            <asp:TextBox ID="txtBillFname" CssClass="le-input" ReadOnly="true" runat="server" TabIndex="6"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" CssClass="red" ControlToValidate="txtBillFname" runat="server" ErrorMessage="*Oops! First Name Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 col-sm-6">
                            <label>Last name*</label>
                            <asp:TextBox ID="txtBillLname" CssClass="le-input" ReadOnly="true" runat="server" TabIndex="7"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" CssClass="red" ControlToValidate="txtBillLname" runat="server" ErrorMessage="*Oops! Last Name Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="row field-row">
                        <div class="col-xs-12 col-sm-4">
                            <label>address*</label>
                            <asp:TextBox ID="txtBillAddress1" CssClass="le-input" ReadOnly="true" runat="server" TabIndex="8"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" CssClass="red" ControlToValidate="txtBillAddress1" runat="server" ErrorMessage="*Oops! Address Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>address 2</label>
                            <asp:TextBox ID="txtBillAddress2" CssClass="le-input" ReadOnly="true" runat="server" TabIndex="8"></asp:TextBox>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>city*</label>
                            <asp:TextBox ID="txtBillCity" CssClass="le-input" ReadOnly="true" runat="server" TabIndex="9"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" CssClass="red" ControlToValidate="txtBillCity" runat="server" ErrorMessage="*Oops! City Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="row field-row">
                        <div class="col-xs-12 col-sm-4">
                            <label>state*</label>
                            <asp:DropDownList ID="ddlBillStates" CssClass="le-input" Enabled="false" runat="server" TabIndex="10"></asp:DropDownList>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>Zip*</label>
                            <asp:TextBox ID="txtBillZip" CssClass="le-input" ReadOnly="true" runat="server" TabIndex="11"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" CssClass="red" ControlToValidate="txtBillZip" runat="server" ErrorMessage="*Oops! Zip Required." Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <label>phone number*</label>
                            <asp:TextBox ID="txtBillPhone" CssClass="le-input" ReadOnly="true" runat="server" TabIndex="12"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" CssClass="red" ControlToValidate="txtBillPhone" runat="server" ErrorMessage="*Oops! Phone Required." Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="*Oops! Invalid Phone." ControlToValidate="txtBillPhone" CssClass="red" Display="Dynamic" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator>
                        </div>
                    </div>

                </section>

                <!-- ORDER ITEMS -->
                <section id="your-order">
                    <h2 class="border h1">Order Summary</h2>
                    <asp:Repeater ID="repShoppingCartItems" runat="server">
                        <ItemTemplate>
                            <div class="row no-margin order-item">
                                <div class="col-xs-12 col-sm-1 no-margin">
                                    <a href="#" class="qty"><%# Eval("Quantity")%></a>
                                </div>

                                <div class="col-xs-12 col-sm-9 ">
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

                <!-- TOTAL -->
                <div id="total-area" class="row no-margin">
                    <div class="col-xs-12 col-lg-4 col-lg-offset-8 no-margin-right">
                        <div id="subtotal-holder">
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
                            <ul id="total-field" class="tabled-data inverse-bold">
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

                <!-- PLACE ORDER -->
                <div class="place-order-button pull-right">
                    <br />
                    <asp:Button ID="btnContinue" CssClass="le-button size-medium submitButton" Text="Submit Order" runat="server" OnClick="btnContinue_Click" TabIndex="13" />
                    <asp:Button ID="btnCancleOrder" CssClass="le-button size-medium" Text="cancel order" runat="server" OnClick="btnCancleOrder_Click" CausesValidation="false" />
                    <br />
                    <br />
                    <br />
                </div>

                <!-- ERROR MESSAGE -->
                <div class="row field-row">
                    <div class="col-sm-12">
                        <div class="h2">
                            <asp:Label runat="server" ID="lblErrorProcessing" CssClass="label label-danger pull-right" Text=""></asp:Label>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </section>

</asp:Content>
