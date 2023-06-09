<%@ Page Title="" Language="C#" MasterPageFile="~/Campbells2.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="CampbellSupply.About" %>

<%@ Register Src="~/usercontrols/pageContent.ascx" TagPrefix="uc1" TagName="pageContent" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">

    <div class="container inner-top-xs inner-bottom-sm">

        <div class="row">
            <div class="col-xs-12 col-md-10 col-lg-10 col-sm-6">

                <section id="who-we-are" class="section m-t-0">
                    <uc1:pageContent runat="server" ID="pageContent" />
                </section>

            </div>
            <div class="col-xs-12 col-md-2 col-lg-2 col-sm-6">
                <section>
                    <img class="pull-right" src="assets/images/Campbell.GIF" />
                </section>
            </div>

        </div>

    </div>
</asp:Content>