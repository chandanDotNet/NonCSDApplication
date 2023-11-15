using System;

namespace POS.Data.Dto
{
    public class InquiryShortDto
    {
        public Guid? Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MobileNo { get; set; }
        public string Message { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public Guid? CityId { get; set; }
        public Guid? CountryId { get; set; }
        public string InquiryStatus { get; set; }
        public string InquirySource { get; set; }
        public string AssignToName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int InquiryNoteCount { get; set; }
        public int InquiryAttachmentCount { get; set; }
        public int InquiryActivityCount { get; set; }
    }
}
