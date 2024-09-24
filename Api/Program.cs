using Api;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

List<Usuario> usuarios = new List<Usuario>
{
    new Usuario { IdUsuario = 1, Nombre = "Lucas", Email = "lucas@example.com", NombredeUsuario = "lucas123", Contraseña = "password", Habilitado = true, FechaCreacion = DateTime.Now },
    new Usuario { IdUsuario = 2, Nombre = "Nahuel", Email = "nahuel@example.com", NombredeUsuario = "nahuel456", Contraseña = "password", Habilitado = true, FechaCreacion = DateTime.Now }
};

// POST: /usuario - Crear nuevo usuario
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
}).WithTags("Usuario");

// GET: /usuarios - Ver todos los usuarios
app.MapGet("/usuarios", () =>
{
    return Results.Ok(usuarios);
}).WithTags("Usuario");

// GET: /usuario/{id} - Ver detalle de un usuario por id
app.MapGet("/usuario/{id}", (int id) =>
{
    var usuario = usuarios.FirstOrDefault(u => u.IdUsuario == id);
    return usuario != null ? Results.Ok(usuario) : Results.NotFound();
}).WithTags("Usuario");

// PUT: /usuario/{id} - Modificar contenido de un usuario (excepto nombre)
app.MapPut("/usuario/{id}", (int id, [FromBody] Usuario usuario) =>
{
    var usuarioAActualizar = usuarios.FirstOrDefault(u => u.IdUsuario == id);
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

    return Results.NoContent();
}).WithTags("Usuario");

// DELETE: /usuario/{id} - Borrar un usuario por id
app.MapDelete("/usuario/{id}", (int id) =>
{
    var usuarioAEliminar = usuarios.FirstOrDefault(u => u.IdUsuario == id);
    if (usuarioAEliminar != null)
    {
        usuarios.Remove(usuarioAEliminar);
        return Results.NoContent();
    }

    return Results.NotFound();
}).WithTags("Usuario");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



// Lista de roles inicial
List<Rol> roles = new List<Rol>
{
    new Rol { Id = 1, Nombre = "Admin", Habilitado = true, FechaCreacion = DateTime.Now },
    new Rol { Id = 2, Nombre = "User", Habilitado = true, FechaCreacion = DateTime.Now }
};

// POST: /rol - Crear nuevo rol
app.MapPost("/rol", ([FromBody] Rol rol) =>
{
    if (string.IsNullOrWhiteSpace(rol.Nombre))
    {
        return Results.BadRequest("El nombre del rol no puede estar vacío o null.");
    }

    rol.Id = roles.Count > 0 ? roles.Max(r => r.Id) + 1 : 1;
    rol.FechaCreacion = DateTime.Now;
    roles.Add(rol);

    return Results.Created($"/rol/{rol.Id}", rol);
}).WithTags("Rol");

// GET: /roles - Ver todos los roles
app.MapGet("/roles", () =>
{
    return Results.Ok(roles);
}).WithTags("Rol");

// GET: /rol/{id} - Ver detalle de un rol por id
app.MapGet("/rol/{id}", (int id) =>
{
    var rol = roles.FirstOrDefault(r => r.Id == id);
    return rol != null ? Results.Ok(rol) : Results.NotFound();
}).WithTags("Rol");

// PUT: /rol/{id} - Modificar contenido de un rol (excepto nombre)
app.MapPut("/rol/{id}", (int id, [FromBody] Rol rol) =>
{
    var rolAActualizar = roles.FirstOrDefault(r => r.Id == id);
    if (rolAActualizar == null)
    {
        return Results.NotFound();
    }

    if (!string.IsNullOrWhiteSpace(rol.Nombre) && rolAActualizar.Nombre != rol.Nombre)
    {
        return Results.BadRequest("No se puede modificar el nombre del rol.");
    }

    // Modificamos solo los atributos que no son el nombre
    rolAActualizar.Habilitado = rol.Habilitado;
    rolAActualizar.FechaCreacion = rol.FechaCreacion;

    return Results.NoContent();
}).WithTags("Rol");

// DELETE: /rol/{id} - Borrar un rol por id
app.MapDelete("/rol/{id}", (int id) =>
{
    var rolAEliminar = roles.FirstOrDefault(r => r.Id == id);
    if (rolAEliminar != null)
    {
        roles.Remove(rolAEliminar);
        return Results.NoContent();
    }

    return Results.NotFound();
}).WithTags("Rol");


app.Run();