using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryMS.DTOs
{
    public class ProductsCustomerDTO
    {
        public int Id { get; set; }
        public int PdId { get; set; }
        public string CusEmail { get; set; }
        public int Count { get; set; }
        public string OrderStatus { get; set; }
        public Nullable<System.DateTime> OrderTime { get; set; }
    }
}