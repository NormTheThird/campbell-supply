using CampbellSupply.DataLayer.DataContracts.RequestAndResponses;
using CampbellSupply.DataLayer.DataServices;
using System;

namespace CampbellSupply
{
    public partial class PasswordResetFinalize : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                spnReset.Visible = true;
                spnInvalid.Visible = false;
                if (!IsPostBack)
                {
                    var resetId = Guid.Parse(Request["id"].ToString());
                    var response = AccountService.ValidatePasswordReset(new ValidatePasswordResetRequest { ResetId = resetId });
                    if (!response.IsSuccess) throw new ApplicationException("");
                }
            }
            catch (Exception)
            {
                spnInvalid.Visible = true;
                spnReset.Visible = false;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            lblError.Visible = false;
            var resetId = Guid.Parse(Request["id"].ToString());
            var response = AccountService.UpdatePassword(new UpdatePasswordRequest { ResetId = resetId, NewPassword = txtPassword.Text });
            if (response.IsSuccess)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Your password has been reset!, <a href=login.aspx>Click Here</a> to log into your account.";
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = response.ErrorMessage;
            }
        }
    }
}