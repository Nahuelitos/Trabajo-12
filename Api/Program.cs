using Api;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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