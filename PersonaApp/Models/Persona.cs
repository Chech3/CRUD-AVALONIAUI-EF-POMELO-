using System;

namespace PersonaApp.Models;

public class Persona
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public DateTimeOffset FechaNacimiento { get; set; }
    public string Genero { get; set; }
    public bool AceptaTerminos { get; set; }
    
}