using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using POS.Helper;

namespace POS.MediatR.Handlers
{
    public class UpdateUserClaimCommandHandler : IRequestHandler<UpdateUserClaimCommand, ServiceResponse<UserClaimDto>>
    {
        IUnitOfWork<POSDbContext> _uow;
        IUserClaimRepository _userClaimRepository;
        private readonly IMapper _mapper;
        public UpdateUserClaimCommandHandler(
            IMapper mapper,
            IUserClaimRepository userClaimRepository,
            IUnitOfWork<POSDbContext> uow
            )
        {
            _mapper = mapper;
            _uow = uow;
            _userClaimRepository = userClaimRepository;
        }

        public async Task<ServiceResponse<UserClaimDto>> Handle(UpdateUserClaimCommand request, CancellationToken cancellationToken)
        {
            request.UserClaims.ForEach(uc => uc.ClaimType = uc.ClaimType.Trim().Replace(" ", "_"));
            var appUserClaims = await _userClaimRepository.All.Where(c => c.UserId == request.Id).ToListAsync();
            var claimsToAdd = request.UserClaims.Where(c => !appUserClaims.Select(c => c.ClaimType).Contains(c.ClaimType)).ToList();
            _userClaimRepository.AddRange(_mapper.Map<List<UserClaim>>(claimsToAdd));
            var claimsToDelete = appUserClaims.Where(c => !request.UserClaims.Select(cs => cs.ClaimType).Contains(c.ClaimType)).ToList();
            _userClaimRepository.RemoveRange(claimsToDelete);
            if (claimsToAdd.Any() || claimsToDelete.Any())
            {
                if (await _uow.SaveAsync() <= 0)
                {
                    return ServiceResponse<UserClaimDto>.Return500();
                }
            }
            return ServiceResponse<UserClaimDto>.ReturnSuccess();
        }
    }
}
