using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PersonaApp.Helpers;
using PersonaApp.Models;
using PersonaApp.Service;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PersonaApp.ViewModels;

public class PersonaViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string>> _errors = new();
    
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    public bool HasErrors => _errors.Any();

    public IEnumerable GetErrors(string? propertyName)
    {
        return _errors.GetValueOrDefault(propertyName ?? string.Empty, new List<string>());
    }

    private void ValidateProperty(string propertyName, object? value)
    {
        _errors.Remove(propertyName);

        switch (propertyName)
        {
            case nameof(Nombre):
                if (string.IsNullOrWhiteSpace(value as string))
                    AddError(propertyName, "El nombre es obligatorio.");
                else if ((value as string)?.Length < 3)
                    AddError(propertyName, "El nombre debe tener al menos 3 caracteres.");
                break;
            case nameof(Genero):
                if (string.IsNullOrWhiteSpace(value as string))
                    AddError(propertyName, "Debe seleccionar un género.");
                break;
            case nameof(AceptaTerminos):
                if (value is bool b && !b)
                    AddError(propertyName, "Debes aceptar los términos.");
                break;
        }

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(nameof(HasErrors));
        
        // Actualizar estado de los comandos
        (AgregarCommand as RelayCommand)?.RaiseCanExecuteChanged();
        (ActualizarCommand as RelayCommand)?.RaiseCanExecuteChanged();
    }

    private void AddError(string propertyName, string error)
    {
        if (!_errors.ContainsKey(propertyName))
            _errors[propertyName] = new List<string>();
        _errors[propertyName].Add(error);
    }

    private readonly PersonaService _service = new PersonaService();
    private ObservableCollection<Persona> _personas = new();
    private Persona? _selectedPersona;

    private string _nombre = string.Empty;
    private DateTimeOffset _fechaNacimiento = DateTimeOffset.Now;
    private string _genero = string.Empty;
    private bool _aceptaTerminos;

    public ObservableCollection<string> Generos { get; } = new() { "Masculino", "Femenino", "Otro" };

    public ObservableCollection<Persona> Personas
    {
        get => _personas;
        set { _personas = value; OnPropertyChanged(); }
    }

    public Persona? SelectedPersona
    {
        get => _selectedPersona;
        set
        {
            _selectedPersona = value;
            OnPropertyChanged();
            
            // Avisar a los comandos que su estado de "se puede ejecutar" ha cambiado
            (ActualizarCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (EliminarCommand as RelayCommand)?.RaiseCanExecuteChanged();

            if (_selectedPersona != null)
            {
                Nombre = _selectedPersona.Nombre;
                FechaNacimiento = _selectedPersona.FechaNacimiento;
                Genero = _selectedPersona.Genero;
                AceptaTerminos = _selectedPersona.AceptaTerminos;
            }
        }
    }

    public string Nombre
    {
        get => _nombre;
        set 
        { 
            _nombre = value; 
            OnPropertyChanged();
            ValidateProperty(nameof(Nombre), value);
        }
    }

    public DateTimeOffset FechaNacimiento
    {
        get => _fechaNacimiento;
        set { _fechaNacimiento = value; OnPropertyChanged(); }
    }

    public string Genero
    {
        get => _genero;
        set 
        { 
            _genero = value; 
            OnPropertyChanged();
            ValidateProperty(nameof(Genero), value);
        }
    }

    public bool AceptaTerminos
    {
        get => _aceptaTerminos;
        set 
        { 
            _aceptaTerminos = value; 
            OnPropertyChanged();
            ValidateProperty(nameof(AceptaTerminos), value);
        }
    }

    public ICommand AgregarCommand { get; }
    public ICommand ActualizarCommand { get; }
    public ICommand EliminarCommand { get; }
    public ICommand LimpiarCommand { get; }

    public PersonaViewModel()
    {
        Personas = new ObservableCollection<Persona>(_service.ObtenerPersona());
        AgregarCommand = new RelayCommand(Agregar, () => !HasErrors && !string.IsNullOrWhiteSpace(Nombre));
        ActualizarCommand = new RelayCommand(Actualizar, () => SelectedPersona != null && !HasErrors);
        EliminarCommand = new RelayCommand(Eliminar, () => SelectedPersona != null);
        LimpiarCommand = new RelayCommand(Limpiar);
    }

    private void Agregar()
    {
        if (string.IsNullOrWhiteSpace(Nombre)) return;

        var nuevaPersona = new Persona
        {
            Nombre = Nombre,
            FechaNacimiento = FechaNacimiento,
            Genero = Genero,
            AceptaTerminos = AceptaTerminos
        };
        _service.AgregarPersona(nuevaPersona);
        Personas.Add(nuevaPersona);
        Limpiar();
    }

    private void Actualizar()
    {
        if (SelectedPersona == null) return;

        SelectedPersona.Nombre = Nombre;
        SelectedPersona.FechaNacimiento = FechaNacimiento;
        SelectedPersona.Genero = Genero;
        SelectedPersona.AceptaTerminos = AceptaTerminos;

        _service.ActualizarPersona(SelectedPersona);
        Limpiar();
    }

    private void Eliminar()
    {
        if (SelectedPersona == null) return;
        _service.EliminarPersona(SelectedPersona);
        Personas.Remove(SelectedPersona);
        Limpiar();
    }

    private void Limpiar()
    {
        Nombre = string.Empty;
        FechaNacimiento = DateTimeOffset.Now;
        Genero = string.Empty;
        AceptaTerminos = false;
        
        // Limpiar errores al resetear el formulario
        _errors.Clear();
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(null));
        OnPropertyChanged(nameof(HasErrors));
        
        SelectedPersona = null;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}