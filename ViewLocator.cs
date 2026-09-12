using Avalonia.Controls;
using Avalonia.Controls.Templates;
using NominaPro.ViewModels;
using System;

namespace NominaPro;

public sealed class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var name = param.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type is not null)
            return (Control)Activator.CreateInstance(type)!;

        return new TextBlock { Text = $"No se encontró la vista para {name}." };
    }

    public bool Match(object? data) => data is ViewModelBase;
}
