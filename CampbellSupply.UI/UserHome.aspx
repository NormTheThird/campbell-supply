<%@ Page Title="" Language="C#" MasterPageFile="~/Campbells2.Master" AutoEventWireup="true" CodeBehind="UserHome.aspx.cs" Inherits="CampbellSupply.UserHome" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">

    <section id="category-grid">
        <div class="container">

            <!-- SIDEBAR -->
            <div class="col-xs-12 col-sm-3 no-margin sidebar narrow">

                <div class="widget">
                    <div class="body bordered">

                        <div class="category-filter">
                            <h2>
                                <asp:Label ID="lblDepartment" CssClass="bold text-info" runat="server" Text="Customer Orders"></asp:Label></h2>
                            <ul>
                                <!-- Order History -->
                                <li>View Orders</li>
                            </ul>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-xs-12 col-sm-9 no-margin wide sidebar">

                <section id="checkout-page">
                    <div class="grid-list-products">
                        <div class="tab-content">

                            <!-- ORDER HISTORY SEARCH -->
                            <div id="divOrderHistorySearch" runat="server" class="row">
                                <div class="col-xs-12 col-md-4">
                                    <asp:TextBox ID="txtOrderSearch" CssClass="le-input" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-xs-12 col-md-4">
                                    <asp:Button ID="btnSearch" CssClass="btn btn-default btn-sm" Text="Search" runat="server" OnClick="btnSearch_Click" />
                                </div>
                            </div>
                            <div class="space15"></div>

                            <!-- ORDER HISTORY TABLE -->
                            <div id="divOrderHistory" runat="server" class="row">
                                <div class="col-xs-12 col-md-12">
                                    <div class="table-responsive">
                                        <table class="table table-striped">
                                            <asp:Repeater ID="rpOrderHistory" runat="server">
                                                <HeaderTemplate>
                                                    <thead>
                                                        <tr>
                                                            <th>Order#</th>
                                                            <th>Order Amount</th>
                                                            <th>Order Date</th>
                                                            <th>View</th>
                                                        </tr>
                                                    </thead>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tbody>
                                                        <tr>
                                                            <td><%# Eval("OrderNumber") %></td>
                                                            <td><%# Eval("OrderAmount") %></td>
                                                            <td><%# Eval("OrderDate") %></td>
                                                            <td>
                                                                <asp:Button ID="btnDetails" CommandName="cmd_Order" CommandArgument='<%# Bind("OrderId") %>' CssClass="btn btn-default btn-sm" Text="View" runat="server" OnClick="btnDetails_Click" /></td>
                                                        </tr>
                                                    </tbody>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>

                                    <!-- PAGING -->
                                    <div class="pagination-holder">
                                        <div class="row">

                                            <!-- PAGER -->
                                            <div class="col-xs-12 col-sm-9 text-left">
                                                <ul class="pagination ">

                                                    <!-- FIRST BUTTON -->
                                                    <asp:LinkButton ID="btnFirst" runat="server" Text="First" CommandArgument="1" OnClick="Page_Changed" Visible="false" />

                                                    <!-- PREVIOUS BUTTON -->
                                                    <asp:LinkButton ID="btnPrevious" runat="server" Text="<" CommandArgument="-1" OnClick="Page_Changed" Visible="false" />

                                                    <!-- PAGER REPEATER -->
                                                    <asp:Repeater ID="rptPaging" runat="server">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                                                CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "page_enabled" : "page_disabled" %>'
                                                                BackColor='<%# Convert.ToBoolean(Eval("Enabled")) ? System.Drawing.Color.White : System.Drawing.Color.Red %>'  
                                                                OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>' />
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                    <!-- NEXT BUTTON -->
                                                    <asp:LinkButton ID="btnNext" runat="server" Text=">" CommandArgument="-2" OnClick="Page_Changed" Visible="false" />

                                                    <!-- LAST BUTTON -->
                                                    <asp:LinkButton ID="btnLast" runat="server" Text="Last" OnClick="Page_Changed" Visible="false" />
                                                </ul>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- ORDER HISTORY ERROR -->
                            <div id="divOrderHistoryError" runat="server" class="row" visible="false">
                                <div class="col-xs-12 col-md-12">
                                    <h1>Order History Information Does Not Exist!</h1>
                                </div>
                            </div>

                        </div>

                        <!-- OREDER HISTORY DETAILS -->
                        <div id="divOrderDetails" visible="false" runat="server" class="row">
                            <div class="col-xs-12 col-md-12">
                                <asp:Button ID="btnHideDetails" CssClass="btn btn-danger btn-sm center-block" Text="Hide Details" runat="server" OnClick="btnHideDetails_Click" />
                                <section id="orderSummary">

                                    <!-- SHIPPING ADDRESS -->
                                    <div class="shipping-address">

                                        <h2 class="border h1">shipping address</h2>
                                        <div class="row field-row">
                                            <div class="col-xs-12 col-sm-4">
                                                <label>First name*</label>
                                                <asp:Label ID="lblFname" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-xs-12 col-sm-8">
                                                <label>Last name*</label>
                                                <asp:Label ID="lblLname" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row field-row">
                                            <div class="col-xs-12 col-sm-4">
                                                <label>address*</label>
                                                <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-xs-12 col-sm-4">
                                                <label>city*</label>
                                                <asp:Label ID="lblCity" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-xs-12 col-sm-4">
                                                <label>state*</label>
                                                <asp:Label ID="lblState" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row field-row">
                                            <div class="col-xs-12 col-sm-4">
                                                <label>Zip*</label>
                                                <asp:Label ID="lblZip" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-xs-12 col-sm-8">
                                                <label>phone number*</label>
                                                <asp:Label ID="lblPhoneNumber" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                    </div>

                                    <!-- BILLING ADDRESS -->
                                    <section id="secBillingInfo" runat="server">
                                        <h2 class="border h1">Billing address</h2>
                                        <div class="row field-row">
                                            <div class="col-xs-12 col-sm-4">
                                                <label>First name*</label>
                                                <asp:Label ID="lblBillFname" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-xs-12 col-sm-8">
                                                <label>Last name*</label>
                                                <asp:Label ID="lblBillLname" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row field-row">
                                            <div class="col-xs-12 col-sm-4">
                                                <label>address*</label>
                                                <asp:Label ID="lblBillAddress" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-xs-12 col-sm-4">
                                                <label>city*</label>
                                                <asp:Label ID="lblBillCity" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-xs-12 col-sm-4">
                                                <label>state*</label>
                                                <asp:Label ID="lblBillState" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row field-row">
                                            <div class="col-xs-12 col-sm-4">
                                                <label>Zip*</label>
                                                <asp:Label ID="lblBillZip" runat="server"></asp:Label>
                                            </div>
                                            <div class="col-xs-12 col-sm-4">
                                                <label>phone number*</label>
                                                <asp:Label ID="lblBillPhone" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </section>

                                    <!-- ORDER SUMMARY -->
                                    <section id="your-order">
                                        <h2 class="border h1">Order Summary</h2>
                                        <asp:Repeater ID="repOrderSummary" runat="server">
                                            <ItemTemplate>
                                                <div class="row no-margin order-item">
                                                    <div class="col-xs-1 col-sm-1 no-margin">
                                                        <a href="#" class="qty"><%# Eval("Quantity")%></a>
                                                    </div>

                                                    <div class="col-xs-9 col-sm-9 ">
                                                        <div class="title"><a href="#"><%# Eval("Name")%></a></div>
                                                        <div class="brand"><%# Eval("Brand")%></div>
                                                    </div>

                                                    <div class="col-xs-2 col-sm-2 no-margin">
                                                        <div class="price"><%# "$" + string.Format("{0:n2}", Eval("Price"))%></div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </section>
                                    <!-- TODO Something should go here to break apart the item list and subtotal -->
                                    <!-- ORDER TOTALS -->
                                    <div id="total-area" class="row no-margin">
                                        <div class="col-xs-12 col-lg-4 col-lg-offset-8 no-margin-right">
                                            <div id="subtotal-holder">

                                                <!-- PRE TOTALS LIST -->
                                                <ul class="tabled-data inverse-bold no-border">
                                                    <li>
                                                        <!-- SUBTOTAL -->
                                                        <label>Order subtotal</label>
                                                        <div class="value ">
                                                            <asp:Label ID="lblSubTotal" runat="server" />
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
                                                        <label>Sales Tax</label>
                                                        <div class="value">
                                                            <asp:Label ID="lblSalesTax" runat="server" />
                                                        </div>
                                                    </li>
                                                </ul>

                                                <!-- ORDER TOTAL LIST -->
                                                <ul id="total-field" class="tabled-data inverse-bold ">
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

                                </section>
                            </div>
                        </div>

                    </div>
                </section>

            </div>

        </div>

    </section>

</asp:Content>