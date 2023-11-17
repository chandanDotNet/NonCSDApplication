using POS.Data.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class WishlistResource : ResourceParameter
    {

        public WishlistResource() : base("CustomerId")
        {

        }

        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
    }
}
