using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PersonaApp.Data;
using PersonaApp.Models;

namespace PersonaApp.Service;

public class PersonaService
{
    private readonly AppDbContext _db = new AppDbContext();

    public List<Persona> ObtenerPersona()
    {
        return _db.Personas.AsNoTracking().ToList();
    }

    public void AgregarPersona(Persona persona)
    {
        _db.Personas.Add(persona);
        _db.SaveChanges();
    }

    public void ActualizarPersona(Persona persona)
    {
        using var context = new AppDbContext();
        context.Attach(persona);
        context.Entry(persona).State = EntityState.Modified;
        context.SaveChanges();
    }

    public void EliminarPersona(Persona persona)
    {
        var entidad = _db.Personas.FirstOrDefault(p => p.Id == persona.Id)
                      ?? _db.Personas.Find(persona.Id);

        if (entidad != null)
        {
            _db.Personas.Remove(entidad);
            _db.SaveChanges();
        }
    }
}