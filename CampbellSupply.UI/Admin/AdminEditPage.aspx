<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="AdminEditPage.aspx.cs" Inherits="CampbellSupply.Admin.AdminEditPage" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">

    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>

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
                                <asp:Label ID="lblDepartment" CssClass="bold text-info" runat="server" Text="Edit Pages"></asp:Label></h2>
                            <ul>
                                <!-- PAGES TO EDIT -->
                                <asp:DropDownList ID="ddPages" CssClass="le-input" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddPages_SelectedIndexChanged">
                                    <asp:ListItem Value="About.aspx">About</asp:ListItem>
                                    <asp:ListItem Value="FAQ.aspx">FAQ</asp:ListItem>
                                    <asp:ListItem Value="Contact">Contact</asp:ListItem>
                                    <asp:ListItem Value="Locations">Contact - Locations</asp:ListItem>
                                </asp:DropDownList>
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
                            <telerik:RadEditor ID="radPages" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.axd" OnDataBinding="Page_Load" runat="server" RenderMode="Mobile" Content='<%# Eval("Content") %>'>
                                <Tools>
                                    <telerik:EditorToolGroup Tag="MainToolbar">
                                        <telerik:EditorTool Name="Print" ShortCut="CTRL+P / CMD+P"></telerik:EditorTool>
                                        <telerik:EditorSeparator />
                                        <telerik:EditorSplitButton Name="Undo">
                                        </telerik:EditorSplitButton>
                                        <telerik:EditorSplitButton Name="Redo">
                                        </telerik:EditorSplitButton>
                                        <telerik:EditorSeparator />
                                        <telerik:EditorTool Name="AjaxSpellCheck" />
                                        <telerik:EditorTool Name="FindAndReplace" ShortCut="CTRL+F / CMD+F" />
                                        <telerik:EditorTool Name="SelectAll" ShortCut="CTRL+A / CMD+A" />
                                    </telerik:EditorToolGroup>
                                    <telerik:EditorToolGroup Tag="InsertToolbar">
                                        <telerik:EditorTool Name="DocumentManager" />
                                        <telerik:EditorSeparator />
                                        <telerik:EditorSplitButton Name="ForeColor">
                                        </telerik:EditorSplitButton>
                                        <telerik:EditorSplitButton Name="BackColor">
                                        </telerik:EditorSplitButton>
                                        <telerik:EditorSeparator />
                                        <telerik:EditorDropDown Name="FontName">
                                        </telerik:EditorDropDown>
                                        <telerik:EditorDropDown Name="RealFontSize">
                                        </telerik:EditorDropDown>
                                    </telerik:EditorToolGroup>
                                </Tools>
      
<Content>
</Content>

                                <TrackChangesSettings CanAcceptTrackChanges="False"></TrackChangesSettings>
                            </telerik:RadEditor>

                        </div>
                        <!-- /.tab-content -->
                    </div>
                    <!-- /.grid-list-products -->
                    <div>
                        <asp:Button ID="btnSave" CssClass="btn btn-lg btn-danger" Text="Update" runat="server" OnClick="btnSave_Click" />
                    </div>
                </section>
            </div>
            <!-- /.col -->
            <!-- ========================================= CONTENT : END ========================================= -->

        </div>

    </section>

</asp:Content>
