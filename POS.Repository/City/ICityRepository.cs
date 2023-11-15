using POS.Common.GenericRepository;
using POS.Data;
using POS.Data.Resources;
using System.Threading.Tasks;

namespace POS.Repository
{
    public interface ICityRepository : IGenericRepository<City>
    {
        Task<CityList> GetCities(CityResource cityResource);
    }
}
