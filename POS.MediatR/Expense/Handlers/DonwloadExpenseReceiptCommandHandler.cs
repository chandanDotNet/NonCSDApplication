using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class DonwloadExpenseReceiptCommandHandler
        : IRequestHandler<DonwloadExpenseReceiptCommand, string>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        public DonwloadExpenseReceiptCommandHandler(
            IExpenseRepository expenseRepository,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _expenseRepository = expenseRepository;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }

        public async Task<string> Handle(DonwloadExpenseReceiptCommand request, CancellationToken cancellationToken)
        {
            var expense = await _expenseRepository.FindAsync(request.Id);
            if (expense == null)
            {
                return "";
            }
            string contentRootPath = _webHostEnvironment.WebRootPath;
            return Path.Combine(contentRootPath, _pathHelper.Attachments, expense.ReceiptPath);
        }
    }
}
