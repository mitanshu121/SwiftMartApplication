using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace GoMartApp
{
    public partial class Form1 : Form
    {
        DBConnect dbCon = new DBConnect();
        public static string loginname, logintype;
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbRole.SelectedIndex = 1;
           txtUsername.Text = "Mitanshu";
           txtPassword.Text = "1234";
        
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbRole.SelectedIndex > 0)
                {
                    if (txtUsername.Text == String.Empty) 
                    {
                        MessageBox.Show("Type your Valid Username   ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtUsername.Focus();
                        return;
                    }
                    if (txtPassword.Text == String.Empty)
                    {
                        MessageBox.Show("Please enter your super-secret password!  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPassword.Focus();
                        return;
                    }
                    if (cmbRole.SelectedIndex > 0 && txtUsername.Text != String.Empty && txtPassword.Text != String.Empty)
                    {
                       if (cmbRole.Text=="Admin")
                        {
                            SqlCommand cmd = new SqlCommand("select top 1 AdminID,Password,FullName from tblAdmin where AdminID=@AdminID and Password=@Password", dbCon.GetCon());
                            cmd.Parameters.AddWithValue("@AdminID", txtUsername.Text);
                            cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                            dbCon.OpenCon();
                            SqlDataAdapter da=new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if(dt.Rows.Count > 0)
                            {
                                MessageBox.Show("Login Success, Welcomme to Home Page   ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                loginname=txtUsername.Text;
                                logintype = cmbRole.Text;
                                clrValue();
                                this.Hide();
                                formMain fm = new formMain();
                                fm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Login      ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        
                        else if(cmbRole.Text == "Seller")
                        {
                            SqlCommand cmd = new SqlCommand("select top 1 SellerName,SellerPass from tblSeller where SellerName = @SellerName and SellerPass = @SellerPass", dbCon.GetCon());
                            cmd.Parameters.AddWithValue("@SellerName", txtUsername.Text);
                            cmd.Parameters.AddWithValue("@SellerPass", txtPassword.Text);
                            dbCon.OpenCon();
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show("Login Success, Welcomme to Home Page   ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                loginname = txtUsername.Text;
                                logintype = cmbRole.Text;
                                clrValue();
                                this.Hide();
                                formMain fm = new formMain();
                                fm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Login      ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter your Username/Password  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        clrValue();
                    }
                }
                else
                {
                    MessageBox.Show("Select a role to proceed    ", "Error" , MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    clrValue();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clrValue();
        }
        private void clrValue()
        {
            cmbRole.SelectedIndex = 0;
            txtUsername.Clear();
            txtPassword.Clear();
        }
    }
}
