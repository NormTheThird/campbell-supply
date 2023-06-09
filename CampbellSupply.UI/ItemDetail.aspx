<%@ Page Title="" Language="C#" MasterPageFile="~/Campbells2.Master" AutoEventWireup="true" CodeBehind="ItemDetail.aspx.cs" Inherits="CampbellSupply.ItemDetail" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">

    <!-- ITEM -->
    <div id="single-product">
        <div class="container">
            <div class="col-xs-12 col-sm-3 no-margin sidebar narrow">
                <!-- PRODUCT FILTER  -->
                <div class="widget">
                    <div class="body bordered">

                        <!-- DEPARTMENT AND CATEGORYS -->
                        <div class="category-filter">
                            <h2>
                                <asp:Label ID="lblDepartment" runat="server" /></h2>
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
            </div>
            <div class="col-xs-12 col-sm-9 no-margin wide sidebar">

                <!-- ITEM IMAGES -->
                <div class="no-margin col-xs-12 col-sm-6 col-md-5 gallery-holder">

                    <!-- MAIN IMAGE -->
                    <div id="owl-single-product">
                        <div class="single-product-gallery-item" id="slide1">
                            <a data-rel="prettyphoto" href="#">
                                <asp:Image ID="imgMain" CssClass="img-responsive img-rounded" runat="server" ImageUrl='<%# this.Product.ImageUrl %>' />
                            </a>
                        </div>
                    </div>
                </div>

                <!-- ITEM BODY -->
                <div class="no-margin col-xs-12 col-sm-7 body-holder">
                    <div class="body">

                        <!-- CURRENT RATING -->
                        <div class="star-holder inline" contenteditable="false">
                            <div class="star" contenteditable="false" data-score="<%: this.Product.RatedReviewed.Select(r => r.Rating).DefaultIfEmpty(0).Average() %>"></div>
                        </div>

                        <!-- PRODUCT NAME -->
                        <div class="title">
                            <asp:Label ID="lblName" runat="server" Text="<%# this.Product.Name %>" />
                        </div>

                        <!-- PRODUCT MANUFACTURER -->
                        <div class="brand">
                            <asp:Label ID="lblBrand" runat="server" Text="<%# this.Product.Brand %>" />
                        </div>
                        <div class="hidden">
                            <asp:Label ID="lblManufacturerUPC" runat="server" Text="<%#this.Product.ManufacturerUPC %>" />  
                        </div>

                        <!-- SOCIAL MEDIA -->
                        <div class="social-row">
                            <span class="st_facebook_hcount"></span>
                            <span class="st_twitter_hcount"></span>
                            <span class="st_pinterest_hcount"></span>
                        </div>

                        <!-- ADD TO WISHLIST -->
                        <div class="buttons-holder">
                            <a runat="server" onserverclick="AddToWishList_Click" class="btn-add-to-wishlist">Add to Wishlist</a>
                        </div>

                        <!-- PRODUCT DESCRIPTION SHORT -->
                        <!-- 
                        <div class="excerpt">
                            <asp:Label ID="lblShortDesc" runat="server" Text="<%# this.Product.DescriptionShort %>" />
                        </div>
                        -->
                        <!-- PRODUCT PRICE -->
                        <div class="prices">
                            <div class="price-current">
                                <asp:Label ID="lblPurchasePrice" runat="server" Text='<%# "$" + this.Product.PurchasePrice %>' />
                            </div>
                            <%-- <div class="price-prev">$2199.00</div>--%>
                        </div>

                        <%--<div>
                        <asp:Label ID="lblSaleMessage" CssClass="text-danger bold" Visible='<%# this.Products.onSale %>' Text="*Sale price only good in store" runat="server"></asp:Label>
                    </div>--%>
                        <asp:Label ID="lblStoreOnly" CssClass="text-danger bold" Visible="false" Text="*Available In Store Only" runat="server"></asp:Label>

                        <!-- SIZE AND COLOR -->
                        <div class="row m-t-35">
                            <div class="col-xs-12 col-md-4 col-lg-6 text-center">
                                <asp:DropDownList ID="ddlSizeAndColor" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSizeAndColor_SelectedIndexChanged" />
                            </div>
                        </div>

                        <!-- ADD TO CART -->
                        <div class="row m-t-35">
                            <div class="col-xs-12 col-md-4 text-center">
                                <asp:Button ID="Button1" runat="server" CssClass="le-button huge" Text="Add To Cart" OnClick="AddToCart_Click" />
                            </div>
                        </div>

                        <!-- EXTRA INFO -->
                        <div class="meta-row m-t-35">
                            <!-- SKU -->
                            <div class="inline">
                                <span><asp:Label ID="lblExtraID" runat="server" /></span>
                            </div>
 


                            <!-- CATEGORIES -->
                            <%--<span class="seperator">/</span>
                        <div class="inline">
                            <label>categories:</label>
                            <span><a href="#">-50% sale</a>,</span>
                            <span><a href="#">gaming</a>,</span>
                            <span><a href="#">desktop PC</a></span>
                        </div>--%>

                            <!-- TAGS -->
                            <%--<span class="seperator">/</span>
                        <div class="inline">
                            <label>tag:</label>
                            <span><a href="#">fast</a>,</span>
                            <span><a href="#">gaming</a>,</span>
                            <span><a href="#">strong</a></span>
                        </div>--%>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </div>

    <!-- TABS -->
    <section id="single-product-tab" role="tabpanel">
        <div class="container">
            <div class="tab-holder">

                <!-- TABS LINKS -->
                <ul class="nav nav-tabs simple" role="tablist">
                    <li class="active"><a href="#description" role="tab" data-toggle="tab">Description</a></li>
                    <%--<li><a href="#additional-info" data-toggle="tab">Additional Information</a></li>--%>
                    <li id="lReviews" runat="server">
                        <a href="#reviews" role="tab" data-toggle="tab">Reviews (<%: this.Product.RatedReviewed.Count %>)</a></li>
                </ul>

                <!-- TAB CONTENT -->
                <div class="tab-content">

                    <!-- DESCRIPTION TAB -->
                    <div class="tab-pane active" role="tabpanel" id="description">
                        <asp:Label ID="lblLongDesc" runat="server" Text="<%# this.Product.DescriptionLong %>" />
                    </div>

                    <!-- ADDITIONAL INFO TAB - NOT USED AT THIS TIME -->
                    <%--<div class="tab-pane" id="additional-info">
                        <ul class="tabled-data">
                            <li>
                                <label>weight</label>
                                <div class="value">7.25 kg</div>
                            </li>
                            <li>
                                <label>dimensions</label>
                                <div class="value">90x60x90 cm</div>
                            </li>
                            <li>
                                <label>size</label>
                                <div class="value">one size fits all</div>
                            </li>
                            <li>
                                <label>color</label>
                                <div class="value">white</div>
                            </li>
                            <li>
                                <label>guarantee</label>
                                <div class="value">5 years</div>
                            </li>
                        </ul>
                    </div>--%>

                    <!-- REVIEWS TAB -->
                    <div class="tab-pane" role="tabpanel" id="reviews">

                        <!-- REVIEW REPEATER -->
                        <div class="comments">
                            <asp:Repeater ID="repReview" runat="server">
                                <ItemTemplate>
                                    <div class="comment-item">
                                        <div class="row no-margin">

                                            <!-- REVIEW AVATAR -->
                                            <div class="col-lg-1 col-xs-12 col-sm-2 no-margin">
                                                <div class="avatar">
                                                    <img alt="avatar" src="assets/images/default-avatar.jpg">
                                                </div>
                                            </div>

                                            <!-- REVIEW -->
                                            <div class="col-xs-12 col-lg-11 col-sm-10 no-margin">
                                                <div class="comment-body">

                                                    <!-- REVIEW INFO -->
                                                    <div class="meta-info">

                                                        <!-- USERNAME -->
                                                        <div class="author inline">
                                                            <a href="#" class="bold"><%# Eval("FirstName")%></a>
                                                        </div>

                                                        <!-- RATING -->
                                                        <div class="star-holder inline">
                                                            <div class="star" data-score="<%# Eval("Rating")%>"></div>
                                                        </div>

                                                        <!-- DATE -->
                                                        <div class="date inline pull-right"><%# Eval("DateCreated")%></div>
                                                    </div>

                                                    <!-- REVIEW -->
                                                    <p class="comment-text"><%# Eval("Review")%></p>

                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                        <div id="divReviewLoggedIn" class="text-center" runat="server">
                            <h4 style="color: red">Must be logged in to leave a review.</h4>
                        </div>

                        <div id="divReviewThanks" class="text-center" runat="server">
                            <h4 style="color: red">Thank you for reviewing this product!</h4>
                        </div>

                        <div id="divReviewAdd" class="add-review row" runat="server">
                            <div class="col-sm-8 col-xs-12">

                                <!-- ADD REVIEW UPDATE PANEL -->
                                <asp:UpdatePanel ID="upAddReview" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>

                                        <!-- ADD REVEIW FORM -->
                                        <div class="new-review-form">
                                            <h2>Add review</h2>

                                            <!-- RATING -->
                                            <div class="field-row star-row">
                                                <label>your rating</label>
                                                <div class="star-holder">
                                                    <div id="divRating" runat="server" class="star big" data-score="0" />
                                                </div>
                                            </div>

                                            <!-- REVIEW -->
                                            <div class="field-row">
                                                <label>your review</label>
                                                <textarea id="reviewText" runat="server" rows="4" class="le-input"></textarea>
                                            </div>

                                            <!-- SUBMIT BUTTON -->
                                            <div class="buttons-holder">
                                                <asp:Button ID="btnAddReview" runat="server" CssClass="le-button huge" OnClick="AddReview_Click" Text="Submit" />
                                            </div>

                                        </div>

                                    </ContentTemplate>

                                </asp:UpdatePanel>

                            </div>
                        </div>

                    </div>

                </div>

            </div>
        </div>

    </section>

</asp:Content>
