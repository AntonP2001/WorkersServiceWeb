using System.ComponentModel.DataAnnotations;

namespace WorkersServiceWeb.Model
{
    public class Passport
    {
        public string Type { get; set; }
        public string Number { get; set; }
    }
    public class Department
    {
        public string Name { get; set; }
        public string Phone { get; set; }
    }

    public class Worker
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
        public string Phone { get; set; }
        public int CompanyId { get; set; }
        public string PassportNumber { get; set; }
        public string DepartmentName { get; set; }
    }

}
