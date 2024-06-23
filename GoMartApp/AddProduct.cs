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
    public partial class AddProduct : Form
    {
        DBConnect dbCon = new DBConnect();
        public AddProduct()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void AddProduct_Load(object sender, EventArgs e)
        {
            lblProductID.Visible = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            btnAdd.Visible = true;
            btnCancel.Visible = false;
            BindProduct();
            BindProductList();
            Searchby_Category();
        }

        private void BindProduct()
        {
            SqlCommand cmd = new SqlCommand("spGetCategory", dbCon.GetCon());
            cmd.CommandType = CommandType.StoredProcedure;
            dbCon.OpenCon();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbCategory.DataSource = dt;
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CatID";
            dbCon.CloseCon();
        }
        private void Searchby_Category()
        {
            SqlCommand cmd = new SqlCommand("spGetCategory", dbCon.GetCon());
            cmd.CommandType = CommandType.StoredProcedure;
            dbCon.OpenCon();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbSearch.DataSource = dt;
            cmbSearch.DisplayMember = "CategoryName";
            cmbSearch.ValueMember = "CatID";
            dbCon.CloseCon();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtProdName.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Product Name   ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtProdName.Focus();
                    return;
                }
                else if (Convert.ToInt32(txtPrice.Text) < 0 || txtPrice.Text == String.Empty)
                {
                    MessageBox.Show("Please enter Price  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
                    return;
                }
                else if (Convert.ToInt32(txtQuantity.Text) < 0 || txtQuantity.Text == String.Empty)
                {
                    MessageBox.Show("Please enter Quantity  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuantity.Focus();
                    return;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("spCheckDuplicateProduct", dbCon.GetCon());
                    cmd.Parameters.AddWithValue("@ProdName", txtProdName.Text);
                    cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                    cmd.CommandType = CommandType.StoredProcedure;
                    dbCon.OpenCon();
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        MessageBox.Show("Product Name already exist ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtClear();
                    }
                    else
                    {
                        cmd = new SqlCommand("spInsertProduct", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@ProdName", txtProdName.Text);
                        cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@ProdPrice", Convert.ToDecimal(txtPrice.Text));
                        cmd.Parameters.AddWithValue("@ProdQty", Convert.ToInt32(txtQuantity.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbCon.OpenCon();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Product Inserted Successfully... ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindProductList();
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

        private void BindProductList()
        {
            try 
            { 
            SqlCommand cmd = new SqlCommand("spgetAllProductList", dbCon.GetCon());
            cmd.CommandType = CommandType.StoredProcedure;
            dbCon.OpenCon();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dbCon.CloseCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void txtClear()
        {
            txtProdName.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtProdName.Text == String.Empty)
                {
                    MessageBox.Show("Please Enter Product Name   ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtProdName.Focus();
                    return;
                }
                else if (lblProductID.Text == "")
                {
                    MessageBox.Show("Please Select Product ID ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtProdName.Focus();
                    return;
                }
                else if (Convert.ToDouble(txtPrice.Text) < 0.0 || txtPrice.Text == String.Empty )
                {
                    MessageBox.Show("Please enter valid Price  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
                    return;
                }
                else if (Convert.ToInt32(txtQuantity.Text) < 0 || txtQuantity.Text == String.Empty )
                {
                    MessageBox.Show("Please enter valid Quantity  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuantity.Focus();
                    return;
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("spUpdateProduct", dbCon.GetCon());
                    
                    cmd.Parameters.AddWithValue("@ProdName", txtProdName.Text);
                    cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                    cmd.Parameters.AddWithValue("@ProdPrice", decimal.Parse(txtPrice.Text));
                    cmd.Parameters.AddWithValue("@ProdQty", int.Parse(txtQuantity.Text));
                    cmd.Parameters.AddWithValue("@ProdID", Convert.ToInt32(lblProductID.Text));

                    cmd.CommandType = CommandType.StoredProcedure;
                    dbCon.OpenCon();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        MessageBox.Show("Product Updated Successfully... ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtClear();
                        BindProductList();
                        lblProductID.Visible = false;
                        btnAdd.Visible = true;
                        btnUpdate.Visible = false;
                        btnDelete.Visible = false;  
                        btnCancel.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Updation Failed!  ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                    dbCon.CloseCon();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            btnUpdate.Visible = true;
            btnDelete.Visible = true;
            lblProductID.Visible = true;
            btnAdd.Visible = false;
            btnCancel.Visible = true;

            lblProductID.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txtProdName.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            cmbCategory.SelectedValue = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txtPrice.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            txtQuantity.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblProductID.Text == String.Empty)
                {
                    MessageBox.Show("Please select Product ID   ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (lblProductID.Text != String.Empty)
                {
                    if (DialogResult.Yes == MessageBox.Show("Do You Want to Delete?", "Confimation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                    {
                        SqlCommand cmd = new SqlCommand("spDeleteProduct", dbCon.GetCon());
                        cmd.Parameters.AddWithValue("@ProdID", Convert.ToInt32(lblProductID.Text));
                        cmd.CommandType = CommandType.StoredProcedure;
                        dbCon.OpenCon();
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                        {
                            MessageBox.Show("Product Deleted Successfully... ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtClear();
                            BindProductList();
                            btnUpdate.Visible = false;
                            btnDelete.Visible = false;
                            lblProductID.Visible = false;
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void Searched_ProductList()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spgetAllProductList_SearchByCat", dbCon.GetCon());
                cmd.Parameters.AddWithValue("@ProdCatID", cmbSearch.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                dbCon.OpenCon();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                dbCon.CloseCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Searched_ProductList();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindProductList();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtClear();
            BindProductList();
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            lblProductID.Visible = false;
            btnAdd.Visible = true;
            btnCancel.Visible = false;
        }
    }
}
