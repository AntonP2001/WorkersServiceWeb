using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WorkersServiceWeb.Model;
using WorkersServiceWeb.Repositories;

namespace WorkersServiceWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class WorkerServiceController : ControllerBase
    {
        IWorkerRepository repo;
        public WorkerServiceController(IWorkerRepository repo)
        {
            this.repo = repo;
        }
        
        /// <summary>
        /// Add new worker
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="phone"></param>
        /// <param name="companyId"></param>
        /// <param name="passport"></param>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        [HttpPost("AddWorker")]
        public IActionResult Add([Required] string name, 
            [Required] string surname, [Required] string phone, [Required] int companyId,
            [Required][FromQuery] Passport passport,
            [Required] string departmentName)
        {
            int newWorker;
            try
            {
                newWorker = repo.AddWorker(new Worker()
                {
                    Name = name,
                    Surname = surname,
                    Phone = phone,
                    CompanyId = companyId,
                    PassportNumber = passport.Number,
                    DepartmentName = departmentName
                }, passport);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok(newWorker);
        }

        /// <summary>
        /// Deletes a specified worker
        /// </summary>
        /// <param name="id" required="true"></param>
        /// <returns></returns>
        [HttpDelete("DeleteWorker")]
        public IActionResult Delete([Required] int id)
        {
            try
            {
                repo.DeleteWorker(id);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
        
        /// <summary>
        /// Get collection of workers by company 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet("GetWorkersByCompany")]
        public IActionResult GetWorkersById([Required] int companyId)
        {
            List<object> workers = new();
            try
            {
                workers = repo.GetWorkersFromCompany(companyId);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok(workers);
        }

        /// <summary>
        /// Get collection of workers by deaprtment
        /// </summary>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        [HttpGet("GetWorkersByDepartment")]
        public IActionResult GetWorkersByDepartment([Required] string departmentName)
        {
            List<object> workers = new();
            try
            {
                workers = repo.GetWorkersFromDepartment(departmentName);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok(workers);
        }

        /// <summary>
        /// Update worker by id 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="phone"></param>
        /// <param name="companyId"></param>
        /// <param name="passportNumber"></param>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        [HttpPost("UpdateWorker")]
        public IActionResult UpdateWorker([Required] int id, string? name, string? surname, string? phone, int? companyId,
            string? passportNumber,
            string? departmentName)
        {
            try
            {
                repo.ChangeWorker(id, name, surname, phone, companyId, passportNumber, departmentName);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}