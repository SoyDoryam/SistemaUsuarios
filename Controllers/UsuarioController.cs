using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SistemaUsuarios.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace SistemaUsuarios.Controllers
{
    // Controlador para la gestión de usuarios
    public class UsuarioController : Controller
    {
        private readonly string _configuration;

        // Constructor para inyectar la configuración de la cadena de conexión
        public UsuarioController(IConfiguration configuration)
        {
            // Recupera la cadena de conexión de la configuración de la aplicación
            _configuration = configuration.GetConnectionString("DefaultConnection");
        }

        // Acción para mostrar la vista principal de usuarios
        public IActionResult Index()
        {
            return View();
        }


        // Acción para obtener la lista de todos los usuarios (utiliza un procedimiento almacenado)
        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            IEnumerable<Usuario> usuarios = new List<Usuario>();

            try
            {
                using (var connection = new SqlConnection(_configuration))
                {
                    connection.Open();

                    // Usamos Dapper para ejecutar el procedimiento almacenado
                    usuarios = connection.Query<Usuario>("listar_usuario", commandType: System.Data.CommandType.StoredProcedure);
                }

                return Json(usuarios); // Retorna la lista de usuarios en formato JSON
            }
            catch (Exception ex)
            {
                return Json(new { mensaje = "Error al listar los usuarios", detalle = ex.Message }); // Manejo de errores
            }
        }

        // Acción para crear un nuevo usuario en la base de datos (utiliza un procedimiento almacenado)
        [HttpPost]
        public JsonResult CrearUsuario([FromBody] Usuario usuario)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration))
                {
                    connection.Open();

                    // Usamos Dapper para ejecutar el procedimiento almacenado
                    connection.Execute("crear_usuario", new
                    {
                        nombre = usuario.Nombre,
                        email = usuario.Email,
                        contrasena = usuario.Contrasena, // Se recomienda usar un hash para la contraseña
                        fecha_nacimiento = usuario.FechaNacimiento,
                        telefono = usuario.Telefono ?? (object)DBNull.Value,
                        es_activo = usuario.EsActivo,
                        saldo = usuario.Saldo,
                        direccion = usuario.Direccion ?? (object)DBNull.Value
                    }, commandType: System.Data.CommandType.StoredProcedure);
                }

                return Json(new { mensaje = "Usuario creado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { mensaje = "Error al crear el usuario", detalle = ex.Message });
            }
        }

        // Acción para obtener un usuario por su ID (utiliza un procedimiento almacenado)
        [HttpGet]
        public JsonResult ObtenerUsuarioPorId(int id)
        {
            Usuario usuario = null;

            try
            {
                using (var connection = new SqlConnection(_configuration))
                {
                    connection.Open();

                    // Usamos Dapper para ejecutar el procedimiento almacenado y obtener un solo resultado
                    usuario = connection.QueryFirstOrDefault<Usuario>(
                        "obtener_usuario_por_id",
                        new { id },
                        commandType: System.Data.CommandType.StoredProcedure);
                }

                return usuario != null ? Json(usuario) : Json(new { mensaje = "Usuario no encontrado." });
            }
            catch (Exception ex)
            {
                return Json(new { mensaje = "Error al obtener el usuario", detalle = ex.Message });
            }
        }

        // Acción para actualizar un usuario (utiliza un procedimiento almacenado)
        [HttpPost]
        public JsonResult ActualizarUsuario([FromBody] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var connection = new SqlConnection(_configuration))
                    {
                        connection.Open();

                        // Usamos Dapper para ejecutar el procedimiento almacenado
                        connection.Execute("actualizar_usuario", new
                        {
                            id = usuario.Id,
                            nombre = usuario.Nombre,
                            email = usuario.Email,
                            contrasena = usuario.Contrasena, // Se recomienda usar un hash para la contraseña
                            fecha_nacimiento = usuario.FechaNacimiento,
                            telefono = usuario.Telefono ?? (object)DBNull.Value,
                            es_activo = usuario.EsActivo,
                            saldo = usuario.Saldo,
                            direccion = usuario.Direccion ?? (object)DBNull.Value
                        }, commandType: System.Data.CommandType.StoredProcedure);
                    }

                    return Json(new { mensaje = "Usuario actualizado correctamente." });
                }
                catch (Exception ex)
                {
                    return Json(new { mensaje = "Error al actualizar el usuario", detalle = ex.Message });
                }
            }

            return Json(new { mensaje = "Datos inválidos para actualizar el usuario." });
        }

        // Acción para eliminar un usuario (utiliza un procedimiento almacenado)
        [HttpPost]
        public JsonResult EliminarUsuario([FromBody] int id)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration))
                {
                    connection.Open();

                    // Usamos Dapper para ejecutar el procedimiento almacenado
                    connection.Execute("eliminar_usuario", new { id }, commandType: System.Data.CommandType.StoredProcedure);
                }

                return Json(new { mensaje = "Usuario eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { mensaje = "Error al eliminar el usuario", detalle = ex.Message });
            }
        }
    }
}
