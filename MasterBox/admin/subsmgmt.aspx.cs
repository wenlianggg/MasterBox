using MasterBox.Auth;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Admin {
    public partial class SubsMgmt : System.Web.UI.Page {

        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e) {
            // Load in coupon table
            if (!IsPostBack) { 
            CouponTable.DataSource = da.SqlGetAllCoupons();
            CouponTable.DataBind();

                // Unredeemed coupons drop down list
                Unredeemed.DataSource = da.SqlGetUnredeemedCpn();
                Unredeemed.DataTextField = "couponcode";
                Unredeemed.DataValueField = "couponcode";
                Unredeemed.DataBind();

                // User Subscription table
                UserTable.DataSource = da.SqlGetUserSubscriptions();
                UserTable.DataBind();

                if (UserTable.SelectedIndex == -1)
                {
                    MbrTypelbl.Visible = false;
                    MbrTypeTxtBox.Visible = false;
                    MbrStartlbl.Visible = false;
                    MbrExplbl.Visible = false;
                    StartDate.Visible = false;
                    EndDate.Visible = false;
                    SaveChanges.Visible = false;
                    DiscardSelection.Visible = false;
                }
            }
        }

        protected void GenerateCode(object sender, EventArgs e)
        {
            // Randomly generate coupon code 
            int maxSize = 16;
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }

            // Display coupon code
            CouponValue.Text = result.ToString();

            // Deselect selected coupons
            CouponTable.SelectedIndex = -1;
            Couponlbl.Text = "";
            InvisCpnLbl.Text = "";
        }

        protected void CouponRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Redeemed Column
                int status = int.Parse(e.Row.Cells[3].Text);
                foreach (TableCell cell in e.Row.Cells)
                {
                    switch (status)
                    {
                        case 0:
                            cell.BackColor = Color.PaleGreen;
                            e.Row.Cells[3].Text = "No";
                            break;
                        case 1:
                            cell.BackColor = Color.Tomato;
                            e.Row.Cells[3].Text = "Yes";
                            break;
                    }
                }

                // Sent Column
                int sent = int.Parse(e.Row.Cells[4].Text);
                foreach (TableCell cell in e.Row.Cells)
                {
                    switch (sent)
                    {
                        case 0:
                            e.Row.Cells[4].Text = "No";
                            break;
                        case 1:
                            e.Row.Cells[4].Text = "Yes";
                            break;
                    }
                }
            }
        }

        protected void CpnSelect(object sender, EventArgs e)
        {
            GridViewRow row = CouponTable.SelectedRow;
            Couponlbl.Text = "Selected Coupon Number: " + row.Cells[1].Text;
            InvisCpnLbl.Text = row.Cells[1].Text;
        }

        protected void UsrSelect(object sender, EventArgs e)
        {
            // Set to visible
            GridViewRow row = UserTable.SelectedRow;
            MbrTypelbl.Visible = true;
            MbrTypeTxtBox.Visible = true;
            MbrStartlbl.Visible = true;
            MbrExplbl.Visible = true;
            StartDate.Visible = true;
            EndDate.Visible = true;
            SaveChanges.Visible = true;
            DiscardSelection.Visible = true;
            
            // Set the values
            SelectedUsrlbl.Text = "Selected User: " + row.Cells[1].Text;
            MbrTypeTxtBox.Text = row.Cells[2].Text + "MB";
            StartDate.Text = da.SqlGetUserMbrStart(row.Cells[1].Text).Date.ToString("yyyy-MM-dd");
            EndDate.Text = da.SqlGetUserMbrExpiry(row.Cells[1].Text).Date.ToString("yyyy-MM-dd");
        }

        protected void AddCoupon(object sender, EventArgs e)
        {      
            if (Convert.ToInt32(Days.SelectedItem.Value) != 0)
            {
                if (DuplicateCode().Equals(false))
                {
                    // Refresh Table
                    da.SqlAddCoupon(CouponValue.Text, Convert.ToInt32(Days.SelectedItem.Value));
                    CouponTable.DataSource = da.SqlGetAllCoupons();
                    CouponTable.DataBind();

                    // Refresh Dropdown
                    Unredeemed.DataSource = da.SqlGetUnredeemedCpn();
                    Unredeemed.DataTextField = "couponcode";
                    Unredeemed.DataValueField = "couponcode";
                    Unredeemed.DataBind();

                    // Deselect selected coupons
                    CouponTable.SelectedIndex = -1;
                    Couponlbl.Text = "";
                    InvisCpnLbl.Text = "";
                }
                else
                {
                    Couponlbl.Text = "Duplicate coupon detected! Please generate a new coupon code!";
                    CouponTable.SelectedIndex = -1;
                    InvisCpnLbl.Text = "";
                }
            }else
            {
                Couponlbl.Text = "Number of days to be given has not been chosen!";
                CouponTable.SelectedIndex = -1;
                InvisCpnLbl.Text = "";
            }
        }

        protected bool DuplicateCode()
        {
            SqlDataReader rs = da.SqlGetCouponReader();
            while (rs.Read())
            {
                if (rs["couponcode"].Equals(CouponValue.Text))
                {
                    // Deselect selected coupons
                    return true;
                }

            }

            // Deselect selected coupons
            CouponTable.SelectedIndex = -1;
            Couponlbl.Text = "";
            InvisCpnLbl.Text = "";
            return false;
        }

        protected void RemoveCoupon(object sender, EventArgs e)
        {
            if (InvisCpnLbl.Text.Equals(""))
            {
                Couponlbl.Text = "You have not selected a coupon to delete!";
            }else
            {
                da.SqlDeleteCoupon(InvisCpnLbl.Text);
                CouponTable.SelectedIndex = -1;
                Couponlbl.Text = "";
                InvisCpnLbl.Text = "";
                CouponTable.DataSource = da.SqlGetAllCoupons();
                CouponTable.DataBind();
            }
        }

        protected void GetRandUser(object sender, EventArgs e)
        {
            if (!Unredeemed.SelectedItem.Value.Equals("0"))
            {
                username.Text = da.SqlGetRandomUsername();
                userlbl.Text = "Successfully sent a coupon to the user " + username.Text;
                userlbl.Attributes.Add("class", "label label-success");
            }
            else
            {
                userlbl.Text = "You have not selected a coupon!";
                userlbl.Attributes.Add("class", "label label-warning");
            }
        }

         protected void ConfirmChanges(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "PopUpModal":
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "showPopup();", true);
                    break;
                case "Confirm":
                    User usr = Auth.User.GetUser(Context.User.Identity.Name);
                    if (MBProvider.Instance.ValidateTOTP(usr.UserName, OTPValue.Text).Equals(true))
                    {
                        GridViewRow row = UserTable.SelectedRow;
                        User selusr = Auth.User.GetUser(row.Cells[1].Text);
                        selusr.MbrStart = DateTime.Parse(StartDate.Text);
                        selusr.MbrExpiry = DateTime.Parse(EndDate.Text);
                        UserTable.DataSource = da.SqlGetUserSubscriptions();
                        UserTable.DataBind();

                        SelectedUsrlbl.Text = "";
                        MbrTypelbl.Visible = false;
                        MbrTypeTxtBox.Visible = false;
                        MbrStartlbl.Visible = false;
                        MbrExplbl.Visible = false;
                        StartDate.Visible = false;
                        EndDate.Visible = false;
                        SaveChanges.Visible = false;
                        DiscardSelection.Visible = false;
                        UserTable.SelectedIndex = -1;
                    }
                    break;
            }
        }

        protected void Discard(object sender, EventArgs e)
        {
            SelectedUsrlbl.Text = "";
            MbrTypelbl.Visible = false;
            MbrTypeTxtBox.Visible = false;
            MbrStartlbl.Visible = false;
            MbrExplbl.Visible = false;
            StartDate.Visible = false;
            EndDate.Visible = false;
            SaveChanges.Visible = false;
            DiscardSelection.Visible = false;
            UserTable.SelectedIndex = -1;
        }
    }
}