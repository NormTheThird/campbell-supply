<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderPrintOut.aspx.cs" Inherits="CampbellSupply.OrderPrintOut" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Order</title>

    <!-- Bootstrap Core CSS -->
    <link rel="stylesheet" href="assets/css/bootstrap.min.css" />
    <link rel="stylesheet" href="assets/css/main.css" />

</head>
<body>
    <div class="container" runat="server">

        <!-- TITLE AND ORDER NUMBER -->
        <div class="row">
            <div class="col-lg-6 col-lg-offset-3 text-center">
                <h3>Order# <b>
                    <asp:Label ID="lblOrderNumber" runat="server" /></b></h3>
                <p class="page-subtitle">Here is a summary of your order</p>
            </div>
        </div>

        <!-- SHIPPING AND BILLING INFORMATION -->
        <div class="row center-block">

            <!-- LEFT COLUMN -->
            <div class="col-sm-12 col-md-6 col-lg-6 pull-left">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Billing Information</h3>
                    </div>
                    <div class="panel-body">

                        <!-- BILLING INFORMATION -->
                        <div class="row">
                            <section runat="server">

                                <div class="col-lg-12">
                                    <div class="row">
                                        <div class="col-sm-5">
                                            Order Date:
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblOrderDate" runat="server" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-5">
                                            Paid:
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblPaid" runat="server" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-5">
                                            Transaction Date:
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblTransactionDate" runat="server" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-5">
                                            Name:
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblBillingName" runat="server" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-5">
                                            E-mail:
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblEmail" runat="server" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-5">
                                            Phone:
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblBillingPhone" runat="server" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-5">
                                            Address:
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblBillingAddress" runat="server" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-5">
                                            City, State, Zip:
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblBillingAddress2" runat="server" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-5">
                                            Transaction ID:
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblTransactionID" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-5">
                                            Card Number:
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblCardNumber" runat="server"></asp:Label>
                                        </div>
                                    </div>


                                    <div class="row">
                                        <div class="col-sm-5">
                                            Experation:
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblCardExperation" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                            </section>
                        </div>

                    </div>
                </div>

            </div>

            <!-- RIGHT COLUMN -->
            <div class="col-sm-12 col-md-6 col-lg-6 pull-right">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Shipping Information</h3>
                    </div>
                    <div class="panel-body">

                        <!-- SHIPPING INFORMATION -->
                        <div class="row">
                            <section runat="server">
                                <div class="col-lg-12">
                                    <div class="row">
                                        <div class="col-sm-5">
                                            Name:
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblShippingName" runat="server" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-5">
                                            Address:
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblShippingAddress" runat="server" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-5">
                                            City, State, Zip:
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblShippingAddress2" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </div>
                    </div>
                </div>

            </div>


        </div>

        <!-- ORDER INFORMATION AND FAQ -->
        <div class="row center-block">
            <div class="container">
                <div class="col-lg-12">

                    <!-- ORDER INFORMATION -->
                    <div id="divOrderItems" runat="server" class="row">
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

                            <!-- TOTAL -->
                            <div id="total-area" class="row no-margin">
                                <div class="col-xs-12 col-lg-4 col-lg-offset-8 no-margin-right">
                                    <div id="subtotal-holder">

                                        <!-- TOTALS LIST -->
                                        <ul class="tabled-data inverse-bold no-border pull-right">
                                            <li>
                                                <!-- SUBTOTAL -->
                                                <label>Order subtotal</label>
                                                <div class="value">
                                                    <asp:Label ID="lblSubTotal" runat="server" />
                                                </div>
                                            </li>
                                            <li>
                                                <!-- COUPON -->
                                                <label>Coupon</label>
                                                <div class="value">
                                                    <asp:Label ID="lblCouponAmount" ForeColor="Red" runat="server" />
                                                </div>
                                            </li>
                                            <li>
                                                <!-- SHIPPING TOTAL -->
                                                <label>shipping</label>
                                                <div class="value">
                                                    <asp:Label ID="lblShippingTotal" runat="server" />
                                                </div>
                                            </li>
                                            <li>
                                                <!-- SALES TAX -->
                                                <label>sales tax</label>
                                                <div class="value">
                                                    <asp:Label ID="lblSalesTax" runat="server" />
                                                </div>
                                            </li>
                                            <li>
                                                <!-- ORDER TOTAL -->
                                                <label>order total</label>
                                                <div class="value">
                                                    <asp:Label ID="lblOrderTotal" runat="server" />
                                                </div>
                                            </li>
                                        </ul>

                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="space15"></div>
                    <!-- LOGO -->
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-lg-12 pull-left">
                            <div class="col-lg-6 pull-left">
                                <br />
                                <img src="assets/images/logo.png" />
                            </div>
                            <div class="pull-left">
                                <div class="panel panel-default">
                                    <div class="panel-body">
                                        <h3 class="page-title red-text centerText">Thank you for your Order!</h3>
                                        www.campbellsupply.net or PH: 605-331-5470
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>

                    <!-- FAQ -->
                    <div class="row">
                        <div class="col-lg-12">

                            <p>
                                <b>Can merchandise be returned?</b><br />
                                All merchandise, except gift certificates, can be returned in the original packaging materials with the original invoice. If the 
                        original packaging is not available, merchanddise must be packed properly in a sturdy shipping container. Items must be 
                        unused of defective to be returned. Items that are returned in an unsellable condition will not be credited. Gas powered items 
                        cannot be returned to Campbell's if gassed. Returns are limited to 30 days from original shipping date. Bargin buys are not 
                        returnable. <b>Freight charges are not refundable.</b>
                            </p>

                            <p>
                                <b>How do I return an Item?</b><br />
                                Returns may be made by requesting an authorization from the website or by returning the product(s) to one of our retail 
                        locations. <b>All returns require a Return Authorization (RA) number before returning.</b> This RA number must be written 
                        on the return label(s).
                            </p>

                            <p>
                                <b>What if I receive damaged merchandise?</b><br />
                                If your merchandise arrived damaged, call the delivering carrier that delivered your package(s) within 24 hours of receipt.  
                        Please do this prior to contacting Campbell's.
                            </p>

                            <p>
                                <b>When will I receive my refund or credit?</b><br />
                                We will notify you via email once your return has been received and a refund or credit has been processed. You can expect a 
                        refund in the same form of payment orifinally used to purchase, within 7-14 business days of receiving your return. Freight 
                        charges are not refundable. If the item was received as a gift, you will receive a gift certificate. This certificate may be used 
                        towards your next purchase at Campbell's.
                            </p>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- JavaScripts placed at the end of the document so the pages load faster -->
        <script src="assets/js/jquery-1.10.2.min.js"></script>
        <script src="assets/js/jquery-migrate-1.2.1.js"></script>
        <script src="assets/js/bootstrap.min.js"></script>
    </div>
</body>
</html>
