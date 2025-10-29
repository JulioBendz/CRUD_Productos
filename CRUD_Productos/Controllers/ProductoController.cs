using CRUD_Productos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CRUD_Productos.Controllers
{
    public class ProductoController : Controller
    {
        private readonly string? cadenaSQL;

        public ProductoController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("cadenaSQL");
        }

        public IActionResult Index()
        {
            List<Producto> lista = new List<Producto>();

            using (SqlConnection cn = new SqlConnection(cadenaSQL))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Productos", cn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Producto
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nombre = dr["Nombre"].ToString(),
                        Precio = Convert.ToDecimal(dr["Precio"]),
                        Stock = Convert.ToInt32(dr["Stock"])
                    });
                }
            }

            return View(lista);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Producto p)
        {
            using (SqlConnection cn = new SqlConnection(cadenaSQL))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Productos (Nombre, Precio, Stock) VALUES (@n, @p, @s)", cn);
                cmd.Parameters.AddWithValue("@n", p.Nombre);
                cmd.Parameters.AddWithValue("@p", p.Precio);
                cmd.Parameters.AddWithValue("@s", p.Stock);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Producto p = new Producto();

            using (SqlConnection cn = new SqlConnection(cadenaSQL))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Productos WHERE Id=@id", cn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    p.Id = Convert.ToInt32(dr["Id"]);
                    p.Nombre = dr["Nombre"].ToString();
                    p.Precio = Convert.ToDecimal(dr["Precio"]);
                    p.Stock = Convert.ToInt32(dr["Stock"]);
                }
            }

            return View(p);
        }

        [HttpPost]
        public IActionResult Edit(Producto p)
        {
            using (SqlConnection cn = new SqlConnection(cadenaSQL))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Productos SET Nombre=@n, Precio=@p, Stock=@s WHERE Id=@id", cn);
                cmd.Parameters.AddWithValue("@n", p.Nombre);
                cmd.Parameters.AddWithValue("@p", p.Precio);
                cmd.Parameters.AddWithValue("@s", p.Stock);
                cmd.Parameters.AddWithValue("@id", p.Id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Producto p = new Producto();

            using (SqlConnection cn = new SqlConnection(cadenaSQL))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Productos WHERE Id=@id", cn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    p.Id = Convert.ToInt32(dr["Id"]);
                    p.Nombre = dr["Nombre"].ToString();
                    p.Precio = Convert.ToDecimal(dr["Precio"]);
                    p.Stock = Convert.ToInt32(dr["Stock"]);
                }
            }

            return View(p);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection cn = new SqlConnection(cadenaSQL))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Productos WHERE Id=@id", cn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
