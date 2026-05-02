using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.DTO
{
    public record ProductUpdateequest(Guid ProductID, string ProductName, CategoryOptions Category, double? UnitPrice, int? QuantityInStock)
    {
        public ProductUpdateequest() : this(default, string.Empty, default, default, default)
        {
        }
    }
}
