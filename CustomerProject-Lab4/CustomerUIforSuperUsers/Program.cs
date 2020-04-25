using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicCustomer;
namespace CustomerUIforSuperUsers
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Customer obj = new Customer();
                obj.CustomerName = Console.ReadLine();
                obj.Validate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            
        }
    }
}
