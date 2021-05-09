using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        // Using Dependency injection to access configuration settings from appsetting File.
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Adding API Methods to get all department details.

        
        public JsonResult Get()
        {
            // using raw Query is a bad practice, should be changed later to variables or even using Entity to get all the data from database.
            string query = @"
                            select DepartmentId, DepartmentName from dbo.Department";

            // Nuget package System.Data.SqlClient is required
            DataTable table = new DataTable();

            // Store Database Connection string
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;


            // Using sqlConnection and sqlCommand we are tying to execute our query and fill the datatable with data.
            using(SqlConnection myCon = new SqlConnection(sqlDataSource))
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
        public JsonResult Post(Department dep)
        {
            // using raw Query is a bad practice, should be changed later to variables or even using Entity to get all the data from database.
            string query = @"
                            insert into dbo.Department values 
                            ('"+dep.DepartmentName+@"')
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
            return new JsonResult("Added Successfully");

        }


    }
}
