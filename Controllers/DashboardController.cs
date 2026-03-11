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

        [HttpGet]
        public IActionResult Detalle(string acronimo)
        {
            var reportes = ObtenerReportesDetalle();
            var reporte = reportes.FirstOrDefault(r => r.Acronimo == acronimo);

            if (reporte == null)
                return NotFound();

            var model = new ReporteCompletoViewModel
            {
                Reporte = reporte,
                Acuses = ObtenerAcuses(reporte.Acronimo),
                ProximosEventos = ObtenerEventosCalendario(reporte.Acronimo),
                Estadisticas = CalcularEstadisticasReporte(reporte.Acronimo),
                HistorialEnvios = ObtenerHistorialEnvios(reporte.Acronimo)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult DescargarAcuse(int id)
        {
            // Lógica para descargar el acuse
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "acuses", "archivo_ejemplo.pdf");
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", "acuse.pdf");
        }



        private List<ReporteDetalle> ObtenerReportesDetalle()
        {
            var random = new Random();
            var reportesBase = ObtenerReportes();
            var reportesDetalle = new List<ReporteDetalle>();

            foreach (var reporte in reportesBase)
            {
                var fechaHoy = DateTime.Now;
                var diasAleatorios = random.Next(-10, 30);

                reportesDetalle.Add(new ReporteDetalle
                {
                    Acronimo = reporte.Acronimo,
                    NombreReporte = reporte.NombreReporte,
                    Autoridad = reporte.Autoridad,
                    NombreCorto = reporte.NombreCorto,
                    Entidad = reporte.Entidad,
                    Tema = reporte.Tema,
                    Periodicidad = reporte.Periodicidad,
                    N3Generacion = reporte.N3Generacion,
                    FechaEnvio = diasAleatorios < 0 ? fechaHoy.AddDays(diasAleatorios) : (DateTime?)null,
                    FechaProximoEnvio = fechaHoy.AddDays(random.Next(1, 30)),
                    Enviado = diasAleatorios < 0,
                    AcusePath = diasAleatorios < 0 ? "/acuses/ejemplo.pdf" : null,
                    Comentarios = "Reporte en proceso de revisión",
                    Estatus = diasAleatorios < 0 ? "Enviado" :
                              (diasAleatorios < 5 ? "Por Vencer" : "Pendiente")
                });
            }

            return reportesDetalle;
        }



        [HttpPost]
        public IActionResult RegistrarEnvio(string acronimo, DateTime fechaEnvio)
        {
            // Lógica para registrar el envío
            TempData["Success"] = "Envío registrado correctamente";
            return RedirectToAction("Detalle", new { acronimo });
        }


        [HttpPost]
        public async Task<IActionResult> SubirAcuse(string acronimo, IFormFile archivo)
        {
            if (archivo != null && archivo.Length > 0)
            {
                // Lógica para guardar el archivo
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "acuses");
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var fileName = $"{acronimo}_{DateTime.Now:yyyyMMddHHmmss}_{archivo.FileName}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                // Aquí guardarías en BD la referencia
                TempData["Success"] = "Acuse subido correctamente";
            }

            return RedirectToAction("Detalle", new { acronimo });
        }

        private EstadisticasEnvio CalcularEstadisticasReporte(string acronimo)
        {
            return new EstadisticasEnvio
            {
                TotalEnvios = 24,
                Enviados = 18,
                Pendientes = 4,
                Atrasados = 2,
                PorcentajeExito = 75.0,
                DíasPromedioAnticipacion = 5
            };
        }

        private List<DocumentoAcuse> ObtenerAcuses(string acronimo)
        {
            return new List<DocumentoAcuse>
            {
                new DocumentoAcuse
                {
                    Id = 1,
                    NombreArchivo = "Acuse_CNBV_2024.pdf",
                    FechaSubida = DateTime.Now.AddDays(-5),
                    TipoArchivo = "PDF",
                    Tamaño = 245760,
                    UsuarioSubio = "Juan Carlos Pérez",
                    RutaArchivo = "/acuses/ejemplo.pdf"
                },
                new DocumentoAcuse
                {
                    Id = 2,
                    NombreArchivo = "Comprobante_BANXICO.pdf",
                    FechaSubida = DateTime.Now.AddDays(-2),
                    TipoArchivo = "PDF",
                    Tamaño = 182345,
                    UsuarioSubio = "María González",
                    RutaArchivo = "/acuses/ejemplo2.pdf"
                }
            };
        }

        private List<EventoCalendario> ObtenerEventosCalendario(string acronimo)
        {
            var fechaHoy = DateTime.Now;
            return new List<EventoCalendario>
            {
                new EventoCalendario { Fecha = fechaHoy.AddDays(3), Titulo = "Vencimiento reporte mensual", Tipo = "vencimiento", Reporte = "ACLME", Acronimo = acronimo, Completado = false },
                new EventoCalendario { Fecha = fechaHoy.AddDays(5), Titulo = "Fecha límite de envío", Tipo = "envio", Reporte = "ACM", Acronimo = acronimo, Completado = false },
                new EventoCalendario { Fecha = fechaHoy.AddDays(-2), Titulo = "Reporte enviado", Tipo = "envio", Reporte = "BURÓ DIARIO", Acronimo = acronimo, Completado = true },
                new EventoCalendario { Fecha = fechaHoy.AddDays(10), Titulo = "Revisión trimestral", Tipo = "recordatorio", Reporte = "RNIE", Acronimo = acronimo, Completado = false },
                new EventoCalendario { Fecha = fechaHoy.AddDays(15), Titulo = "Junta de seguimiento", Tipo = "recordatorio", Reporte = "Varios", Acronimo = acronimo, Completado = false }
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

        private List<HistorialEnvio> ObtenerHistorialEnvios(string acronimo)
        {
            return new List<HistorialEnvio>
            {
                new HistorialEnvio { Fecha = DateTime.Now.AddMonths(-1), Estatus = "Enviado", Usuario = "Juan Carlos Pérez", Observaciones = "Envío dentro del plazo", AcuseGenerado = true },
                new HistorialEnvio { Fecha = DateTime.Now.AddMonths(-2), Estatus = "Enviado", Usuario = "María González", Observaciones = "Con observaciones menores", AcuseGenerado = true },
                new HistorialEnvio { Fecha = DateTime.Now.AddMonths(-3), Estatus = "Atrasado", Usuario = "Juan Carlos Pérez", Observaciones = "Se solicitó prórroga", AcuseGenerado = false },
                new HistorialEnvio { Fecha = DateTime.Now.AddMonths(-4), Estatus = "Enviado", Usuario = "Roberto Méndez", Observaciones = "Todo correcto", AcuseGenerado = true }
            };
        }

        private List<ReporteDetalle> ObtenerReportes()
        {
            return new List<ReporteDetalle>
            {
                new ReporteDetalle { Acronimo = "ACLME", NombreReporte = "Act. y Pas. en M.E. y Posición en Divisas", Autoridad = "BANXICO", NombreCorto = "Act. y Pas. en M.E. y Posición en Divisas", Entidad = "Casa de Bolsa", Tema = "Operativo", Periodicidad = "Diaria", N3Generacion = "Juan Rodriguez" },
                new ReporteDetalle { Acronimo = "ACLME", NombreReporte = "Act. y Pas. en M.E. y Posición en Divisas", Autoridad = "BANXICO", NombreCorto = "Act. y Pas. en M.E. y Posición en Divisas", Entidad = "Banco", Tema = "Operativo", Periodicidad = "Diaria", N3Generacion = "Juan Rodriguez" },
                new ReporteDetalle { Acronimo = "ACM", NombreReporte = "Anexo del Catálogo Mínimo", Autoridad = "BANXICO", NombreCorto = "Anexo del Catálogo Mínimo", Entidad = "Banco", Tema = "Financiero / Contable", Periodicidad = "Mensual", N3Generacion = "Juan Rodriguez" },
                new ReporteDetalle { Acronimo = "RNIE", NombreReporte = "Aviso de Actualización al Registro Nacional de Inversiones Extranjeras", Autoridad = "SE", NombreCorto = "Aviso de Actualización RNIE", Entidad = "Operadora de Fondos", Tema = "Inversión Extranjera", Periodicidad = "Trimestral", N3Generacion = "Hugo Rondero" },
                new ReporteDetalle { Acronimo = "BURÓ DIARIO", NombreReporte = "Buró Diario", Autoridad = "BURÓ", NombreCorto = "Buró Diario", Entidad = "Banco", Tema = "Operativo", Periodicidad = "Diaria", N3Generacion = "Juan Rodriguez" },
                new ReporteDetalle { Acronimo = "BURÓ PF", NombreReporte = "Buró Personas Físicas", Autoridad = "BURÓ", NombreCorto = "Buró Personas Físicas", Entidad = "Banco", Tema = "Operativo", Periodicidad = "Mensual", N3Generacion = "Juan Rodriguez" }
            };
        }
    }
}
