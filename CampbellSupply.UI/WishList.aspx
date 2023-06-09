<%@ Page Title="" Language="C#" MasterPageFile="~/Campbells2.Master" AutoEventWireup="true" CodeBehind="WishList.aspx.cs" Inherits="CampbellSupply.WishList" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">
    <section id="cart-page">
        <div class="container">

            <!-- WISH LIST HEADER -->
            <div class="page-header">
                <h2 class="page-title">Your Wish List</h2>
                <hr />
            </div>

            <!-- WISH LIST ITEMS -->
            <div class="col-xs-12 col-md-12 items-holder no-margin">

                <!-- WISH LIST REPEATER -->
                <asp:Repeater ID="repWishList" runat="server">
                    <ItemTemplate>
                        <div class="row no-margin cart-item">
                            <div class="col-xs-12 col-sm-2 no-margin">
                                <a href="ItemDetail.aspx?productid=<%# Eval("ProductId")%>" class="thumb-holder">
                                    <img class="lazy" alt="" width="70" height="70" src="<%# "https://s3-us-west-2.amazonaws.com/campbellsp/Product/" + Eval("URL")%>"
                                        onerror="this.onerror=null;this.src='assets/images/NoImgAvail.png';" />
                                </a>
                            </div>
                            <div class="col-xs-12 col-sm-5">
                                <div class="title"><a href="ItemDetail.aspx?productid=<%# Eval("ProductId")%>"><%# Eval("Name")%></a></div>
                                <div class="brand"><%# Eval("Brand")%></div>
                            </div>
                            <div class="col-xs-12 col-sm-2 no-margin">
                                <div class="price"><%# "$" + string.Format("{0:n2}", Eval("Price"))%></div>
                            </div>
                            <div class="col-xs-12 col-sm-3">
                                <br />
                                <a name='<%# Eval("ProductId")%>' runat="server" onserverclick="AddToCart_Click" class="le-button small">Add To Cart</a>
                                <a name='<%# Eval("ProductId")%>' runat="server" onserverclick="DeleteFromWishList_Click" class="close-btn"></a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

            </div>

        </div>
    </section>
    <div class="space100"></div>--%>
</asp:Content>