using Microsoft.AspNetCore.Mvc;
using RR.Models;

namespace RR.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var model = new DashboardView2Model
            {
                Reportes = ObtenerReportes(),
                Autoridades = CalcularEstadisticas(ObtenerReportes(), "Autoridad"),
                Frecuencias = CalcularEstadisticas(ObtenerReportes(), "Periodicidad"),
                Directores = CalcularEstadisticas(ObtenerReportes(), "N3Generacion"),
                Temas = CalcularEstadisticas(ObtenerReportes(), "Tema"),
                TotalReportes = 326
            };

            return View(model);
        }

        private List<ReporteDetalle> ObtenerReportes()
        {
            return new List<ReporteDetalle>
            {
                new ReporteDetalle { Acronimo = "ACLME", NombreReporte = "Act. y Pas. en M.E. y Posición en Divisas", Autoridad = "BANXICO", NombreCorto = "Act. y Pas. en M.E. y Posición en Divisas", Entidad = "Casa de Bolsa", Tema = "Operativo", Periodicidad = "Diaria", N3Generacion = "Juan Rodriguez" },
                new ReporteDetalle { Acronimo = "ACLME", NombreReporte = "Act. y Pas. en M.E. y Posición en Divisas", Autoridad = "BANXICO", NombreCorto = "Act. y Pas. en M.E. y Posición en Divisas", Entidad = "Banco", Tema = "Operativo", Periodicidad = "Diaria", N3Generacion = "Juan Rodriguez" },
                new ReporteDetalle { Acronimo = "ACM", NombreReporte = "Anexo del Catálogo Mínimo", Autoridad = "BANXICO", NombreCorto = "Anexo del Catálogo Mínimo", Entidad = "Banco", Tema = "Financiero / Contable", Periodicidad = "Mensual", N3Generacion = "Juan Rodriguez" },
                new ReporteDetalle { Acronimo = "Aviso de Actualización (RNIE)", NombreReporte = "Aviso de Actualización al Registro Nacional de Inversiones Extranjeras", Autoridad = "SE", NombreCorto = "Aviso de Actualización al Registro Nacional de Inversiones Extranjeras", Entidad = "Operadora de Fondos", Tema = "Inversión Extranjera", Periodicidad = "Trimestral", N3Generacion = "Hugo Rondero" },
                new ReporteDetalle { Acronimo = "Aviso de Actualización (RNIE)", NombreReporte = "Aviso de Actualización al Registro Nacional de Inversiones Extranjeras", Autoridad = "SE", NombreCorto = "Aviso de Actualización al Registro Nacional de Inversiones Extranjeras", Entidad = "Fondos de Inversión", Tema = "Inversión Extranjera", Periodicidad = "Trimestral", N3Generacion = "Hugo Rondero" },
                new ReporteDetalle { Acronimo = "Aviso de Actualización (RNIE)", NombreReporte = "Aviso de Actualización al Registro Nacional de Inversiones Extranjeras", Autoridad = "SE", NombreCorto = "Aviso de Actualización al Registro Nacional de Inversiones Extranjeras", Entidad = "Banco", Tema = "Inversión Extranjera", Periodicidad = "Trimestral", N3Generacion = "Hugo Rondero" },
                new ReporteDetalle { Acronimo = "Aviso de Actualización (RNIE)", NombreReporte = "Aviso de Actualización al Registro Nacional de Inversiones Extranjeras", Autoridad = "SE", NombreCorto = "Aviso de Actualización al Registro Nacional de Inversiones Extranjeras", Entidad = "Sociedad Controladora", Tema = "Inversión Extranjera", Periodicidad = "Trimestral", N3Generacion = "Hugo Rondero" },
                new ReporteDetalle { Acronimo = "Aviso de Actualización (RNIE)", NombreReporte = "Aviso de Actualización al Registro Nacional de Inversiones Extranjeras", Autoridad = "SE", NombreCorto = "Aviso de Actualización al Registro Nacional de Inversiones Extranjeras", Entidad = "Casa de Bolsa", Tema = "Inversión Extranjera", Periodicidad = "Trimestral", N3Generacion = "Hugo Rondero" },
                new ReporteDetalle { Acronimo = "BURÓ DIARIO", NombreReporte = "Buró Diario", Autoridad = "BURÓ", NombreCorto = "Buró Diario", Entidad = "Banco", Tema = "Operativo", Periodicidad = "Diaria", N3Generacion = "Juan Rodriguez" },
                new ReporteDetalle { Acronimo = "BURÓ PERSONAS FISICAS", NombreReporte = "Buró Personas Físicas", Autoridad = "BURÓ", NombreCorto = "Buró Personas Físicas", Entidad = "Banco", Tema = "Operativo", Periodicidad = "Mensual", N3Generacion = "Juan Rodriguez" },
                new ReporteDetalle { Acronimo = "BURÓ PERSONAS FISICAS", NombreReporte = "Buró Personas Físicas", Autoridad = "BURÓ", NombreCorto = "Buró Personas Físicas", Entidad = "Pensiones", Tema = "Operativo", Periodicidad = "Mensual", N3Generacion = "Miguel Mike" },
                new ReporteDetalle { Acronimo = "BURÓ PERSONAS FISICAS Complemento", NombreReporte = "Buró Personas Físicas Complemento", Autoridad = "BURÓ", NombreCorto = "Buró Personas Físicas Complemento", Entidad = "Banco", Tema = "Operativo", Periodicidad = "Mensual", N3Generacion = "Juan Rodriguez" }
            };
        }

        private List<EstadisticaItem> CalcularEstadisticas(List<ReporteDetalle> reportes, string propiedad)
        {
            var grupos = propiedad switch
            {
                "Autoridad" => reportes.GroupBy(x => x.Autoridad),
                "Periodicidad" => reportes.GroupBy(x => x.Periodicidad),
                "N3Generacion" => reportes.GroupBy(x => x.N3Generacion),
                "Tema" => reportes.GroupBy(x => x.Tema),
                _ => null
            };

            if (grupos == null) return new List<EstadisticaItem>();

            var total = reportes.Count;
            var items = new List<EstadisticaItem>();
            var orden = 1;

            foreach (var grupo in grupos.OrderByDescending(g => g.Count()))
            {
                items.Add(new EstadisticaItem
                {
                    Nombre = grupo.Key,
                    Cantidad = grupo.Count(),
                    Porcentaje = Math.Round((double)grupo.Count() / total * 100, 2),
                    Orden = orden++
                });
            }

            return items;
        }
    }
}
