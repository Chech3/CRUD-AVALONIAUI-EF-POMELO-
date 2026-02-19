using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PersonaApp.Data;
using PersonaApp.Models;

namespace PersonaApp.Service;

public class PersonaService
{
    private readonly AppDbContext _db = new AppDbContext();

    public PersonaService()
    {
        try 
        {
            // Crea la base de datos y tablas si no existen según los modelos
            _db.Database.EnsureCreated();
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al inicializar DB: {ex.Message}");
        }
    }

    public List<Persona> ObtenerPersona()
    {
        try 
        {
            return _db.Personas.AsNoTracking().ToList();
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al obtener personas: {ex.Message}");
            return new List<Persona>();
        }
    }

    public void AgregarPersona(Persona persona)
    {
        _db.Personas.Add(persona);
        _db.SaveChanges();
    }

    public void ActualizarPersona(Persona persona)
    {
        try
        {
            var tracked = _db.Personas.Local.FirstOrDefault(p => p.Id == persona.Id);
            if (tracked != null && tracked != persona)
            {
                _db.Entry(tracked).State = EntityState.Detached;
            }

            _db.Personas.Update(persona);
            _db.SaveChanges();
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al actualizar: {ex.Message}");
        }
    }

    public void EliminarPersona(Persona persona)
    {
        try
        {
            var entidad = _db.Personas.Local.FirstOrDefault(p => p.Id == persona.Id)
                          ?? _db.Personas.Find(persona.Id);

            if (entidad != null)
            {
                _db.Personas.Remove(entidad);
                _db.SaveChanges();
            }
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al eliminar: {ex.Message}");
        }
    }
}