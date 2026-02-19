using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PersonaApp.Models;

public class Persona : INotifyPropertyChanged
{
    private int _id;
    private string _nombre = string.Empty;
    private DateTimeOffset _fechaNacimiento;
    private string _genero = string.Empty;
    private bool _aceptaTerminos;

    public int Id
    {
        get => _id;
        set { _id = value; OnPropertyChanged(); }
    }

    public string Nombre
    {
        get => _nombre;
        set { _nombre = value; OnPropertyChanged(); }
    }

    public DateTimeOffset FechaNacimiento
    {
        get => _fechaNacimiento;
        set { _fechaNacimiento = value; OnPropertyChanged(); }
    }

    public string Genero
    {
        get => _genero;
        set { _genero = value; OnPropertyChanged(); }
    }

    public bool AceptaTerminos
    {
        get => _aceptaTerminos;
        set { _aceptaTerminos = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}