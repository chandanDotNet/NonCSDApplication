using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class DeleteContactUsCommandHandler : IRequestHandler<DeleteContactUsCommand, ServiceResponse<bool>>
    {
        private readonly IContactUsRepository _contactUsRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteActionCommandHandler> _logger;

        public DeleteContactUsCommandHandler(IContactUsRepository contactUsRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteActionCommandHandler> logger)
        {
            _logger = logger;
            _contactUsRepository = contactUsRepository;
            _uow = uow;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteContactUsCommand request, CancellationToken cancellationToken)
        {
            var contactUs = await _contactUsRepository.FindAsync(request.Id);
            if (contactUs == null)
            {
                _logger.LogError("Contact Us not found", request);
                return ServiceResponse<bool>.Return404();
            }

            _contactUsRepository.Delete(contactUs);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while deleting Contact Us.", request);
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
