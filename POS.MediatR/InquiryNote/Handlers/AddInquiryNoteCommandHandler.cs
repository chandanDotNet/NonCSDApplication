using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data.Entities;
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
    public class AddInquiryNoteCommandHandler : IRequestHandler<AddInquiryNoteCommand, ServiceResponse<InquiryNoteDto>>
    {
        private readonly IInquiryNoteRepository _inquiryNoteRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddInquiryNoteCommandHandler> _logger;
        public AddInquiryNoteCommandHandler(
            IInquiryNoteRepository inquiryNoteRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddInquiryNoteCommandHandler> logger)
        {
            _inquiryNoteRepository = inquiryNoteRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ServiceResponse<InquiryNoteDto>> Handle(AddInquiryNoteCommand request, CancellationToken cancellationToken)
        {
            var inquiryNoteEntity = _mapper.Map<InquiryNote>(request);
            _inquiryNoteRepository.Add(inquiryNoteEntity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while adding industry");
                return ServiceResponse<InquiryNoteDto>.Return500();
            }
            var inquiryNoteDto = _mapper.Map<InquiryNoteDto>(inquiryNoteEntity);
            return ServiceResponse<InquiryNoteDto>.ReturnResultWith200(inquiryNoteDto);
        }
    }
}
