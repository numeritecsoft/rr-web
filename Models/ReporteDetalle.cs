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
