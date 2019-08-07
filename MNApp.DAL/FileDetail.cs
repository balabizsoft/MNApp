using System;
using System.Collections.Generic;
using System.Text;

namespace MNApp.DAL
{
    public class FileDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdate { get; set; }

        public List<DomainDetail> DomainDetails { get; set; }
    }
}
