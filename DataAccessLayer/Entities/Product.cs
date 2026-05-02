using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLayer.Entities
{
    [Table("products")]
    public class Product
    {
        [Key]
        [Column("productid")]
        public Guid ProductID { get; set; }

        [Column("productname")]
        public string ProductName { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [Column("unitprice")]
        public double? UnitPrice { get; set; }

        [Column("quantityinstock")]
        public int? QuantityInStock { get; set; }
    }
}
