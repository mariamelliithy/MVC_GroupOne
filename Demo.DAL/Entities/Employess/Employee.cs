using Demo.DAL.Entities.Common.Enums;
using Demo.DAL.Entities.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities.Employess
{
    public class Employee : ModelBase
    {
        public string Name { get; set; } = null!;
        public string? Email {  get; set; }
        public int? Age { get; set; }
        public string? Address { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly HiringDate { get; set; }
        public Gender Gender { get; set; }
        public EmployeeType EmpolyeeType { get; set; }
        //Navgitonal property [One]
        public virtual Department? Department { get; set; }
        //Foreign key
        public int? DepartmentId { get; set; }
        public string? Image {  get; set; }
    }
}
