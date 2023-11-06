using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryMS.DTOs
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public int OrId { get; set; }
        public int OrderedQuantity { get; set; }
        public int Price { get; set; }
        public int ProductId { get; set; }
    }
}