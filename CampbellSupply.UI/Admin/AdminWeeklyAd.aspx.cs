using System;
using CampbellSupply.Models;
using System.IO;
using CampbellSupply.DataLayer.DataServices;
using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataContracts.Models;
using CampbellSupply.Common.Enums;
using System.Data;
using System.Web.UI.WebControls;

namespace CampbellSupply.Admin
{
    public partial class AdminWeeklyAd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            this.WeeklyAdForm.Visible = false;
            this.btnSave.Visible = false;

            if (IsPostBack)
            {
                this.WeeklyAdForm.Visible = true;
                this.btnSave.Visible = true;
            }
            else
            {

            }

            if (ddWeeklyAd.Items.Count == 0) updateDropDown();

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var ad = new WeeklyAdModel();
                ad.Id = Guid.Parse(this.ddWeeklyAd.SelectedValue);
                ad.Title = this.title.Text;
                ad.FileName = this.WeeklyAdPdf.FileName;
                ad.FileStorageType = StorageType.PDF;
                ad.File = this.WeeklyAdPdf.FileBytes;
                ad.ImageName = this.WeeklyAdImage.FileName;
                ad.ImageStorageType = StorageType.Image;
                ad.Image = this.WeeklyAdImage.FileBytes;
                ad.EffectiveDate = this.EffectiveDate.SelectedDate;
                ad.EndDate = this.EndDate.SelectedDate;

                var response = PageContentService.SaveWeeklyAd(new SaveWeeklyAdRequest { WeeklyAd = ad });  
                if (response.IsSuccess)
                {
                    this.HideWaitContainer();
                    updateDropDown();
                    emptyForm();
                    ddWeeklyAd_SelectedIndexChanged(0, System.EventArgs.Empty);
                    this.WeeklyAdForm.Visible = false;
                    this.btnSave.Visible = false;
                }
            }
            catch (Exception)
            {
                this.HideWaitContainer();
            }

        }
        protected void ddWeeklyAd_SelectedIndexChanged(object sender, EventArgs e)
        {
            var adResponse = PageContentService.GetWeeklyAd(new GetWeeklyAdRequest { Id = Guid.Parse(ddWeeklyAd.SelectedValue) });
            if (adResponse.IsSuccess && adResponse.WeeklyAds.Count > 0)
            {
                this.title.Text = adResponse.WeeklyAds[0].Title;
                if (adResponse.WeeklyAds[0].EffectiveDate != null) this.EffectiveDate.SelectedDate = (DateTime)adResponse.WeeklyAds[0].EffectiveDate;
                if (adResponse.WeeklyAds[0].EndDate != null) this.EndDate.SelectedDate = adResponse.WeeklyAds[0].EndDate.Value;
                this.imgWeeklyAdImage.ImageUrl = adResponse.WeeklyAds[0].ImageStoragePath;
                //WeeklyAdImage.Visible = true;
                this.linkWeeklyAdPdf.CssClass = "show";
                this.linkWeeklyAdPdf.NavigateUrl = adResponse.WeeklyAds[0].FileStoragePath;

                this.SaveSuccess.CssClass = "hidden";
                this.WeeklyAdForm.Visible = true;
                this.btnSave.Visible = true;

            }
            if (ddWeeklyAd.SelectedItem.Text.Equals("Please Select", StringComparison.CurrentCultureIgnoreCase))
            {
                this.WeeklyAdForm.Visible = false;
                this.btnSave.Visible = false;
            }
            else if (ddWeeklyAd.SelectedItem.Text.Equals("New Ad", StringComparison.CurrentCulture))
            {
                emptyForm();
            }
        }
        protected void btnNewAd_Click(object sender, EventArgs e)
        {
            ddWeeklyAd.Items[0].Text = "New Ad";
            emptyForm();
            this.ddWeeklyAd.SelectedIndex = 0;
            this.WeeklyAdForm.Visible = true;
            this.btnSave.Visible = true;
        }
        protected void updateDropDown()
        {
            ddWeeklyAd.Items.Clear();

            GetWeeklyAdRequest WeeklyAdRequest = new GetWeeklyAdRequest();
            GetWeeklyAdResponse response = PageContentService.GetWeeklyAds(WeeklyAdRequest);
            response.WeeklyAds.Sort((d, nd) => nd.EffectiveDate.CompareTo(d.EffectiveDate));

            DataTable dtAdTable = new DataTable();
            dtAdTable.Columns.Add("Text", typeof(string));
            dtAdTable.Columns.Add("Value", typeof(string));

            dtAdTable.Rows.Add("Please Select", Guid.Empty.ToString());

            response.WeeklyAds.ForEach(wa => dtAdTable.Rows.Add(
                    wa.EffectiveDate.ToShortDateString() + " " + wa.Title, wa.Id));

            ddWeeklyAd.DataTextField = "Text";
            ddWeeklyAd.DataValueField = "Value";
            ddWeeklyAd.DataSource = dtAdTable;
            ddWeeklyAd.DataBind();
        }

        protected void emptyForm()
        {
            this.title.Text = "";
            this.EffectiveDate.SelectedDate = DateTime.MinValue;
            this.EndDate.SelectedDate = DateTime.MinValue;
            this.WeeklyAdImage = null;
            this.WeeklyAdPdf = null;
            this.imgWeeklyAdImage.ImageUrl = null;
            this.linkWeeklyAdPdf.CssClass = "hidden";

        }

        /// <summary>
        /// Hide the wait container
        /// </summary>
        private void HideWaitContainer()
        {
            try
            {
                var waitDiv = (System.Web.UI.HtmlControls.HtmlGenericControl)Master.FindControl("a_wait_container");
                if (waitDiv != null) waitDiv.Style.Add("display", "none");
            }
            catch (Exception) { }
        }
    }
}