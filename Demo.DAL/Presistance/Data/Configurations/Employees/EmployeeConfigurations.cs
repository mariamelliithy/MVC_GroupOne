using Demo.DAL.Entities.Common.Enums;
using Demo.DAL.Entities.Employess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Presistance.Data.Configurations.Employees
{
    public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E => E.Name).HasColumnType("varchar(50)").IsRequired();
            builder.Property(E => E.Email).HasColumnType("varchar(50)").IsRequired();
            builder.Property(E => E.Address).HasColumnType("varchar(100)");
            builder.Property(E => E.Salary).HasColumnType("decimal(8.2)");
            builder.Property(D => D.LastModifiedOn).HasComputedColumnSql("GETDATE()");
            builder.Property(D => D.CreatedOn).HasDefaultValueSql("GETDATE()");

            builder.Property(E => E.Gender)
                .HasConversion(
                (gender) => gender.ToString(),
                (gender) => (Gender)Enum.Parse(typeof(Gender), gender)
                );

            builder.Property(E => E.EmpolyeeType)
                .HasConversion(
                (empTypee) => empTypee.ToString(),
                (empTypee) => (EmployeeType)Enum.Parse(typeof(EmployeeType), empTypee)
                );
        }
    }
}
