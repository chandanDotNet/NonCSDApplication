using AutoMapper;
using MediatR;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Country.Commands;
using POS.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Country.Handlers
{
    public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, ServiceResponse<bool>>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        public DeleteCountryCommandHandler(
           ICountryRepository countryRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow
            )
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _uow = uow;
        }
        public async Task<ServiceResponse<bool>> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _countryRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                return ServiceResponse<bool>.Return404();
            }
            _countryRepository.Delete(request.Id);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
