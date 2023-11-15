using POS.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class Inquiry : BaseEntity
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        [MaxLength(100)]
        public string ContactPerson { get; set; }
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Message { get; set; }
        public string CityName { get; set; }
        public string Website { get; set; }
        public string CountryName { get; set; }
        public Guid? CityId { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? AssignTo { get; set; }
        public Guid? InquiryStatusId { get; set; }

        public Country Country { get; set; }
        public City City { get; set; }
        [ForeignKey("AssignTo")]
        public User AssignUser { get; set; }
        [ForeignKey("InquiryStatusId")]
        public InquiryStatus InquiryStatus { get; set; }
        public Guid InquirySourceId { get; set; }
        public ICollection<InquiryAttachment> InquiryAttachments { get; set; }
        public ICollection<InquiryActivity> InquiryActivities { get; set; }
        public InquirySource InquirySource { get; set; }
        public ICollection<InquiryNote> InquiryNotes { get; set; }
        public ICollection<InquiryProduct> InquiryProducts { get; set; }
    }
}
