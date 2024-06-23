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

namespace GoMartApp
{
    public partial class AddAdmin : Form
    {
        DBConnect dbCon = new DBConnect();
        public AddAdmin()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAdminName.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Admin Name   ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAdminName.Focus();
                    return;
                }
                else if (txtAdminID.Text == String.Empty)
                {
                    MessageBox.Show("Please enter Admin ID  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAdminID.Focus();
                    return;
                }
                else if (txtPass.Text == String.Empty)
                {
                    MessageBox.Show("Please enter Password  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPass.Focus();
                    return;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("select AdminID from tblAdmin where AdminID=@ID", dbCon.GetCon());
                    cmd.Parameters.AddWithValue("@ID", txtAdminID.Text);
                    dbCon.OpenCon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("Admin ID already exist ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClear();
                    }
                    else
                    {
                        cmd = new SqlCommand("spAdminInsert", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@AdminName", txtAdminName.Text);
                        cmd.Parameters.AddWithValue("@AdminPass", txtPass.Text);
                        cmd.Parameters.AddWithValue("@AdminID", txtAdminID.Text);
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbCon.OpenCon();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Admin Inserted Successfully... ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindAdmin();
                        }
                    }
                    dbCon.CloseCon();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        
        }

        private void AddAdmin_Load(object sender, EventArgs e)
        {
            lblAdminID.Visible = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnAdd.Visible = true;
            txtAdminName.Focus();
            btnCancel.Visible = false;
            BindAdmin();
        }
        private void txtClear()
        {
            txtAdminName.Clear();
            txtPass.Clear();
            txtAdminID.Clear();
            txtAdminID.Focus();
        }
        private void BindAdmin()
        {
            SqlCommand cmd = new SqlCommand(" select * from tblAdmin", dbCon.GetCon());
            dbCon.OpenCon();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dbCon.CloseCon();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblAdminID.Text == String.Empty)
                {
                    MessageBox.Show("Please select Seller ID   ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtAdminName.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Select Name   ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAdminName.Focus();
                    return;
                }
                else if (txtPass.Text == String.Empty)
                {
                    MessageBox.Show("Please enter Password  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPass.Focus();
                    return;
                }
                else if (lblAdminID.Text == String.Empty)
                {
                    MessageBox.Show("Please enter label Admin ID  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lblAdminID.Focus();
                    return;
                }
                else
                {
                    
                    SqlCommand cmd = new SqlCommand("spAdminUpdate ", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@AdminName", txtAdminName.Text);
                        cmd.Parameters.AddWithValue("@AdminPass", txtPass.Text);
                        cmd.Parameters.AddWithValue("@AdminID", txtAdminID.Text);
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbCon.OpenCon();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Admin record Updated Successfully... ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindAdmin();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            lblAdminID.Visible = false;
                            btnCancel.Visible = false;
                            btnAdd.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("Update Failed! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        
                    }
                    dbCon.CloseCon();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_Click_1(object sender, EventArgs e)
        {
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            lblAdminID.Visible = true;
            btnAdd.Visible = false;
            btnCancel.Visible = true;

            lblAdminID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txtAdminID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            txtPass.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txtAdminName.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblAdminID.Text == String.Empty)
                {
                    MessageBox.Show("Please select Admin ID   ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (lblAdminID.Text != String.Empty)
                {
                    if (DialogResult.Yes == MessageBox.Show("Do You Want to Delete?", "Confimation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {
                        SqlCommand cmd = new SqlCommand("spAdminDelete", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@AdminID", lblAdminID.Text);
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbCon.OpenCon();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Category Deleted Successfully... ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindAdmin();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            lblAdminID.Visible = false;
                            btnAdd.Visible = true;
                            btnCancel.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Delete Failed! ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtClear();
            BindAdmin();
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            lblAdminID.Visible = false;
            btnAdd.Visible = true;
            btnCancel.Visible = false;
        }
    }
}
