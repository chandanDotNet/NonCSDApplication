using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using POS.Helper;
using Microsoft.Extensions.Logging;

namespace POS.MediatR.Handlers
{
    public class UpdatePageCommandHandler : IRequestHandler<UpdatePageCommand, ServiceResponse<PageDto>>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePageCommandHandler> _logger;
        public UpdatePageCommandHandler(
           IPageRepository pageRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdatePageCommandHandler> logger
            )
        {
            _pageRepository = pageRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<PageDto>> Handle(UpdatePageCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _pageRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id)
                .FirstOrDefaultAsync();
            if (entityExist != null)
            {
                _logger.LogError("Page Name Already Exist.");
                return ServiceResponse<PageDto>.Return409("Page Name Already Exist.");
            }
            entityExist = await _pageRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            entityExist.Name = request.Name;
            entityExist.Order = request.Order;
            _pageRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<PageDto>.Return500();
            }
            return ServiceResponse<PageDto>.ReturnResultWith200(_mapper.Map<PageDto>(entityExist));
        }
    }
}
