using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace GoMartApp
{
    public partial class SellingForm : Form
    {
        DBConnect dbCon = new DBConnect();
        private PrintDocument printDocument;
        private PrintDialog printDialog;


        public SellingForm()
        {
            InitializeComponent();

            printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);

            printDialog = new PrintDialog
            {
                Document = printDocument
            };
        }

        double GrandTotal = 0.0; 
        int n=0;
        private void SellingForm_Load(object sender, EventArgs e)
        {
            BindProduct();
            lblDate.Text =DateTime.Now.ToShortDateString();
            BindBillList();
        }
        private void BindProduct()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void Searched_ProductList()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spgetAllProductList_SearchByCat", dbCon.GetCon());
                cmd.Parameters.AddWithValue("@ProdCatID", cmbCategory.SelectedValue);
                cmd.CommandType = CommandType.StoredProcedure;
                dbCon.OpenCon();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2_Product.DataSource = dt;
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

        private void dataGridView2_Product_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void dataGridView2_Product_Click(object sender, EventArgs e)
        {
            try
            {
                txtProductID.Clear();
                txtProductID.Text = dataGridView2_Product.SelectedRows[0].Cells[0].Value.ToString();
                txtProdName.Clear();
                txtProdName.Text = dataGridView2_Product.SelectedRows[0].Cells[1].Value.ToString();
                txtPrice.Clear();
                txtPrice.Text = dataGridView2_Product.SelectedRows[0].Cells[4].Value.ToString();
                //txtQuantity.Text = dataGridView2_Product.SelectedRows[0].Cells[5].Value.ToString();
                txtQuantity.Clear();
                txtQuantity.Focus();
                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPrice.Text == "" || txtQuantity.Text == "")
                {
                    MessageBox.Show("Enter Valid Quantity or Price", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {
                    double Total = Convert.ToDouble(txtPrice.Text) * Convert.ToInt32(txtQuantity.Text);
                    DataGridViewRow addRow = new DataGridViewRow();
                    addRow.CreateCells(dataGridView1_Order);
                    //
                    addRow.Cells[0].Value = ++n;
                    addRow.Cells[1].Value = txtProdName.Text;
                    addRow.Cells[2].Value = txtPrice.Text;
                    addRow.Cells[3].Value = txtQuantity.Text;
                    addRow.Cells[4].Value = Total;
                    dataGridView1_Order.Rows.Add(addRow);
                    GrandTotal += Total;
                    lblGrandTotal.Text = "Rs." + GrandTotal;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnAddBilll_Details_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtBillNo.Text=="")
                {
                    MessageBox.Show("Enter Bill No.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else 
                {
                    SqlCommand cmd = new SqlCommand("spInsertBill", dbCon.GetCon());
                    cmd.Parameters.AddWithValue("@Bill_ID", txtBillNo.Text);
                    cmd.Parameters.AddWithValue("@SellerID", Form1.loginname);
                    cmd.Parameters.AddWithValue("@SellDate", lblDate.Text);
                    cmd.Parameters.AddWithValue("@TotalAmt", GrandTotal);
                    cmd.CommandType = CommandType.StoredProcedure;
                    dbCon.OpenCon();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        BindBillList();
                        MessageBox.Show("Bill Inserted Successfully... ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    }
                    dbCon.CloseCon();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void BindBillList()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spGetBillList", dbCon.GetCon());
                cmd.CommandType = CommandType.StoredProcedure;
                dbCon.OpenCon();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView2.DataSource = dt;
                dbCon.CloseCon();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            
            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            Font regularFont = new Font("Arial", 12, FontStyle.Regular);
            Brush brush = Brushes.Black;

           
            Rectangle printArea = new Rectangle(e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Width, e.MarginBounds.Height);

            
            string sellerId = Form1.loginname; 
            string sellDate = lblDate.Text; 
            

            
            e.Graphics.DrawString("Bill", titleFont, brush, new PointF(e.MarginBounds.Left, e.MarginBounds.Top));
           // e.Graphics.DrawString($"Bill ID: {billId}", regularFont, brush, new PointF(e.MarginBounds.Left, e.MarginBounds.Top + titleFont.Height));
            e.Graphics.DrawString($"Seller ID: {sellerId}", regularFont, brush, new PointF(e.MarginBounds.Left, e.MarginBounds.Top + titleFont.Height + regularFont.Height));
            e.Graphics.DrawString($"Sell Date: {sellDate}", regularFont, brush, new PointF(e.MarginBounds.Left, e.MarginBounds.Top + titleFont.Height + 2 * regularFont.Height));
           // e.Graphics.DrawString($"Total Amount: {totalAmt}", regularFont, brush, new PointF(e.MarginBounds.Left, e.MarginBounds.Top + titleFont.Height + 3 * regularFont.Height));

            
            e.Graphics.DrawLine(Pens.Black, e.MarginBounds.Left, e.MarginBounds.Top + titleFont.Height + 4 * regularFont.Height, e.MarginBounds.Right, e.MarginBounds.Top + titleFont.Height + 4 * regularFont.Height);

            
            int[] columnWidths = { 100, 200, 100, 100 }; 
            int xPosition = e.MarginBounds.Left;

            
            for (int i = 0; i < dataGridView2.Columns.Count; i++)
            {
                e.Graphics.DrawString(dataGridView2.Columns[i].HeaderText, regularFont, brush,
                    new Rectangle(xPosition, e.MarginBounds.Top + titleFont.Height + 4 * regularFont.Height, columnWidths[i], regularFont.Height));
                xPosition += columnWidths[i];
            }

            
            int startY = e.MarginBounds.Top + titleFont.Height + 5 * regularFont.Height;

            
            for (int rowIndex = 0; rowIndex < dataGridView2.Rows.Count; rowIndex++)
            {
                DataGridViewRow row = dataGridView2.Rows[rowIndex];
                xPosition = e.MarginBounds.Left;

                for (int colIndex = 0; colIndex < row.Cells.Count; colIndex++)
                {
                    e.Graphics.DrawString(row.Cells[colIndex].FormattedValue.ToString(), regularFont, brush,
                        new Rectangle(xPosition, startY, columnWidths[colIndex], regularFont.Height));
                    xPosition += columnWidths[colIndex];
                }

                startY += regularFont.Height;
            }
        }



    }
}
