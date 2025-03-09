namespace Demo.PL.ViewModels.Departments
{
    public class DepartmentEditViewModel
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Code { get; set; } = null!;
        public DateOnly CreationDate { get; set; }
    }
}
