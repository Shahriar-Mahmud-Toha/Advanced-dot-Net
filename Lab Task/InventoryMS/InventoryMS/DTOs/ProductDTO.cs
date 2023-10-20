using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryMS.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CtId { get; set; }
        public int Price { get; set; }
    }
}