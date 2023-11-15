using System;
using System.ComponentModel.DataAnnotations;

namespace POS.Data
{
    public class LoginAudit
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime LoginTime { get; set; }
        [MaxLength(50)]
        public string RemoteIP { get; set; }
        public string Status { get; set; }
        public string Provider { get; set; }
        [MaxLength(50)]
        public string Latitude { get; set; }
        [MaxLength(50)]
        public string Longitude { get; set; }
    }
}
