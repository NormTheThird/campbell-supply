using System;
using CampbellSupply.Models;
using System.IO;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;

namespace CampbellSupply.Admin
{
    public partial class AdminEditPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = Request.Url.ToString();
            string fileName = Path.GetFileName(url);


            if (!IsPostBack)
            {
                var response = PageContentService.GetPageContent(new GetPageContentRequest { PageName = ddPages.SelectedValue });
                if (response.IsSuccess) this.radPages.Content = response.Content;
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var response = PageContentService.SavePageContent(new SavePageContentRequest { PageName = ddPages.SelectedValue, Content = this.radPages.Content});
        }
        protected void ddPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            var response = PageContentService.GetPageContent(new GetPageContentRequest { PageName = ddPages.SelectedValue });
            if (response.IsSuccess) this.radPages.Content = response.Content;
        }
    }
}