using MediatR;
using POS.Data;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CommandAndQuery
{
    public class GetAllWishlistQuery : IRequest<AllWishList>
    {
        public WishlistResource WishlistResource { get; set; }
    }
}
