namespace NamiSoft.Enumeraciones;

/// <summary>
/// De dónde proviene una checada. El motor de nómina nunca distingue por origen;
/// solo trabaja con checadas normalizadas. Guardado como entero en BD.
/// </summary>
public enum OrigenChecada
{
    Manual = 0,
    Sistema = 1,
    ImportacionExcel = 2,
    ImportacionCsv = 3,
    Dispositivo = 4
}
