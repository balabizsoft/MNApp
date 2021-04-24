using System;
using System.Collections.Generic;
using System.Text;

namespace MNApp.DAL
{
    public class DomainDetail
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool? IsPing { get; set; }
        public bool? HasDate { get; set; }
        public bool? HasMedia { get; set; }
        public bool? HasMail { get; set; }
        public bool? HasContact { get; set; }
        public string IPAddress { get; set; }
        public DateTime? RegisterAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? ExpiryAt { get; set; }

        public int FileDetailId { get; set; }
        public FileDetail FileDetail { get; set; }
    }
}
