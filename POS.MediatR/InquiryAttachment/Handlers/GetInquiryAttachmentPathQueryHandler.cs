using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetInquiryAttachmentPathQueryHandler : IRequestHandler<GetInquiryAttachmentPathQuery, string>
    {

        private readonly IInquiryAttachmentRepository _inquiryAttachmentRepository;
        private readonly PathHelper _pathHelper;

        public GetInquiryAttachmentPathQueryHandler(
            IInquiryAttachmentRepository inquiryAttachmentRepository,
            PathHelper pathHelper
          )
        {
            _inquiryAttachmentRepository = inquiryAttachmentRepository;
            _pathHelper = pathHelper;
        }

        public async Task<string> Handle(GetInquiryAttachmentPathQuery request, CancellationToken cancellationToken)
        {
            var entity = await _inquiryAttachmentRepository.FindAsync(request.Id);
            if (entity != null)
            {
                return entity.Path;
            }
            return string.Empty;
        }
    }
}
