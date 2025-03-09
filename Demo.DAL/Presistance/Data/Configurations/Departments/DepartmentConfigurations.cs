using Demo.DAL.Entities.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Presistance.Data.Configurations.Departments
{
    internal class DepartmentConfigurations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(D => D.Id).UseIdentityColumn(10, 10);
            builder.Property(D => D.Name).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(D => D.Code).HasColumnType("nvarchar(20)").IsRequired();
            builder.Property(D => D.LastModifiedOn).HasComputedColumnSql("GETDATE()");
            builder.Property(D => D.CreatedOn).HasDefaultValueSql("GETDATE()");
        }
    }
}
