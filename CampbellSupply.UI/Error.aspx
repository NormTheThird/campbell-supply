<%@ Page Title="" Language="C#" MasterPageFile="~/Campbells2.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="CampbellSupply.Error" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">
    <div class="row">
        <div class="col-lg-12">
            <asp:Label ID="lblError" CssClass="center-block text-center errorNotFound" Text="Oops!" runat="server"></asp:Label>
            <asp:Label ID="lblError2" CssClass="center-block text-center errorNotFoundsm" Text="Something went wrong." runat="server"></asp:Label>
            <label class="center-block text-center bold">The page you are looking for was moved, removed, renamed or might never existed.</label>
            <br /><br /><br /><br /><br />
        </div>

    </div>

</asp:Content>