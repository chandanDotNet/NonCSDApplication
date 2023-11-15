using POS.Data.Dto;
using POS.Helper;
using MediatR;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class GetTestimonialsQuery : IRequest<ServiceResponse<TestimonialsDto>>
    {
        public Guid Id { get; set; }
    }
}
