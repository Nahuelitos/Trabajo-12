using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class UsuarioEndpoints
{
    public static RouteGroupBuilder MapUsuarioEndpoints(this RouteGroupBuilder app)
    {
        List<Usuario> usuarios = new List<Usuario>
        {
            new Usuario { IdUsuario = 1, Nombre = "Lucas", Email = "lucas@example.com", NombredeUsuario = "lucas123", Contraseña = "password", Habilitado = true, FechaCreacion = DateTime.Now },
            new Usuario { IdUsuario = 2, Nombre = "Nahuel", Email = "nahuel@example.com", NombredeUsuario = "nahuel456", Contraseña = "password", Habilitado = true, FechaCreacion = DateTime.Now }
        };

        app.MapGet("/usuario", () =>
        {
            return Results.Ok(usuarios);
        });

        app.MapPost("/usuario", ([FromBody] Usuario usuario) =>
        {
            if (string.IsNullOrWhiteSpace(usuario.Nombre) || 
                string.IsNullOrWhiteSpace(usuario.Email) || 
                string.IsNullOrWhiteSpace(usuario.NombredeUsuario) || 
                string.IsNullOrWhiteSpace(usuario.Contraseña))
            {
                return Results.BadRequest("Los campos no pueden estar vacíos o null.");
            }

            usuario.IdUsuario = usuarios.Count > 0 ? usuarios.Max(u => u.IdUsuario) + 1 : 1;
            usuario.FechaCreacion = DateTime.Now;
            usuarios.Add(usuario);

            return Results.Created($"/usuario/{usuario.IdUsuario}", usuario);
        });

        app.MapDelete("/usuario", ([FromQuery] int idUsuario) =>
        {
            var usuarioAEliminar = usuarios.FirstOrDefault(usuario => usuario.IdUsuario == idUsuario);
            if (usuarioAEliminar != null)
            {
                usuarios.Remove(usuarioAEliminar);
                return Results.NoContent(); // Código 204
            }
            else
            {
                return Results.NotFound(); // Código 404
            }
        });

        app.MapPut("/usuario", ([FromQuery] int idUsuario, [FromBody] Usuario usuario) =>
        {
            var usuarioAActualizar = usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);
            if (usuarioAActualizar == null)
            {
                return Results.NotFound();
            }

            if (!string.IsNullOrWhiteSpace(usuario.Nombre) && usuarioAActualizar.Nombre != usuario.Nombre)
            {
                return Results.BadRequest("No se puede modificar el nombre del usuario.");
            }

            usuarioAActualizar.Email = usuario.Email;
            usuarioAActualizar.NombredeUsuario = usuario.NombredeUsuario;
            usuarioAActualizar.Contraseña = usuario.Contraseña;
            usuarioAActualizar.Habilitado = usuario.Habilitado;
            usuarioAActualizar.FechaCreacion = usuario.FechaCreacion;

            return Results.NoContent(); // Código 204
        });

        return app;
    }
}
