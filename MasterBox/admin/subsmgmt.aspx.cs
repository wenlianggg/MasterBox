using MasterBox.Auth;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;

namespace MasterBox.Admin {
    public partial class SubsMgmt : System.Web.UI.Page {

        DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e) {
            // Load in coupon table
            if (!IsPostBack) { 
            CouponTable.DataSource = da.SqlGetAllCoupons();
            CouponTable.DataBind();
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
        }

        protected void CouponRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
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
            }
        }

        protected void Selected(object sender, EventArgs e)
        {
            GridViewRow row = CouponTable.SelectedRow;
            Couponlbl.Text = "Selected Coupon Number: " + row.Cells[1].Text;
        }

        protected void AddCoupon(object sender, EventArgs e)
        {      
            if (Convert.ToInt32(Days.SelectedItem.Value) != 0)
            {
                if (DuplicateCode().Equals(false))
                {
                    da.SqlAddCoupon(CouponValue.Text, Convert.ToInt32(Days.SelectedItem.Value));
                    CouponTable.DataSource = da.SqlGetAllCoupons();
                    CouponTable.DataBind();
                }
                else
                {
                    Couponlbl.Text = "Duplicate coupon detected! Please generate a new coupon code!";
                }
            }else
            {
                Couponlbl.Text = "Number of days to be given has not been chosen!";
            }
        }

        protected bool DuplicateCode()
        {
            SqlDataReader rs = da.SqlGetCouponReader();
            while (rs.Read())
            {
                if (rs["couponcode"].Equals(CouponValue.Text))
                {
                    return true;
                }

            }
            return false;
        }
    }
}