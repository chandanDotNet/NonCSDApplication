using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class DeleteInquiryNoteCommandHandler : IRequestHandler<DeleteInquiryNoteCommand, ServiceResponse<bool>>
    {
        private readonly IInquiryNoteRepository _inquiryNoteRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteInquiryNoteCommandHandler> _logger;
        public DeleteInquiryNoteCommandHandler(
            IInquiryNoteRepository inquiryNoteRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<DeleteInquiryNoteCommandHandler> logger)
        {
            _inquiryNoteRepository = inquiryNoteRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteInquiryNoteCommand request, CancellationToken cancellationToken)
        {
            var inquiryNoteEntity = await _inquiryNoteRepository.FindAsync(request.Id) ;
            if (inquiryNoteEntity != null)
            {
                _inquiryNoteRepository.Remove(inquiryNoteEntity);
                if (await _uow.SaveAsync() <= 0)
                {
                    _logger.LogError("Error while adding industry");
                    return ServiceResponse<bool>.Return500();
                }
                return ServiceResponse<bool>.ReturnResultWith200(true);
            }
            return ServiceResponse<bool>.Return404();
        }
    }
}
