using System;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;

namespace CampbellSupply.usercontrols
{
    public partial class PageContentLocation : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var response = PageContentService.GetPageContent(new GetPageContentRequest { PageName = "locations" });
            if (response .IsSuccess) this.ltContentLocations.Text = response.Content;
        }
    }
}