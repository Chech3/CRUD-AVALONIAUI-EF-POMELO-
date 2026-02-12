using System.Collections.Specialized;
using PersonaApp.Service;

namespace PersonaApp.ViewModels;

public class PersonaViewModel: INotifyCollectionChanged
{
    private readonly PersonaService _service =new PersonaService();
}