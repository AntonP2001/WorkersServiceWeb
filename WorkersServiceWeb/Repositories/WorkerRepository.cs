using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Text;
using WorkersServiceWeb.Model;

namespace WorkersServiceWeb.Repositories
{
    public interface IWorkerRepository
    {
        public int AddWorker(Worker worker, Passport passport);
        public void DeleteWorker(int id);
        public List<object> GetWorkersFromCompany(int companyId);
        public List<object> GetWorkersFromDepartment(string department);
        public void ChangeWorker(params object[] parameters);
    }
    public class WorkerRepository : IWorkerRepository
    {
        string connectionString;
        public WorkerRepository(string conn)
        {
            connectionString = conn;
        }
        public int AddWorker(Worker worker, Passport passport)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var query = "INSERT INTO Passport (Type, Number)" +
                    "Values(@Type, @Number)";
                db.Execute(query, passport); 
                var query2 = "INSERT INTO Worker (Name, Surname, Phone, " +
                    "CompanyId, Passport, Department) " +
                "VALUES(@Name, @Surname, @Phone, @CompanyId, @PassportNumber, @DepartmentName);" +
                "SELECT SCOPE_IDENTITY()";
                return db.Query<int>(query2, worker).First();
            }
        }

        public void ChangeWorker(params object[] parameters)
        {
            var paramsNames = new StackTrace().GetFrame(1).GetMethod().GetParameters().ToList();
            StringBuilder builder = new();
            builder.Append("UPDATE Worker SET ");
            foreach(var param in parameters){
                int i = parameters.ToList().IndexOf(param);
                if (param != null && i > 0)
                {
                    builder.Append(String.Format("{0} = '{1}', ", paramsNames[i].Name, param)); 
                }
            }
            builder.Remove(builder.Length - 2, 2); 
            builder.Append(String.Format(" WHERE Id = {0}", parameters[0]));
            using(IDbConnection db = new SqlConnection(connectionString))
            {
                db.Execute(builder.ToString());
            }
        }

        public void DeleteWorker(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string query = "DELETE Worker " +
                    "WHERE Id = @id";
                db.Execute(query, new {id});
            }
        }

        public List<object> GetWorkersFromCompany(int companyId)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string query = "SELECT Worker.Id, Worker.Name, Worker.Surname, Worker.Phone, " +
                    "Worker.CompanyId, Department.Name as DepartmentName, Department.Phone as DepartmentPhone, Passport.Type, Passport.Number " +
                    "FROM Worker " +
                    "INNER JOIN Department ON Worker.Department = Department.Name " +
                    "INNER JOIN Passport ON Worker.Passport = Passport.Number " +
                    "WHERE CompanyId = @companyId";
                return db.Query<object>(query, new { companyId }).ToList();
            }
        }

        public List<object> GetWorkersFromDepartment(string department)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string query = "SELECT Worker.Id, Worker.Name, Worker.Surname, Worker.Phone, " +
                    "Worker.CompanyId, Department.Name as DepartmentName, Department.Phone as DepartmentPhone, Passport.Type, Passport.Number " +
                    "FROM Worker " +
                    "INNER JOIN Department ON Worker.Department = Department.Name " +
                    "INNER JOIN Passport ON Worker.Passport = Passport.Number " +
                    "WHERE Department = @department";
                return db.Query<object>(query, new { department }).ToList();
            }
        }
    }
}
