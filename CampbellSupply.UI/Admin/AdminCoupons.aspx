<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="AdminCoupons.aspx.cs" Inherits="CampbellSupply.Admin.AdminCoupons" %>

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
                    <asp:Label ID="lblDepartment" CssClass="bold text-info" runat="server" Text="Manage Coupons"></asp:Label>
                </h2>
            </div>

            <!-- COUPONS -->
            <div class="col-xs-12 col-sm-12 col-md-12 no-margin wide">
                <section id="products">
                    <div class="grid-list-products">
                        <div class="tab-content">
                            <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
                            <telerik:RadSkinManager ID="RadSkinManager1" runat="server" Skin="Sunset" />
                            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                                <AjaxSettings>
                                    <telerik:AjaxSetting AjaxControlID="radGridCoupons">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="radGridCoupons" LoadingPanelID="RadAjaxLoadingPanel1" />
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                </AjaxSettings>
                            </telerik:RadAjaxManager>
                            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                            <telerik:RadFormDecorator runat="server" DecorationZoneID="demo" EnableRoundedCorners="false" DecoratedControls="All" />
                        </div>
                        <div>
                            <telerik:RadGrid ID="radGridCoupons"
                                runat="server"
                                AllowPaging="True"
                                AllowAutomaticUpdates="False"
                                AllowAutomaticInserts="False"
                                AllowAutomaticDeletes="False"
                                AllowSorting="true"
                                OnUpdateCommand="rpReviews_UpdateCommand"
                                OnNeedDataSource="rpReviews_NeedDataSource"
                                OnInsertCommand="RadGrid1_InsertCommand"
                                OnItemDataBound="RadGrid1_ItemDataBound">
                                <PagerStyle Mode="NextPrevAndNumeric" />
                                <ClientSettings AllowKeyboardNavigation="true">
                                    <Selecting AllowRowSelect="true" />
                                    <Scrolling AllowScroll="true"
                                        UseStaticHeaders="True"
                                        SaveScrollPosition="true" />
                                </ClientSettings>
                                <MasterTableView DataKeyNames="Id"
                                    AutoGenerateColumns="false"
                                    EditMode="InPlace"
                                    CommandItemDisplay="Top">

                                    <Columns>

                                        <telerik:GridEditCommandColumn ButtonType="ImageButton" HeaderStyle-Width="70px" UniqueName="EditCommandColumn" />

                                        <telerik:GridBoundColumn DataField="Id" Visible="false" UniqueName="Id" />
                                        <telerik:GridBoundColumn DataField="Code" HeaderText="Coupon Code" SortExpression="Code" UniqueName="Code" HeaderStyle-Width="120px" />
                                        <telerik:GridBoundColumn DataField="Description" HeaderText="Coupon Description" SortExpression="Description" UniqueName="Description" HeaderStyle-Width="300px" />
                                        <telerik:GridBoundColumn DataField="Value" HeaderText="Coupon Value" SortExpression="Value" UniqueName="Value" HeaderStyle-Width="60px" />
                                        <telerik:GridCheckBoxColumn DataField="IsPercent" HeaderText="Percentage" UniqueName="IsPercent" AllowSorting="true" HeaderStyle-Width="100px" />
                                        <telerik:GridCheckBoxColumn DataField="IsAmount" HeaderText="Amount" UniqueName="IsAmount" AllowSorting="true" HeaderStyle-Width="100px" />
                                        <telerik:GridCheckBoxColumn DataField="IsOneTime" HeaderText="Single Use" UniqueName="IsOneTime" AllowSorting="true" HeaderStyle-Width="100px" />
                                        <telerik:GridCheckBoxColumn DataField="IsActive" HeaderText="Active" UniqueName="IsActive" AllowSorting="true" HeaderStyle-Width="100px" />
                                        <telerik:GridDateTimeColumn DataField="DateActiveChanged" HeaderText="Date Active Changed" SortExpression="DateActiveChanged" UniqueName="DateActiveChanged" AllowSorting="true" HeaderStyle-Width="80px" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" />
                                        <telerik:GridDateTimeColumn DataField="DateCreated" HeaderText="Date Created" SortExpression="DateCreated" UniqueName="DateCreated" AllowSorting="true" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="80px" ReadOnly="true" />

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
