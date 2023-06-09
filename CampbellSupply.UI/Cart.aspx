<%@ Page Title="" Language="C#" MasterPageFile="~/Campbells2.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="CampbellSupply.Cart" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">

    <section id="cart-page">
        <div class="container">

            <!-- SHOPPING CART -->
            <div class="col-xs-12 col-md-9 items-holder no-margin">
                <asp:Label ID="lblOrderCancel" CssClass="page-title red-text h2" Text="Your order has been cancelled." runat="server"></asp:Label>
                <asp:Label ID="lblCartEmpty" CssClass="page-title red-text h2" Text="Your Shopping Cart is empty" runat="server"></asp:Label>

                <!-- SHOPPING CART ITEMS -->
                <asp:Repeater ID="repShoppingCartItems" runat="server">
                    <ItemTemplate>
                        <div class="row no-margin cart-item">
                            <div class="col-xs-12 col-sm-2 no-margin">
                                <a href="#" class="thumb-holder">
                                    <img class="lazy" alt="" width="70" height="70" src="<%# "https://s3-us-west-2.amazonaws.com/campbellsp/Product/" + Eval("URL")%>"
                                        onerror="this.onerror=null;this.src='assets/images/NoImgAvail.png';" />
                                </a>
                            </div>
                            <div class="col-xs-12 col-sm-5 ">
                                <div class="title"><a href="ItemDetail.aspx?productid=<%# Eval("ProductId")%>"><%# Eval("Name")%> <%# Eval("Size")%></a></div>
                                <div class="sub-label"><%# Eval("Brand")%></div>
                                <div class="sub-label"><%# Eval("ManufacturerUPC") %></div>
                            </div>
                            <div class="col-xs-12 col-sm-3 no-margin">
                                <div class="quantity">
                                    <div class="le-quantity">
                                        <a id="minus" class="minus" name='<%# Eval("Id")%>' runat="server" onserverclick="RemoveCountToItem"></a>
                                        <div>
                                            <input name="quantity" readonly="readonly" type="text" value="<%# Eval("Quantity")%>" />
                                        </div>
                                        <a class="plus" name='<%# Eval("Id")%>' runat="server" onserverclick="AddCountToItem"></a>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-2 no-margin">
                                <div class="price"><%# "$" + string.Format("{0:n2}", Eval("Price"))%></div>
                                <a class="close-btn" name='<%# Eval("Id")%>' runat="server" onserverclick="DeleteItemFromCart"></a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

            </div>

            <!-- RIGHT SIDE MEMU -->
            <div class="col-xs-12 col-md-3 no-margin sidebar ">

                <!-- SHOPPING CART SUMMARY -->
                <div class="widget cart-summary">
                    <h1 class="border">shopping cart</h1>
                    <div class="body">

                        <!-- PRE TOTALS LIST -->
                        <ul class="tabled-data no-border inverse-bold">
                            <li>
                                <!-- SUBTOTAL -->
                                <label>cart subtotal</label>
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
                        </ul>

                        <!-- ORDER TOTAL LIST -->
                        <ul id="total-price" class="tabled-data inverse-bold no-border">
                            <li>
                                <!-- ORDER TOTAL -->
                                <label>order total</label>
                                <div class="value pull-right">
                                    <asp:Label ID="lblOrderTotal" runat="server" />
                                </div>
                            </li>
                        </ul>
                        <div class="buttons-holder">
                            <asp:Button ID="btnCheckOut" CssClass="le-button big" Text="checkout" runat="server" OnClick="btnCheckOut_Click" />
                            <asp:LinkButton ID="btnContShopping" CssClass="simple-link block" Text="continue shopping" runat="server" OnClick="btnContShopping_Click" />
                        </div>
                    </div>
                </div>

                <!-- COUPON -->
                <div id="cupon-widget" class="widget">
                    <h1 class="border">use coupon</h1>
                    <div class="body">
                        <div class="inline-input">
                            <input id="couponCode" runat="server" data-placeholder="enter coupon code" type="text" />
                            <asp:LinkButton ID="btnApply" CssClass="le-button" Text="Apply" runat="server" OnClick="btnApply_Click" />
                        </div>
                    </div>
                </div>

            </div>

        </div>
    </section>

</asp:Content>
