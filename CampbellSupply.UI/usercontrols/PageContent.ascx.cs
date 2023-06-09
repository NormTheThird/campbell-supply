using System;
using System.IO;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataServices;

namespace CampbellSupply.usercontrols
{
    public partial class PageContent : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = Request.Url.ToString();
            string fileName = Path.GetFileName(url);
            var response = PageContentService.GetPageContent(new GetPageContentRequest { PageName = fileName });
            if (response.IsSuccess) this.ltContent.Text = response.Content;
        }
    }
}