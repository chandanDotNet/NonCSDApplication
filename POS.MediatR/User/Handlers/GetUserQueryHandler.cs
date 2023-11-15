using AutoMapper;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using POS.Helper;
using Microsoft.Extensions.Logging;
using System.IO;

namespace POS.MediatR.Handlers
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ServiceResponse<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserQueryHandler> _logger;
        private readonly PathHelper _pathHelper;
        public GetUserQueryHandler(
           IUserRepository userRepository,
            IMapper mapper,
            ILogger<GetUserQueryHandler> logger,
            PathHelper pathHelper
            )
        {

            _mapper = mapper;
            _userRepository = userRepository;
            _logger = logger;
            _pathHelper = pathHelper;
        }

        public async Task<ServiceResponse<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await _userRepository.AllIncluding(c => c.UserRoles, cs => cs.UserClaims).FirstOrDefaultAsync(c => c.Id == request.Id);
            if (entity != null)
            {
                var entityDto = _mapper.Map<UserDto>(entity);
                if (!string.IsNullOrWhiteSpace(entityDto.ProfilePhoto))
                {
                    entityDto.ProfilePhoto = Path.Combine(_pathHelper.UserProfilePath, entityDto.ProfilePhoto);
                }
                return ServiceResponse<UserDto>.ReturnResultWith200(entityDto);
            }
            else
            {
                _logger.LogError("User not found");
                return ServiceResponse<UserDto>.ReturnFailed(404, "User not found");
            }
        }
    }
}
