using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model.Model
{
    public class AddPlanRequest
    {
        public string shipper_code { get; set; }
        public Order order { get; set; }
        public List<Address> address { get; set; }
        public List<Goods> goods { get; set; }
        public string bill_pay { get; set; }
        public string plate { get; set; }
    }

    public class Order
    {
        public string id { get; set; }
        public string shipperNo { get; set; }
        public string deliTime { get; set; }
        public string arriTime { get; set; }
        public string remark { get; set; }
        public string no { get; set; }
        public string plate { get; set; }
    }

    public class Address
    {
        public string type { get; set; }
        public string name { get; set; }
        public string linkman { get; set; }
        public string tel { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string district { get; set; }
    }

    public class Goods
    {
        public string name { get; set; }
        public string pack { get; set; }
        public double volume { get { return 0; } }
        public double amount { get; set; }
        public double weight { get; set; }
        public string code { get; set; }
    }
}
