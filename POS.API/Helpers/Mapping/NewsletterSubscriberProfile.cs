using AutoMapper;
using POS.Data;
using POS.Data.Dto;

namespace POS.API.Helpers.Mapping
{
    public class NewsletterSubscriberProfile:Profile
    {
        public NewsletterSubscriberProfile()
        {
            CreateMap<NewsletterSubscriber, NewsletterSubscriberDto>().ReverseMap();
        }
    }
}
