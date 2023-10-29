using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EmployeeApi.EFEmployee
{
    public partial class EmployeeDetail
    {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; }
        public string MobilePhone { get; set; }
        public string JobClass { get; set; }
        public string Department { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
