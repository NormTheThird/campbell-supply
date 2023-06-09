<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="AdminWeeklyAd.aspx.cs" Inherits="CampbellSupply.Admin.AdminWeeklyAd" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>

        </Scripts>
    </telerik:RadScriptManager>

    <script type="text/javascript">
        jQuery(function ($) {
            $(".submitButton").click(function () {
                    $(".wait-container").show();
            });
        });
    </script>

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
                                <asp:Label ID="lblDepartment" CssClass="bold text-info" runat="server" Text="Edit Weekly Ads"></asp:Label></h2>
                            <ul>
                                <!-- PAGES TO EDIT -->

                                <!-- TODO Populate dropdowns of weekly ads for editting -->
                                <asp:DropDownList ID="ddWeeklyAd" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddWeeklyAd_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ul>
                        </div>

                    </div>
                </div>
            </div>

            <!-- ========================================= CONTENT ========================================= -->

            <div class="col-xs-12 col-sm-9 no-margin wide sidebar">


                <asp:Button ID="btnNewAd" CssClass="le-button" Text="New Ad +" runat="server" OnClick="btnNewAd_Click" />
                <asp:Button ID="btnSave" CssClass="le-button submitButton" Text="Save" runat="server" OnClick="btnSave_Click" />
                <br />

                <section id="WeeklyAdForm" runat="server">
                    <div class="grid-list-products">
                        <div class="">
                            <div class="form-group">
                                <label class="">Title: </label>
                                <asp:TextBox runat="server" class="form-control" ID="title" />
                            </div>
                            <div class="form-group inline">
                                <label>Effective Date:</label>
                                <asp:Calendar runat="server" ID="EffectiveDate"></asp:Calendar>
                            </div>
                            <div class="form-group inline">
                                <label>End Date</label>
                                <asp:Calendar runat="server" ID="EndDate"></asp:Calendar>
                            </div>
                        </div>
                        <div class="">
                            <div class="form-group">
                                <div class="col-md-6">
                                    <label>Ad Image:</label>
                                    <asp:FileUpload runat="server" class="form-control" ID="WeeklyAdImage" />
                                </div>
                                <div class="col-md-6">
                                    <label>Ad PDF:</label>
                                    <asp:FileUpload runat="server" class="form-control" ID="WeeklyAdPdf" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-6">
                                    <br />
                                    <asp:Image runat="server" ID="imgWeeklyAdImage" Height="100px" />
                                </div>
                                <div class="col-md-6">
                                    <br />
                                    <asp:HyperLink runat="server" ID="linkWeeklyAdPdf" class="hidden show">
                                        <i class="fa fa-file-pdf-o fa-4x" aria-hidden="true" ></i>
                                    </asp:HyperLink>
                                </div>
                            </div>

                        </div>
                    </div>

                </section>

                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <asp:Label runat="server" ID="SaveSuccess" class="hidden">Weekly Ad Saved Successfully</asp:Label>
            </div>
            <div class="col-md-12">
                <div class="container">
                </div>
            </div>
            <!-- /.col -->
            <!-- ========================================= CONTENT : END ========================================= -->
            <div class="tab-content">
                <div class="form-group">
                </div>
            </div>
        </div>

    </section>

</asp:Content>
