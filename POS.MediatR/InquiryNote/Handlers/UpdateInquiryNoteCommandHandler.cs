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
    public class UpdateInquiryNoteCommandHandler : IRequestHandler<UpdateInquiryNoteCommand, ServiceResponse<bool>>
    {
        private readonly IInquiryNoteRepository _inquiryNoteRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateInquiryNoteCommandHandler> _logger;
        public UpdateInquiryNoteCommandHandler(
            IInquiryNoteRepository inquiryNoteRepository,
            IMapper mapper,
            ILogger<UpdateInquiryNoteCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow)
        {
            _inquiryNoteRepository = inquiryNoteRepository;
            _mapper = mapper;
            _logger = logger;
            _uow = uow;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateInquiryNoteCommand request, CancellationToken cancellationToken)
        {
            var entity = await _inquiryNoteRepository.FindAsync(request.Id);
            if (entity == null)
            {
                return ServiceResponse<bool>.Return404();
            }
            entity.Note = request.Note;
            _inquiryNoteRepository.Update(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while adding industry");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
