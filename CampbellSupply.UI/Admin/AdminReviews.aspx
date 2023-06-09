<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="AdminReviews.aspx.cs" Inherits="CampbellSupply.Admin.AdminReviews" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">

    <style>
        a:hover {
            color: red;
        }
    </style>
    <section id="category-grid">
        <div class="container">

            <!-- SIDEBAR -->
            <div class="col-xs-12 col-sm-3 no-margin sidebar narrow">
                <h2>
                    <!-- HEADING -->
                    <asp:Label ID="lblHeading" CssClass="bold text-info" runat="server" Text="Customer Reviews"></asp:Label>
                </h2>
            </div>

            <!-- RATINGS AND REVIEWS -->
            <div class="col-xs-12 col-sm-12 col-md-12 no-margin wide">
                <section id="products">
                    <div class="grid-list-products">
                        <div class="tab-content">
                            <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
                            <telerik:RadSkinManager ID="RadSkinManager1" runat="server" Skin="Sunset" />
                            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                                <AjaxSettings>
                                    <telerik:AjaxSetting AjaxControlID="RadGrid1">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="rpReviews" LoadingPanelID="RadAjaxLoadingPanel1" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                </AjaxSettings>
                            </telerik:RadAjaxManager>
                            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                            <telerik:RadFormDecorator runat="server" DecorationZoneID="demo" EnableRoundedCorners="false" DecoratedControls="All" />
                        </div>
                        <div>
                            <telerik:RadGrid ID="rpReviews"
                                runat="server"
                                AllowPaging="True"
                                AllowAutomaticUpdates="False"
                                AllowAutomaticInserts="False"
                                AllowAutomaticDeletes="False"
                                AllowMultiRowSelection="true"
                                AllowSorting="true"
                                OnUpdateCommand="rpReviews_UpdateCommand"
                                OnNeedDataSource="rpReviews_NeedDataSource"
                                OnItemDataBound="rpReviews_ItemDataBound">
                                <PagerStyle Mode="NextPrevAndNumeric" />
                                <ClientSettings AllowKeyboardNavigation="true">
                                    <Selecting AllowRowSelect="true" />
                                    <Scrolling AllowScroll="true"
                                        UseStaticHeaders="true"
                                        SaveScrollPosition="true" />
                                </ClientSettings>
                                <MasterTableView DataKeyNames="Id"
                                    AutoGenerateColumns="false"
                                    EditMode="InPlace"
                                    CommandItemDisplay="Top">

                                    <CommandItemSettings ShowAddNewRecordButton="false" />
                                    <Columns>

                                        <%-- EDIT COLUMN --%>
                                        <telerik:GridEditCommandColumn ButtonType="ImageButton"
                                            HeaderStyle-Width="70px"
                                            UniqueName="EditCommandColumn">
                                        </telerik:GridEditCommandColumn>

                                        <%-- ID COLUMN --%>
                                        <telerik:GridBoundColumn
                                            DataField="Id"
                                            Visible="false"
                                            UniqueName="Id">
                                        </telerik:GridBoundColumn>

                                        <%-- CUSTOMER NAME COLUMN --%>
                                        <telerik:GridBoundColumn DataField="FirstName"
                                            HeaderText="Reviewer Name"
                                            HeaderStyle-Width="150px"
                                            SortExpression="FirstName"
                                            UniqueName="FirstName"
                                            ReadOnly="true">
                                        </telerik:GridBoundColumn>

                                        <%-- PRODUCT UPC --%>
                                        <telerik:GridBoundColumn DataField="ProductUpc"
                                            HeaderText="Product Upc"
                                            HeaderStyle-Width="130px"
                                            SortExpression="ProductUpc"
                                            UniqueName="ProductUpc"
                                            ReadOnly="true">
                                        </telerik:GridBoundColumn>

                                        <%-- PRODUCT NAME COLUMN --%>
                                        <telerik:GridBoundColumn DataField="ProductName"
                                            HeaderText="Product Name"
                                            HeaderStyle-Width="200px"
                                            SortExpression="productName"
                                            UniqueName="productName"
                                            ReadOnly="true">
                                        </telerik:GridBoundColumn>

                                        <%-- RATING COLUMN --%>
                                        <telerik:GridBoundColumn DataField="Rating"
                                            HeaderText="Rating"
                                            HeaderStyle-Width="60px"
                                            SortExpression="Rating"
                                            UniqueName="Rating"
                                            ReadOnly="true">
                                        </telerik:GridBoundColumn>

                                        <%-- REVIEW COLUMN --%>
                                        <telerik:GridBoundColumn DataField="Review"
                                            HeaderText="Review"
                                            SortExpression="Review"
                                            UniqueName="Review"
                                            ReadOnly="true">
                                        </telerik:GridBoundColumn>

                                        <%-- IS ACTIVE COLUMN --%>
                                        <telerik:GridCheckBoxColumn
                                            DataField="IsActive"
                                            HeaderText="Active"
                                            HeaderStyle-Width="80px"
                                            UniqueName="IsActive"
                                            AllowSorting="true">
                                        </telerik:GridCheckBoxColumn>

                                        <%-- IS AUTHORIZED COLUMN --%>
                                        <telerik:GridCheckBoxColumn
                                            DataField="IsAuthorized"
                                            HeaderText="Authorized"
                                            HeaderStyle-Width="80px"
                                            UniqueName="IsAuthorized"
                                            AllowSorting="true">
                                        </telerik:GridCheckBoxColumn>

                                        <%-- DATE ACTIVE CHANGED COLUMN --%>
                                        <telerik:GridDateTimeColumn
                                            DataField="DateActiveChanged"
                                            HeaderText="Date Active Changed"
                                            DataFormatString="{0:MM/dd/yyyy}"
                                            HeaderStyle-Width="80px"
                                            SortExpression="DateActiveChanged"
                                            UniqueName="DateActiveChanged"
                                            AllowSorting="true"
                                            ReadOnly="true">
                                        </telerik:GridDateTimeColumn>

                                        <%-- DATE AUTHORIZED CHANGED COLUMN --%>
                                        <telerik:GridDateTimeColumn
                                            DataField="DateAuthorizedChanged"
                                            HeaderText="Date Authorized Changed"
                                            DataFormatString="{0:MM/dd/yyyy}"
                                            HeaderStyle-Width="80px"
                                            SortExpression="DateAuthorizedChanged"
                                            UniqueName="DateAuthorizedChanged"
                                            AllowSorting="true"
                                            ReadOnly="true">
                                        </telerik:GridDateTimeColumn>

                                        <%-- DATE CREATED COLUMN --%>
                                        <telerik:GridDateTimeColumn
                                            DataField="DateCreated"
                                            HeaderText="Date Created"
                                            DataFormatString="{0:MM/dd/yyyy}"
                                            HeaderStyle-Width="80px"
                                            SortExpression="DateCreated"
                                            UniqueName="DateCreated"
                                            AllowSorting="true"
                                            ReadOnly="true">
                                        </telerik:GridDateTimeColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>

                        </div>
                    </div>
                </section>
            </div>

        </div>
    </section>

</asp:Content>
