using BusinessLogicCustomer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace DaL
{
    public class CustomerDal
    {
        public SqlConnection CreateConnection()
        {
            string connstr = ConfigurationManager.ConnectionStrings["DbConn"].ToString();
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            return conn;
        }
        public bool Update(Customer obj, int CustomerId)
        {
                try
                {
                SqlConnection conn = CreateConnection();
                    // Step 2 :- SQL --> Command
                    SqlCommand command = new SqlCommand();
                    command.Connection = conn;
                    string sqltxt1 = "UPDATE tblCustomer " +
                                     "SET CustomerName='" + obj.CustomerName +
                                     "',PhoneNumber='" + obj.PhoneNumber +
                                     "',BillAmount=" + obj.BillAmount +
                                     ",ProductId_fk=" + obj.ProductId  +
                                     " WHERE customerId=" + CustomerId;
                    string sqltxt2 = "UPDATE tblCustomer " +
                                     "SET CustomerName='" + obj.CustomerName +
                                     "', PhoneNumber='" + obj.PhoneNumber +
                                     "', ProductId_fk=" + obj.ProductId +
                                     ", BillAmount=" + obj.BillAmount +
                                     " WHERE Id=(SELECT MAX(CustomerId from tblCustomer)";
                    command.CommandText = sqltxt1;
                    command.ExecuteNonQuery();
                    // Step 3:- Close connection
                    conn.Close();
                    return true;
                }
                catch (Exception)
                {
                return false;
                }

            
        }
        public bool Add(Customer obj)
        {
            // ADO.NET
            // Step 1 :- open connection
            try
            {
                SqlConnection conn = CreateConnection();
                // Step 2 :- SQL --> Command
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = "INSERT into tblCustomer " +
                                      "(CustomerName,PhoneNumber,BillAmount,ProductId_fk)" +
                                      "values('" + obj.CustomerName +
                                      "','" + obj.PhoneNumber  + "'," + obj.BillAmount.ToString() +
                                      "," + obj.ProductId  +
                                      ")";
                command.ExecuteNonQuery();
                // Step 3:- Close connection
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public DataSet ReadProducts()
        {
            SqlConnection conn = CreateConnection();
            // Step 2 :- SQL --> Command
            SqlCommand command = new SqlCommand();
            command.Connection = conn;
            command.CommandText = "SELECT * from mstProduct";
            //DataSet and DataAdapter
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet products = new DataSet();
            adapter.Fill(products);
            // Step 3:- Close connection
            conn.Close();
            return products;
        }
        public DataSet Read()
        {
                SqlConnection conn = CreateConnection();
                // Step 2 :- SQL --> Command
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = "SELECT tblCustomer.CustomerId," +
                                         " tblCustomer.CustomerName, " + 
                                         " tblCustomer.PhoneNumber, " +
                                         " tblCustomer.BillAmount, " +
                                         "mstProduct.ProductName " +
                                         "FROM tblCustomer INNER JOIN mstProduct " +
                                         "ON tblCustomer.ProductId_fk = mstProduct.ProductId";
                //DataSet and DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet customers = new DataSet();
                adapter.Fill(customers);
                // Step 3:- Close connection
                conn.Close();
                return customers;
        }
        public bool Delete(int CustomerId)
        {
            try
            {
                SqlConnection conn = CreateConnection();
                // Step 2 :- SQL --> Command
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = "DELETE  FROM tblCustomer " +
                                      "WHERE customerId=" + CustomerId;
                //
                command.ExecuteNonQuery();
                // Step 3:- Close connection
                conn.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
    
}
