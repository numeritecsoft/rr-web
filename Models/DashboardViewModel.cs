namespace RR.Models
{
        public class DashboardViewModel
        {
            /// <summary>
            /// Diccionario con las categorías y sus valores totales (para el gráfico)
            /// </summary>
            public Dictionary<string, double> VentasPorCategoria { get; set; }
        

        /// <summary>
        /// Lista de ventas filtradas (para la tabla de detalle)
        /// </summary>
        public List<Venta> DetalleVentas { get; set; }
            public List<Reporte> DetalleReportes { get; set; }

        /// <summary>
        /// Categoría actualmente seleccionada (para mostrar en el título)
        /// </summary>
        public string CategoriaSeleccionada { get; set; }
        }

        // Modelo auxiliar para representar una venta
        public class Venta
        {
            public int Id { get; set; }
            public string Categoria { get; set; }
            public string Producto { get; set; }
            public int Cantidad { get; set; }
            public decimal Monto { get; set; }
            public DateTime Fecha { get; set; }
        public string Acronimo { get; set; }
        public string Nombre { get; set; }
        public string Autoridad { get; set; }
        public string Tema { get; set; }
        public string Periodicidad { get; set; }
        public string Entidad { get; set; }
    }

    public class Reporte
    {
        public int Id { get; set; }
        public string Acronimo { get; set; }
        public string Nombre { get; set; }
        public string Autoridad { get; set; }
        public string Tema { get; set; }
        public string Periodicidad { get; set; }
    }
}
