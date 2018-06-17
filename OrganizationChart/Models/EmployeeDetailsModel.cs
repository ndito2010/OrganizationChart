using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrganizationChart.Models
{
    public class EmployeeDetailsModel
    {
        public string EmployeeId { get; set; }

        public string FirstName { get; set; }
      
        public string Title { get; set; }
        public string ManagerId { get; set; }
        
    }
}