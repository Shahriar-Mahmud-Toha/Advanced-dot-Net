using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryMS.DTOs
{
    public class OrderDTO
    {
        public int OrId { get; set; }
        public string Status { get; set; }
        public System.DateTime Time { get; set; }
        public string CusEmail { get; set; }
        public string DelAddress { get; set; }
    }
}