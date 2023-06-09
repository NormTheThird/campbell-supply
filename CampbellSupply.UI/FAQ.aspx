<%@ Page Title="" Language="C#" MasterPageFile="~/Campbells2.Master" AutoEventWireup="true" CodeBehind="FAQ.aspx.cs" Inherits="CampbellSupply.FAQ" %>

<%@ Register Src="~/usercontrols/pageContent.ascx" TagPrefix="uc1" TagName="pageContent" %>

<asp:Content ContentPlaceHolderID="Main" runat="server">
    <main id="faq" class="inner-bottom-md">
	<div class="container">
		
		<section class="section">
			<div class="page-header">
				<h2 class="page-title">Frequently Asked Questions</h2>
    			</div>
			<div id="questions" class="panel-group panel-group-faq">
                <uc1:pageContent runat="server" id="pageContent" />
			</div><!-- /.panel-group -->
		</section><!-- /.section -->

	</div><!-- /.container -->
</main>
</asp:Content>