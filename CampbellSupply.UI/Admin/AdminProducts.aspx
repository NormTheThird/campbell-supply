<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="AdminProducts.aspx.cs" Inherits="CampbellSupply.Admin.AdminProducts" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">
    <style>
        a:hover {
            color: red;
        }

        .RadGrid .item-style td {
            padding-top: 0;
            padding-bottom: 0;
            height: 40px;
            vertical-align: middle;
            overflow:hidden;
        }
    </style>

    <section id="category-grid">
        <div class="container">

            <!-- SIDEBAR -->
            <div class="col-xs-12 col-sm-3 no-margin sidebar narrow">
                <h2>
                    <!-- HEADING -->
                    <asp:Label ID="lblHeading" CssClass="bold text-info" runat="server" Text="Prodcuts"></asp:Label>
                </h2>
            </div>

            <!-- PRODUCTS -->
            <div class="col-xs-12 col-sm-12 col-md-12 no-margin wide">
                <section id="products">
                    <div class="grid-list-products">
                        <div class="tab-content">

                            <!-- SEARCH BUTTON -->
                            <div id="divOrderHistorySearch" runat="server" class="row">
                                <div class="col-xs-12 col-md-4">
                                    <asp:TextBox ID="txtSearch" CssClass="le-input" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-xs-12 col-md-4">
                                    <asp:Button ID="btnSearch" CssClass="btn btn-default btn-sm" Text="Search" runat="server" OnClick="btnSearch_Click" />
                                    <asp:Button ID="btnClear" CssClass="btn btn-default btn-sm" Text="Clear" runat="server" OnClick="btnClear_Click" />
                                </div>
                            </div>

                            <!-- DROP DOWNS -->
                            <div class="row m-t-35">

                                <!-- DEPARTMENT LIST -->
                                <div class="col-xs-12 col-md-4 text-center">
                                    <asp:DropDownList ID="ddlDepartment" CssClass="le-input" runat="server" AutoPostBack="true" />
                                </div>

                                <!-- CATEGORY LIST -->
                                <div class="col-xs-12 col-md-4 text-center">
                                    <asp:DropDownList ID="ddlCategory" CssClass="le-input" runat="server" AutoPostBack="true" />
                                </div>

                            </div>

                            <!-- TELERIK -->
                            <div class="row m-t-35">
                                <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
                                <telerik:RadSkinManager ID="RadSkinManager1" runat="server" Skin="Sunset" />
                                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                                    <AjaxSettings>
                                        <telerik:AjaxSetting AjaxControlID="RadGrid1">
                                            <UpdatedControls>
                                                <telerik:AjaxUpdatedControl ControlID="radGridProducts" LoadingPanelID="RadAjaxLoadingPanel1" />
                                            </UpdatedControls>
                                        </telerik:AjaxSetting>
                                    </AjaxSettings>
                                </telerik:RadAjaxManager>
                                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                                <telerik:RadFormDecorator runat="server" DecorationZoneID="demo" EnableRoundedCorners="false" DecoratedControls="All" />
                                <div>

                                    <telerik:RadGrid ID="radGridProducts"
                                        runat="server"
                                        PageSize="50"
                                        Width="100%"
                                        AllowPaging="True"
                                        AllowAutomaticUpdates="False"
                                        AllowAutomaticInserts="False"
                                        AllowAutomaticDeletes="False"
                                        AllowMultiRowSelection="true"
                                        AllowSorting="true"
                                        OnItemDataBound="radGridProducts_ItemDataBound"
                                        OnEditCommand="radGridProducts_EditCommand"
                                        OnUpdateCommand="radGridProducts_UpdateCommand"
                                        OnCancelCommand="radGridProducts_CancelCommand"
                                        OnNeedDataSource="radGridProducts_NeedDataSource">
                                        <PagerStyle Mode="NextPrevAndNumeric" />
                                        <ClientSettings AllowKeyboardNavigation="true">
                                            <Selecting AllowRowSelect="true" />
                                            <Scrolling AllowScroll="true"
                                                UseStaticHeaders="true"
                                                ScrollHeight="600px"
                                                SaveScrollPosition="true"
                                                FrozenColumnsCount="3"></Scrolling>
                                        </ClientSettings>

                                        <%-- RADGRID TABLE --%>
                                        <MasterTableView DataKeyNames="Id"
                                            AutoGenerateColumns="False"
                                            EditMode="InPlace"
                                            CommandItemDisplay="Top">

                                            <ItemStyle CssClass="item-style" />
                                            <AlternatingItemStyle CssClass="item-style" />

                                            <CommandItemSettings ShowAddNewRecordButton="false" />
                                            <Columns>

                                                <telerik:GridClientSelectColumn UniqueName="csMultipleRow" HeaderStyle-Width="30px" Visible="false" />
                                                <telerik:GridEditCommandColumn UniqueName="unEdit" HeaderStyle-Width="60px" ButtonType="ImageButton" />

                                                <telerik:GridBoundColumn DataField="Id" UniqueName="Id" Visible="false" />
                                                <telerik:GridBoundColumn DataField="ManufacturerUPC" HeaderText="UPC" UniqueName="ManufacturerUPC" SortExpression="ManufacturerUPC" HeaderStyle-Width="120px" />
                                                <telerik:GridBoundColumn DataField="Name" HeaderText="Product Name" UniqueName="Name" SortExpression="Name" HeaderStyle-Width="200px" />
                                                <telerik:GridBoundColumn DataField="DescriptionShort" HeaderText="Short Description" UniqueName="DescriptionShort" SortExpression="DescriptionShort" HeaderStyle-Width="200px" />
                                                <telerik:GridBoundColumn DataField="PartNumber" HeaderText="Part #" UniqueName="PartNumber" SortExpression="PartNumber" HeaderStyle-Width="100px" />
                                                <telerik:GridBoundColumn DataField="WebPartNumber" HeaderText="Web Part #" UniqueName="WebPartNumber" SortExpression="WebPartNumber" HeaderStyle-Width="100px" />
                                                <telerik:GridBoundColumn DataField="Sku" HeaderText="Sku" UniqueName="Sku" SortExpression="Sku" HeaderStyle-Width="120px" />
                                                <telerik:GridNumericColumn DataField="PurchasePrice" HeaderText="Price" UniqueName="PurchasePrice" SortExpression="PurchasePrice" HeaderStyle-Width="60px" NumericType="Currency" />
                                                <telerik:GridNumericColumn DataField="OverridePrice" HeaderText="Override" UniqueName="OverridePrice" SortExpression="OverridePrice" HeaderStyle-Width="60px" NumericType="Currency" />
                                                <telerik:GridNumericColumn DataField="SalePrice" HeaderText="Sale" UniqueName="SalePrice" SortExpression="SalePrice" HeaderStyle-Width="60px" NumericType="Currency" />
                                                <telerik:GridNumericColumn DataField="ShippingPrice" HeaderText="Shipping" UniqueName="ShippingPrice" SortExpression="ShippingPrice" HeaderStyle-Width="60px" NumericType="Currency" />
                                                <telerik:GridCheckBoxColumn DataField="IsFeatured" HeaderText="Featured" UniqueName="IsFeatured" SortExpression="IsFeatured" HeaderStyle-Width="60px" />
                                                <telerik:GridCheckBoxColumn DataField="IsOnSale" HeaderText="On Sale" UniqueName="IsOnSale" SortExpression="IsOnSale" HeaderStyle-Width="60px" />
                                                <telerik:GridCheckBoxColumn DataField="IsShippingValid" HeaderText="Can Ship" UniqueName="IsShippingValid" SortExpression="IsShippingValid" HeaderStyle-Width="60px" />
                                                <telerik:GridCheckBoxColumn DataField="IsActive" HeaderText="Active" UniqueName="IsActive" SortExpression="IsActive" HeaderStyle-Width="60px" />
                                                <telerik:GridCheckBoxColumn DataField="IsDeleted" HeaderText="Deleted" UniqueName="IsDeleted" SortExpression="IsDeleted" HeaderStyle-Width="60px" />
                                                <telerik:GridBoundColumn DataField="Unkonwn" HeaderText="Unkonwn" UniqueName="Unkonwn" SortExpression="Unkonwn" HeaderStyle-Width="60px" />
                                                <telerik:GridBoundColumn DataField="Color" HeaderText="Color" UniqueName="Color" SortExpression="Color" HeaderStyle-Width="60px" />
                                                <telerik:GridBoundColumn DataField="Size" HeaderText="Size" UniqueName="Size" SortExpression="Size" HeaderStyle-Width="60px" />
                                                <telerik:GridBoundColumn DataField="Brand" HeaderText="Brand" UniqueName="Brand" SortExpression="Brand" HeaderStyle-Width="60px" />
                                                <telerik:GridBoundColumn DataField="ImageUrl" HeaderText="Image" UniqueName="ImageUrl" SortExpression="ImageUrl" HeaderStyle-Width="180px" />
                                                <telerik:GridBoundColumn DataField="Status" HeaderText="Status" UniqueName="Status" SortExpression="Status" HeaderStyle-Width="60px" />
                                                <telerik:GridBoundColumn DataField="Market" HeaderText="Market" UniqueName="Market" SortExpression="Market" HeaderStyle-Width="60px" />
                                                <telerik:GridBoundColumn DataField="Group" HeaderText="Group" UniqueName="Group" SortExpression="Group" HeaderStyle-Width="60px" />
                                                <telerik:GridBoundColumn DataField="Mirror" HeaderText="Mirror" UniqueName="Mirror" SortExpression="Mirror" HeaderStyle-Width="60px" />
                                                <telerik:GridNumericColumn DataField="Weight" HeaderText="Weight" UniqueName="Weight" SortExpression="Weight" HeaderStyle-Width="60px" />
                                               <%-- 
                                                <telerik:GridDateTimeColumn DataField="DateOn" HeaderText="Date On" SortExpression="DateOn" UniqueName="DateOn" HeaderStyle-Width="80px" DataFormatString="{0:MM/dd/yyyy}" AllowSorting="true" ReadOnly="true" />
                                                <telerik:GridDateTimeColumn DataField="DateOff" HeaderText="Date Off" SortExpression="DateOff" UniqueName="DateOff" HeaderStyle-Width="80px" DataFormatString="{0:MM/dd/yyyy}" AllowSorting="true" ReadOnly="true" />
                                               --%> 
                                                <telerik:GridDateTimeColumn DataField="DateActiveChanged" HeaderText="Date Active Changed" SortExpression="DateActiveChanged" UniqueName="DateActiveChanged" HeaderStyle-Width="80px" DataFormatString="{0:MM/dd/yyyy}" AllowSorting="true" ReadOnly="true" />
                                                <telerik:GridDateTimeColumn DataField="DateDeletedChanged" HeaderText="Date Deleted Changed" SortExpression="DateDeletedChanged" UniqueName="DateDeletedChanged" HeaderStyle-Width="80px" DataFormatString="{0:MM/dd/yyyy}" AllowSorting="true" ReadOnly="true" />
                                                <telerik:GridDateTimeColumn DataField="DateCreated" HeaderText="Date Created" SortExpression="DateCreated" UniqueName="DateCreated" HeaderStyle-Width="80px" DataFormatString="{0:MM/dd/yyyy}" AllowSorting="true" ReadOnly="true" />

                                                <%--                                                <telerik:GridTemplateColumn HeaderText="MFG"
                                                    UniqueName="manufacturerName"
                                                    DataField="manufacturerName"
                                                    HeaderStyle-Width="250px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblManufacturer" runat="server" Text='<%# Eval("manufacturerName") %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="lstManufacturer" runat="server" />
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="Department"
                                                    UniqueName="department"
                                                    DataField="departmentID"
                                                    HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDepartment" runat="server" Text='<%# Eval("department") %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="lstDepartment" AutoPostBack="true" OnSelectedIndexChanged="lstDepartment_SelectedIndexChanged" runat="server" />
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="Category"
                                                    UniqueName="category"
                                                    DataField="category"
                                                    HeaderStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("category") %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="lstCategory" runat="server" />
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>--%>

                                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" HeaderStyle-Width="50px" />

                                            </Columns>

                                        </MasterTableView>
                                    </telerik:RadGrid>

                                </div>

                            </div>

                        </div>
                    </div>
                </section>
            </div>

        </div>
    </section>

</asp:Content>
