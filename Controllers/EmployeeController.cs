using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        // Using Dependency injection to access configuration settings from appsetting File.
        private readonly IConfiguration _configuration;
        // Using Dependency injection to get application path to folder
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // Adding API Methods to get all department details.

        [HttpGet]
        public JsonResult Get()
        {
            // using raw Query is a bad practice, should be changed later to variables or even using Entity to get all the data from database.
            string query = @"
                            select EmployeeId, EmployeeName, Department,
                            convert(varchar(10),DateOfJoining,120) as DateOfJoining,
                            PhotoFileName
                            from dbo.Employee
                            ";

            // Nuget package System.Data.SqlClient is required
            DataTable table = new DataTable();

            // Store Database Connection string
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;


            // Using sqlConnection and sqlCommand we are tying to execute our query and fill the datatable with data.
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            // Return Datatable as JsonResoult
            return new JsonResult(table);

        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            // using raw Query is a bad practice, should be changed later to variables or even using Entity to get all the data from database.
            string query = @"
                            insert into dbo.Employee
                            (EmployeeName,Department,DateOfJoining,PhotoFileName) 
                            values (
                            '" + emp.EmployeeName + @"',
                            '" + emp.Department + @"',
                            '" + emp.DateOfJoining + @"',
                            '" + emp.PhotoFileName + @"' )
                            ";

            // Nuget package System.Data.SqlClient is required
            DataTable table = new DataTable();

            // Store Database Connection string
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;


            // Using sqlConnection and sqlCommand we are tying to execute our query and fill the datatable with data.
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            // Return succes message as JsonResoult
            return new JsonResult("Added Successfully");
        }

        // Update Data
        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            // using raw Query is a bad practice, should be changed later to variables or even using Entity to get all the data from database.
            string query = @"
                            update dbo.Employee set  
                            EmployeeName = '" + emp.EmployeeName + @"',
                            Department = '" + emp.Department + @"',
                            DateOfJoining = '" + emp.DateOfJoining + @"'     
                            where EmployeeId = " + emp.EmployeeId + @"
                            ";

            // Nuget package System.Data.SqlClient is required
            DataTable table = new DataTable();

            // Store Database Connection string
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;


            // Using sqlConnection and sqlCommand we are tying to execute our query and fill the datatable with data.
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            // Return succes message as JsonResoult
            return new JsonResult("Updated Successfully");
        }


        // Delete Data (We are sending the id so we have to add route parameter)
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            // using raw Query is a bad practice, should be changed later to variables or even using Entity to get all the data from database.
            string query = @"
                            delete from dbo.Employee                              
                            where EmployeeId = " + id + @"
                            ";

            // Nuget package System.Data.SqlClient is required
            DataTable table = new DataTable();

            // Store Database Connection string
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;


            // Using sqlConnection and sqlCommand we are tying to execute our query and fill the datatable with data.
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            // Return succes message as JsonResoult
            return new JsonResult("Deleted Successfully");
        }

        // Custom Route Name
        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

        [Route("GetAllDepartmentNames")]
        [HttpGet]
        public JsonResult GetAllDepartmentNames()
        {
            // using raw Query is a bad practice, should be changed later to variables or even using Entity to get all the data from database.
            string query = @"
                            select DepartmentName from dbo.Department
                            ";

            // Nuget package System.Data.SqlClient is required
            DataTable table = new DataTable();

            // Store Database Connection string
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;


            // Using sqlConnection and sqlCommand we are tying to execute our query and fill the datatable with data.
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            // Return Datatable as JsonResoult
            return new JsonResult(table);
        }

    }
}
