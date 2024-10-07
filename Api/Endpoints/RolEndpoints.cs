using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class RolEndpoints
{
    public static RouteGroupBuilder MapRolEndpoints(this RouteGroupBuilder app)
    {
        List<Rol> roles = new List<Rol>
        {
            new Rol { IdRol = 1, Nombre = "Administrador" },
            new Rol { IdRol = 2, Nombre = "Usuario" }
        };

        // Ver todos los roles
        app.MapGet("/rol", () =>
        {
            return Results.Ok(roles);
        });

        // Crear un nuevo rol
        app.MapPost("/rol", ([FromBody] Rol rol) =>
        {
            if (string.IsNullOrWhiteSpace(rol.Nombre))
            {
                return Results.BadRequest("El nombre del rol no puede estar vacío o null.");
            }

            rol.IdRol = roles.Count > 0 ? roles.Max(r => r.IdRol) + 1 : 1;
            roles.Add(rol);

            return Results.Created($"/rol/{rol.IdRol}", rol);
        });

        // Eliminar un rol por ID
        app.MapDelete("/rol", ([FromQuery] int idRol) =>
        {
            var rolAEliminar = roles.FirstOrDefault(rol => rol.IdRol == idRol);
            if (rolAEliminar != null)
            {
                roles.Remove(rolAEliminar);
                return Results.NoContent(); // Código 204
            }
            else
            {
                return Results.NotFound(); // Código 404
            }
        });

        // Actualizar un rol por ID
        app.MapPut("/rol", ([FromQuery] int idRol, [FromBody] Rol rol) =>
        {
            var rolAActualizar = roles.FirstOrDefault(r => r.IdRol == idRol);
            if (rolAActualizar == null)
            {
                return Results.NotFound(); // Código 404
            }

            rolAActualizar.Nombre = rol.Nombre;
            return Results.NoContent(); // Código 204
        });

        return app;
    }
}
