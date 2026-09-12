# Reglas para OpenCode / agentes

Este repositorio es la migración de NamiSoft a Avalonia.

## Stack obligatorio

- C#
- .NET 10 LTS
- Avalonia UI OSS
- MVVM
- SQLite + Entity Framework Core

## Licencias

No agregues paquetes comerciales, trialware, Avalonia Plus/Pro/Enterprise ni controles que requieran pago.
Antes de agregar una dependencia, confirma que tiene licencia permisiva y uso comercial gratuito (preferir MIT, Apache-2.0 o BSD).

## Arquitectura

- `Views/`: solo presentación y code-behind mínimo necesario para cerrar ventanas/dialogs.
- `ViewModels/`: estado y comandos de la pantalla.
- `Servicios/`: reglas de negocio, persistencia y operaciones reutilizables.
- `Modelos/`: dominio/DTOs.
- No acceder directamente a EF Core desde AXAML o code-behind.
- No introducir `System.Windows.Forms` ni WPF.

## Migración

El comportamiento del motor de nómina original es la referencia. No cambies fórmulas o reglas de negocio durante un cambio puramente visual.

## UI

- Diseñar para escritorio y uso táctil.
- Controles interactivos con altura mínima aproximada de 42 px.
- Evitar posiciones absolutas; usar `Grid`, `StackPanel`, `WrapPanel`, etc.
- La interfaz debe funcionar en Linux y Windows.
- No depender de `DataGrid` premium; las tablas actuales están construidas con controles base.

## Verificación mínima después de cambios

```bash
dotnet restore
dotnet build
dotnet run
```

Para cambios de lógica de nómina, agregar/ejecutar pruebas antes de aceptar la modificación.
