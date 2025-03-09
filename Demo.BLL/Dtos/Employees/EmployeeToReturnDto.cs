using Demo.DAL.Entities.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Dtos.Employees
{
    public class EmployeeToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public int? Age { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public string Gender { get; set; }
        public string EmpolyeeType { get; set; }
    }
}
