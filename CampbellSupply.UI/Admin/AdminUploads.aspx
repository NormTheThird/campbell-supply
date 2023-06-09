<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="AdminUploads.aspx.cs" Inherits="CampbellSupply.Admin.AdminUploads" %>

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

                <div class="widget">
                    <div class="body bordered">

                        <div class="category-filter">
                            <h2>
                                <asp:Label ID="lblDepartment" CssClass="bold text-info" runat="server" Text="Uploads"></asp:Label></h2>
                            <ul>
                                <!-- UPLOADS -->
                                <li>Weekly Ads</li>
                                <li>Add/Edit Products</li>
                                <li>Banners</li>
                                <li>Home Page Image Sliders</li>
                                <li>Product Images</li>
                            </ul>
                        </div>

                    </div>
                </div>
            </div>

            <!-- ========================================= CONTENT ========================================= -->

            <div class="col-xs-12 col-sm-9 no-margin wide sidebar">

                <section id="products">
                    <div class="grid-list-products">

                        <div class="tab-content">
                            <span id="spnAds" runat="server">
                                <div class="row">
                                    <div class="col-sm-12 col-lg-12">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                                                    <AjaxSettings>
                                                        <telerik:AjaxSetting AjaxControlID="Configuratorpanel1">
                                                            <UpdatedControls>
                                                                <telerik:AjaxUpdatedControl ControlID="FileExplorer1" />
                                                            </UpdatedControls>
                                                        </telerik:AjaxSetting>
                                                    </AjaxSettings>
                                                </telerik:RadAjaxManager>

                                                <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
                                                <telerik:RadFileExplorer ID="RadFileExplorer1" runat="server">
                                                    <Configuration ViewPaths="~/Utility" UploadPaths="~/Utility"
                                                        DeletePaths="~/Utility"></Configuration>
                                                </telerik:RadFileExplorer>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </span>
                            <span id="spnAddProduct" runat="server"></span>
                            <span id="spnBanner" runat="server"></span>
                            <span id="spnImages" runat="server"></span>
                        </div>
                        <!-- /.tab-content -->
                    </div>
                    <!-- /.grid-list-products -->

                </section>
            </div>
            <!-- /.col -->
            <!-- ========================================= CONTENT : END ========================================= -->

        </div>

    </section>

</asp:Content>