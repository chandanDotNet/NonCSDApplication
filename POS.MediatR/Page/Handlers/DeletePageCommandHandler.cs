using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using POS.Helper;

namespace POS.MediatR.Handlers
{
    public class DeletePageCommandHandler : IRequestHandler<DeletePageCommand, ServiceResponse<PageDto>>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public DeletePageCommandHandler(
           IPageRepository pageRepository,
            IUnitOfWork<POSDbContext> uow
            )
        {
            _pageRepository = pageRepository;
            _uow = uow;
        }

        public async Task<ServiceResponse<PageDto>> Handle(DeletePageCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _pageRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                return ServiceResponse<PageDto>.Return404();
            }
            _pageRepository.Delete(request.Id);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<PageDto>.Return500();
            }
            return ServiceResponse<PageDto>.ReturnSuccess();
        }
    }
}
