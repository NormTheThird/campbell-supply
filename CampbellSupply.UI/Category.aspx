<%@ Page Title="" Language="C#" MasterPageFile="~/Campbells2.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="CampbellSupply.Category" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">
    <style> a:hover { color: red; } </style>

    <section id="category-grid">
        <div class="container">

            <!-- SIDEBAR -->
            <div class="col-xs-12 col-sm-3 no-margin sidebar narrow">

                <!-- PRODUCT FILTER  -->
                <div class="widget">
                    <div class="body bordered">

                        <!-- DEPARTMENT AND CATEGORYS -->
                        <div class="category-filter">
                            <h2>
                                <asp:Label ID="lblDepartment" runat="server" Text="Main Category"></asp:Label></h2>
                            <ul>
                                <!-- CATEGORY REPEATER -->
                                <asp:Repeater ID="repCategorys" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <asp:HyperLink ID="lnkSubCat" CssClass="textCategory" runat="server" Text='<%# Eval("Name")%>'
                                                NavigateUrl='<%# String.Format("~/Category.aspx?department={0}&category={1}", DataBinder.Eval(Container.DataItem, "Department"), DataBinder.Eval(Container.DataItem, "Name"))%>' />
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>

                    </div>
                </div>

                <!-- FEATURED PRODUCTS -->
                <div class="widget">
                    <h1 class="border">Featured Products</h1>
                    <ul class="product-list">

                        <!-- FEATURED REPEATER -->
                        <asp:Repeater ID="repFeatured" runat="server">
                            <ItemTemplate>
                                <li class="sidebar-product-list-item">
                                    <div class="row">
                                        <div class="col-xs-4 col-sm-4 no-margin">
                                            <!-- IMAGE HYPERLINK WILL LINK TO THE ITEMS DETAIL PAGE -->
                                            <a href="ItemDetail.aspx?productid=<%# Eval("ProductId")%>" class="thumb-holder">
                                                <img alt="" width="73" height="73" src="<%# "https://s3-us-west-2.amazonaws.com/campbellsp/Product/" + Eval("URL")%>" onerror="this.onerror=null;this.src='assets/images/NoImgAvail.png';"></img>
                                            </a>
                                        </div>
                                        <div class="col-xs-8 col-sm-8 no-margin">
                                            <!-- NAME WILL LINK TO THE ITEMS DETAIL PAGE -->
                                            <a href="ItemDetail.aspx?productid=<%# Eval("ProductId")%>"><%# Eval("Name")%></a>
                                            <div class="price">
                                                <div class="price-current"><%# "$" + string.Format("{0:n2}", Eval("Price"))%></div>
                                            </div>
                                        </div>
                                    </div>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>

                    </ul>
                </div>

            </div>

            <!-- CONTENT -->
            <div class="col-xs-12 col-sm-9 no-margin wide sidebar">

                <section id="products">
                    <div class="grid-list-products">

                        <div class="tab-content">
                            <div id="grid-view" class="products-grid fade tab-pane in active">

                                <!-- PRODUCTS -->
                                <div class="product-grid-holder">
                                    <div class="row no-margin">

                                        <!-- PRODUCT ITEM REPEATER -->
                                        <asp:Repeater ID="repProducts" runat="server">
                                            <ItemTemplate>
                                                <div class="col-xs-12 col-sm-4 no-margin product-item-holder hover">
                                                    <div class="product-item">
                                                        <div class="image">
                                                            <!-- IMAGE HYPERLINK WILL LINK TO THE ITEMS DETAIL PAGE -->
                                                            <a href="ItemDetail.aspx?department=<%: this.lblDepartment.Text %>&productid=<%# Eval("ProductId")%>">
                                                                <img alt="" width="150" height="150" src="<%# "https://s3-us-west-2.amazonaws.com/campbellsp/Product/" + Eval("URL")%>" onerror="this.onerror=null;this.src='assets/images/NoImgAvail.png';"></img>
                                                            </a>
                                                        </div>
                                                        <div class="body">
                                                            <div class="title">
                                                                <!-- NAME WILL LINK TO THE ITEMS DETAIL PAGE -->
                                                                <a href="ItemDetail.aspx?department=<%: this.lblDepartment.Text %>&productid=<%# Eval("ProductId")%>"><%# Eval("Name")%></a>
                                                            </div>
                                                            <div class="brand"><%# Eval("Brand")%></div>
                                                            <div class="partNumber">Part #<%# Eval("PartNumber")%></div>
                                                        </div>
                                                        <div class="prices">
                                                            <div class="price-current pull-right"><%# "$" + string.Format("{0:n2}", Eval("Price"))%></div>
                                                        </div>
                                                        <div class="hover-area">
                                                            <div class="add-cart-button">
                                                                <a href="ItemDetail.aspx?department=<%: this.lblDepartment.Text %>&productid=<%# Eval("ProductId")%>" class="le-button">view product</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </div>

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
                                                <asp:Repeater ID="repPaging" runat="server">
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

                    </div>
                </section>

            </div>

        </div>
    </section>

</asp:Content>