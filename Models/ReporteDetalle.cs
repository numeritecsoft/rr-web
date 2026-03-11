namespace RR.Models
{
    public class ReporteDetalle
    {
        public string Acronimo { get; set; }
        public string NombreReporte { get; set; }
        public string Autoridad { get; set; }
        public string NombreCorto { get; set; }
        public string Entidad { get; set; }
        public string Tema { get; set; }
        public string Periodicidad { get; set; }
        public string N3Generacion { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public DateTime? FechaProximoEnvio { get; set; }
        public bool Enviado { get; set; }
        public string AcusePath { get; set; }
        public string Comentarios { get; set; }
        public string Estatus { get; set; } // "Pendiente", "Enviado", "Atrasado", "Por Vencer"
    }

    public class ReporteCompletoViewModel
    {
        public ReporteDetalle Reporte { get; set; }
        public List<DocumentoAcuse> Acuses { get; set; }
        public List<EventoCalendario> ProximosEventos { get; set; }
        public EstadisticasEnvio Estadisticas { get; set; }
        public List<HistorialEnvio> HistorialEnvios { get; set; }
    }

    public class DocumentoAcuse
    {
        public int Id { get; set; }
        public string NombreArchivo { get; set; }
        public DateTime FechaSubida { get; set; }
        public string TipoArchivo { get; set; }
        public long Tamaño { get; set; }
        public string UsuarioSubio { get; set; }
        public string RutaArchivo { get; set; }
    }

    public class EventoCalendario
    {
        public DateTime Fecha { get; set; }
        public string Titulo { get; set; }
        public string Tipo { get; set; } // "envio", "vencimiento", "recordatorio"
        public string Reporte { get; set; }
        public string Acronimo { get; set; }
        public bool Completado { get; set; }
    }

    public class EstadisticasEnvio
    {
        public int TotalEnvios { get; set; }
        public int Enviados { get; set; }
        public int Pendientes { get; set; }
        public int Atrasados { get; set; }
        public double PorcentajeExito { get; set; }
        public int DíasPromedioAnticipacion { get; set; }
    }

    public class HistorialEnvio
    {
        public DateTime Fecha { get; set; }
        public string Estatus { get; set; }
        public string Usuario { get; set; }
        public string Observaciones { get; set; }
        public bool AcuseGenerado { get; set; }
    }

    public class EstadisticaItem
    {
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public double Porcentaje { get; set; }
        public int Orden { get; set; }
    }

    public class DashboardView2Model
    {
        public List<ReporteDetalle> Reportes { get; set; }
        public List<EstadisticaItem> Autoridades { get; set; }
        public List<EstadisticaItem> Frecuencias { get; set; }
        public List<EstadisticaItem> Directores { get; set; }
        public List<EstadisticaItem> Temas { get; set; }
        public int TotalReportes { get; set; }
    }
}
