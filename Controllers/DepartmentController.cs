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

        [HttpGet]
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

        // Update Data
        [HttpPut]
        public JsonResult Put(Department dep)
        {
            // using raw Query is a bad practice, should be changed later to variables or even using Entity to get all the data from database.
            string query = @"
                            update dbo.Department set  
                            DepartmentName = '" +dep.DepartmentName + @"'
                            where DepartmentId = "+dep.DepartmentId  + @"
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
            return new JsonResult("Updated Successfully");
        }


        // Delete Data (We are sending the id so we have to add route parameter)
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            // using raw Query is a bad practice, should be changed later to variables or even using Entity to get all the data from database.
            string query = @"
                            delete from dbo.Department                              
                            where DepartmentId = " + id + @"
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
            return new JsonResult("Deleted Successfully");
        }





    }
}
