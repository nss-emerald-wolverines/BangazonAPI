using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonAPI.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            // First have to add connection in appsettings.json
            // "AllowedHosts": "*",
            // "ConnectionStrings": {
            // "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=StudentExercisesDB;Trusted_Connection=True;"
            // }

            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // CODE FOR GETTING A LIST OF EXERCISES

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Price, Title, Description, Quantity FROM Product";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Product> exercises = new List<Product>();

                    while (reader.Read())
                    {
                        Product exercise = new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ExerciseName = reader.GetString(reader.GetOrdinal("ExerciseName")),
                            Language = reader.GetString(reader.GetOrdinal("Language"))
                        };

                        exercises.Add(exercise);
                    }
                    reader.Close();

                    return Ok(exercises);
                }
            }
        }

        // CODE FOR GETTING A SINGLE EXERCISES

        [HttpGet("{id}", Name = "GetOneProduct")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT
                            Id, Price, Title, Description, Quantity
                            FROM Product
                            WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Product product = null;

                    if (reader.Read())
                    {
                        product = new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ExerciseName = reader.GetString(reader.GetOrdinal("Title")),
                            Language = reader.GetString(reader.GetOrdinal("Description"))
                        };
                    }
                    reader.Close();

                    return Ok(product);
                }
            }
        }