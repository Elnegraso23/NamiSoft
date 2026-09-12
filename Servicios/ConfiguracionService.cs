using NominaPro.Datos;
using NominaPro.Modelos;

namespace NominaPro.Servicios;

public sealed class ConfiguracionService
{
    public ConfiguracionSistema ObtenerOCrear()
    {
        using var db = new NominaContext();
        var config = db.ConfiguracionSistema.FirstOrDefault();
        if (config is not null)
            return config;

        config = new ConfiguracionSistema
        {
            EntradaManana = new TimeSpan(7, 0, 0),
            SalidaManana = new TimeSpan(17, 0, 0),
            ToleranciaManana = 10,
            EntradaTarde = new TimeSpan(12, 0, 0),
            SalidaTarde = new TimeSpan(22, 0, 0),
            ToleranciaTarde = 10,
            CadaRetardos = 3,
            DescuentoRetardosMinutos = 30,
            AplicarPrimaDominical = true,
            PorcentajePrimaDominical = 25
        };

        db.ConfiguracionSistema.Add(config);
        db.SaveChanges();
        return config;
    }

    public void Guardar(ConfiguracionSistema value)
    {
        using var db = new NominaContext();
        var config = db.ConfiguracionSistema.FirstOrDefault();
        if (config is null)
        {
            db.ConfiguracionSistema.Add(value);
        }
        else
        {
            config.EntradaManana = value.EntradaManana;
            config.SalidaManana = value.SalidaManana;
            config.ToleranciaManana = value.ToleranciaManana;
            config.EntradaTarde = value.EntradaTarde;
            config.SalidaTarde = value.SalidaTarde;
            config.ToleranciaTarde = value.ToleranciaTarde;
            config.CadaRetardos = value.CadaRetardos;
            config.DescuentoRetardosMinutos = value.DescuentoRetardosMinutos;
            config.AplicarPrimaDominical = value.AplicarPrimaDominical;
            config.PorcentajePrimaDominical = value.PorcentajePrimaDominical;
        }
        db.SaveChanges();
    }
}
