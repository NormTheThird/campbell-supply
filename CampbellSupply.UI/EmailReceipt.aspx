<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailReceipt.aspx.cs" Inherits="CampbellSupply.EmailReceipt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!-- Bootstrap Core CSS -->
    <!-- <link rel="stylesheet" href="assets/css/bootstrap.min.css"> -->
    <link rel="stylesheet" href="assets/css/bootstrap.css" />

    <!-- Main CSS -->
    <link rel="stylesheet" href="assets/css/main.css" />
    <link rel="stylesheet" href="assets/css/red.css" />
    <link rel="stylesheet" href="assets/css/owl.carousel.css" />
    <link rel="stylesheet" href="assets/css/owl.transitions.css" />
    <link rel="stylesheet" href="assets/css/animate.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <div id="PrintDiv" class="container" runat="server">

                <!-- TITLE AND ORDER NUMBER -->
                <section class="section">
                    <div class="page-header">
                        <img src="assets/images/logo.png" />
                        <h1 class="page-title red-text">Thank you for your Order!</h1>
                        <h3>Order# <b>
                            <asp:Label ID="lblOrderNumber" runat="server" /></b></h3>
                        <p class="page-subtitle">Here is a summary of your order</p>
                    </div>
                </section>

                <!-- ORDER SECTION -->
                <section id="orderSummary">

                    <!-- SHIPPING ADDRESS -->
                    <div class="shipping-address">
                        <h2 class="border h1">shipping address</h2>

                        <div class="row field-row">
                            <div class="col-xs-12 col-sm-4">
                                <label>First name</label>
                                <asp:Label ID="lblFname" runat="server" />
                            </div>
                            <div class="col-xs-12 col-sm-8">
                                <label>Last name</label>
                                <asp:Label ID="lblLname" runat="server" />
                            </div>
                        </div>

                        <div class="row field-row">
                            <div class="col-xs-12 col-sm-4">
                                <label>address</label>
                                <asp:Label ID="lblAddress1" runat="server" />
                            </div>
                            <div class="col-xs-12 col-sm-4">
                                <label>address 2</label>
                                <asp:Label ID="lblAddress2" runat="server" />
                            </div>
                            <div class="col-xs-12 col-sm-4">
                                <label>city</label>
                                <asp:Label ID="lblCity" runat="server" />
                            </div>
                        </div>

                        <div class="row field-row">
                            <div class="col-xs-12 col-sm-4">
                                <label>State</label>
                                <asp:Label ID="lblState" runat="server" />
                            </div>
                            <div class="col-xs-12 col-sm-4">
                                <label>zip code</label>
                                <asp:Label ID="lblZip" runat="server" />
                            </div>

                            <div class="col-xs-12 col-sm-4">
                                <label>phone number</label>
                                <asp:Label ID="lblPhoneNumber" runat="server" />
                            </div>
                        </div>

                    </div>

                    <!-- BILLING ADDRESS -->
                    <section id="secBillingInfo" runat="server">
                        <h2 class="border h1">Billing address</h2>

                        <div class="row field-row">
                            <div class="col-xs-12 col-sm-4">
                                <label>first name</label>
                                <asp:Label ID="lblBillFname" runat="server" />
                            </div>
                            <div class="col-xs-12 col-sm-8">
                                <label>last name*</label>
                                <asp:Label ID="lblBillLname" runat="server" />
                            </div>
                        </div>

                        <div class="row field-row">
                            <div class="col-xs-12 col-sm-4">
                                <label>address</label>
                                <asp:Label ID="lblBillAddress1" runat="server" />
                            </div>
                            <div class="col-xs-12 col-sm-4">
                                <label>address 2</label>
                                <asp:Label ID="lblBillAddress2" runat="server" />
                            </div>
                            <div class="col-xs-12 col-sm-4">
                                <label>city</label>
                                <asp:Label ID="lblBillCity" runat="server" />
                            </div>
                        </div>

                        <div class="row field-row">
                            <div class="col-xs-12 col-sm-4">
                                <label>state</label>
                                <asp:Label ID="lblBillState" runat="server" />
                            </div>
                            <div class="col-xs-12 col-sm-4">
                                <label>zip code</label>
                                <asp:Label ID="lblBillZip" runat="server" />
                            </div>

                            <div class="col-xs-12 col-sm-4">
                                <label>phone number</label>
                                <asp:Label ID="lblBillPhone" runat="server" />
                            </div>
                        </div>

                    </section>

                    <!-- ORDER SUMMARY -->
                    <section id="your-order">
                        <h2 class="border h1">Order Summary</h2>
                        <div class="col-lg-12">
                            <div class="table-responsive">
                                <table class="table table-striped">
                                    <asp:Repeater ID="repOrderItems" runat="server">
                                        <HeaderTemplate>
                                            <thead>
                                                <tr>
                                                    <th>SKU</th>
                                                    <th>Part Number</th>
                                                    <th>Name</th>
                                                    <th>Size</th>
                                                    <th>Color</th>
                                                    <th>Qty</th>
                                                    <th>Price</th>
                                                </tr>
                                            </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tbody>
                                                <tr>
                                                    <td><%# Eval("Sku") %></td>
                                                    <td><%# Eval("PartNumber") %></td>
                                                    <td><%# Eval("Name") %></td>
                                                    <td><%# Eval("Size") %></td>
                                                    <td><%# Eval("Color") %></td>
                                                    <td><%# Eval("Quantity") %></td>
                                                    <td><%# Eval("Price") %></td>
                                                </tr>
                                            </tbody>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>
                        <%--                        <asp:Repeater ID="repOrderItems" runat="server">
                            <ItemTemplate>
                                <div class="row no-margin order-item">
                                    <div class="col-xs-12 col-sm-1 no-margin">
                                        <a href="#" class="qty"><%# Eval("Quantity")%></a>
                                    </div>

                                    <div class="col-xs-12 col-sm-4">
                                        <div class="title"><a href="#"><%# Eval("Name")%></a></div>
                                        <div class="brand"><%# Eval("Brand")%></div>
                                        <div class="ManufacturerUPC"><%# Eval("ManufacturerUPC") %></div>

                                    </div>

                                    <div class="col-xs-12 col-sm-4">
                                        <div class="price pull-right"><b><%# "$" + string.Format("{0:n2}", Eval("Price"))%></b></div>
                                    </div>
                                </div>
                                <hr />
                            </ItemTemplate>
                        </asp:Repeater>--%>
                    </section>

                    <!-- ORDER TOTALs -->
                    <div id="total-area" class="row no-margin">
                        <div class="col-xs-12 col-lg-4 col-lg-offset-8 no-margin-right">
                            <div id="subtotal-holder">

                                <!-- PRE TOTALS LIST -->
                                <ul class="tabled-data inverse-bold no-border pull-right">
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
                                    <li id="total-field">
                                        <!-- ORDER TOTAL -->
                                        <label>order total</label>
                                        <div class="value pull-right">
                                            <asp:Label ID="lblOrderTotal" runat="server" />
                                        </div>
                                    </li>
                                </ul>

                                <!-- ORDER TOTAL LIST -->
                                <!-- <ulclass="abled-data inverse-bold no-border pull-right">
                             
                            </ul>
                            -->

                            </div>
                        </div>
                    </div>

                </section>

            </div>

        </div>
    </form>
</body>
</html>
