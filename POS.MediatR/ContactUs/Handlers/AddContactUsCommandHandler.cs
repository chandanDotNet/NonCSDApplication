using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
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
    public class AddContactUsCommandHandler : IRequestHandler<AddContactUsCommand, ServiceResponse<ContactUsDto>>
    {
        private readonly IMapper _mapper;
        private readonly IContactUsRepository _contactUsRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<AddContactUsCommandHandler> _logger;

        public AddContactUsCommandHandler(IMapper mapper,
            IContactUsRepository contactUsRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddContactUsCommandHandler> logger)
        {
            _mapper = mapper;
            _contactUsRepository = contactUsRepository;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<ContactUsDto>> Handle(AddContactUsCommand request, CancellationToken cancellationToken)
        {
            var contactRequestData = _mapper.Map<ContactRequest>(request);
            _contactUsRepository.Add(contactRequestData);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while added Contact Us.");
                return ServiceResponse<ContactUsDto>.Return500();
            }
            var contactUsDto = _mapper.Map<ContactUsDto>(contactRequestData);
            return ServiceResponse<ContactUsDto>.ReturnResultWith200(contactUsDto);
        }
    }
}
