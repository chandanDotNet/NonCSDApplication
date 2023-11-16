using POS.Common.GenericRepository;
using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<CartList> GetCartsData(CartResource cartResource);

    }
}
