using System;
using System.Data;
using System.Windows.Forms;
using BusinessLogicCustomer;
using DaL;
using System.Configuration;
namespace CustomerProject_Lab4_
{
    public partial class CustomerUI : Form
    {

        public CustomerUI()
        {
            InitializeComponent();
        }
        private void CustomerUI_Load(object sender, EventArgs e)// Form loader
        {
            // Read the configurations and apply to the screen
            this.Text = ConfigurationManager.AppSettings["NameoftheApplication"].ToString();
            btnAdd.Text = ConfigurationManager.AppSettings["AddButton"].ToString();
            btnUpdate.Text = ConfigurationManager.AppSettings["UpdateButton"].ToString();
            btnDelete.Text = ConfigurationManager.AppSettings["DeleteButton"].ToString();
            LoadGrid();
            FillProducts();
            //dtgCustomers.Visible = false;
            ClearUI();
        }
        private void FillProducts()
        {
            CustomerDal dal = new CustomerDal();
            cmbProduct.DisplayMember = "ProductName";
            cmbProduct.ValueMember = "PoductId";
            cmbProduct.DataSource = dal.ReadProducts().Tables[0];
        }
        private void BtnAdd_Click(object sender, EventArgs e) // Callback function
        {
            try
            {
                Customer custobj = new Customer(); // creating the object
                custobj.CustomerName = txtCustomerName.Text;
                custobj.PhoneNumber = txtPhoneNumber.Text;
                custobj.BillAmount = Convert.ToDecimal(txtBillAmount.Text);
                custobj.ProductId = Convert.ToInt16(((DataRowView)cmbProduct.SelectedValue)["ProductId"]);//https://stackoverflow.com/questions/9429139/unable-to-cast-object-of-type-system-data-datarowview-to-type-system-iconvert/37696708

                if (custobj.Validate()) // thrown
                {
                    CustomerDal dal = new CustomerDal();
                    dal.Add(custobj);
                }
                LoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Loading values from the UI
                Customer updatedCustomer = new Customer();
                updatedCustomer.CustomerName = txtCustomerName.Text;
                updatedCustomer.PhoneNumber = txtPhoneNumber.Text;
                updatedCustomer.BillAmount = Convert.ToDecimal(txtBillAmount.Text);
                updatedCustomer.ProductId = Convert.ToInt16(((DataRowView)cmbProduct.SelectedValue)["ProductId"]);

                // Creating DaL object
                CustomerDal dal = new CustomerDal();
                // Updating database per customer record
                dal.Update(updatedCustomer, Convert.ToInt16(txtCustomerId.Text));

                // Calling Load grid refreshes the grid display
                LoadGrid();

                // Clears the UI inputs
                ClearUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Delete customer record
            // Create the record object to be deleted
            CustomerDal dal = new CustomerDal();
            DataSet records = dal.Read();
            // Load selected row (unique key)
            int recordDel = Convert.ToInt16(txtCustomerId.Text);// Fetches unique key object id and assigns it to rowSelected

            string record = recordDel.ToString();
            string mode = "delete";
            // Ask user if to proceed with deletion of this record

            if (doubleCheck(record, mode))
            {
                // Deletes selected record from the server

                dal.Delete(Convert.ToInt16(record));
            }


            LoadGrid();

            ClearUI();
        }
        
        private void ClearUI()
        {
            txtCustomerName.Text = "";
            txtPhoneNumber.Text = "";
            txtBillAmount.Text = "";
            cmbProduct.SelectedIndex = -1;
        }
        private void LoadGrid() 
        {
            CustomerDal dal = new CustomerDal();
            DataSet customers = dal.Read();
            dtgCustomers.DataSource = customers.Tables[0];
        }
        private void dtgCustomers_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int rowSelected = e.RowIndex;// Current Row
            txtCustomerId.Text = dtgCustomers.Rows[rowSelected].Cells[0].Value.ToString();
            txtCustomerName.Text = dtgCustomers.Rows[rowSelected].Cells[1].Value.ToString();
            txtPhoneNumber.Text = dtgCustomers.Rows[rowSelected].Cells[2].Value.ToString();
            txtBillAmount.Text = dtgCustomers.Rows[rowSelected].Cells[3].Value.ToString();
            cmbProduct.Text = dtgCustomers.Rows[rowSelected].Cells[4].Value.ToString();
        }
        private bool doubleCheck(string record, string mode)
        {
           
            string message = "Are you sure you want to " + mode + " record = " + record;
            const string caption = "Danger Will Robinson!!!";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Displays the MessageBox.
            result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.Yes)
            {
                return true;
            }
            // Closes the parent form.
            this.Close();
            return false;
        }
    }
}
