using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RR.Models;

namespace RR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Datos de ejemplo (en tu caso vendrán de BD)
        private static List<Venta> ObtenerDatos()
        {
            //return new List<Reporte>() {
            //    new Reporte() {
            //        Id = 1,
            //        Acronimo = "ACLME",
            //        Nombre = "Act y Pas en ME",
            //        Autoridad = "BANXICO",
            //        Tema = "Operativo",
            //        Periodicidad = "Diaria"
            //    },
            //    new Reporte() {
            //        Id = 2,
            //        Acronimo = "ACM",
            //        Nombre = "Anexo Catalogo Min",
            //        Autoridad = "BANXICO",
            //        Tema = "Financiero",
            //        Periodicidad = "Mensual"
            //    },
            //    new Reporte() {
            //        Id = 3,
            //        Acronimo = "CVT",
            //        Nombre = "Compra Venta Titulos",
            //        Autoridad = "BANXICO",
            //        Tema = "Operativo",
            //        Periodicidad = "Diario"
            //    }
            //};



            return new List<Venta>
            {
                new Venta { Id = 1, Categoria = "Banco", Producto = "ACLME", Cantidad = 1, Monto = 1, Fecha = DateTime.Now.AddDays(-1), Acronimo="ACLME", Nombre="Activo Pasivo", Autoridad="BANXICO", Entidad = "Banco", Periodicidad="Diaria", Tema="Operativo" },
                new Venta { Id = 2, Categoria = "Banco", Producto = "Monitor 4K", Cantidad = 1, Monto = 1, Fecha = DateTime.Now.AddDays(-2), Acronimo="ACM", Nombre="Anexo Catalog Minimo", Autoridad="BANXICO", Entidad = "Banco", Periodicidad="Mensual", Tema="Financiero" },
                new Venta { Id = 3, Categoria = "Casa de Bolsa", Producto = "Mouse", Cantidad = 1, Monto = 1, Fecha = DateTime.Now.AddDays(-3), Acronimo="CVT", Nombre="Compra Venta Titulos", Autoridad="BANXICO", Entidad = "Casa de Bolsa", Periodicidad="Diaria", Tema="Operativo" },
                new Venta { Id = 4, Categoria = "Casa de Bolsa", Producto = "Camiseta Premium", Cantidad = 1, Monto = 1, Fecha = DateTime.Now.AddDays(-1), Acronimo="DC", Nombre="Derivados credito", Autoridad="BANXICO", Entidad = "Casa de Bolsa", Periodicidad="Diaria", Tema="Operativo" },
                new Venta { Id = 5, Categoria = "Seguros", Producto = "Jeans", Cantidad = 1, Monto = 1, Fecha = DateTime.Now.AddDays(-3), Acronimo="CONTRAPARTES", Nombre="Contrapartes", Autoridad="BANXICO", Entidad = "Seguros", Periodicidad="Por Evento", Tema="Operativo" },
                new Venta { Id = 6, Categoria = "Seguros", Producto = "Chaqueta", Cantidad = 1, Monto = 1, Fecha = DateTime.Now.AddDays(-5), Acronimo="IFIT", Nombre="Ingreso Fichas Tecnicas", Autoridad="CONDUSEF", Entidad = "Seguros", Periodicidad="Mensual", Tema="Registro Productos" },
                new Venta { Id = 7, Categoria = "Banco", Producto = "Sofá", Cantidad = 1, Monto = 1, Fecha = DateTime.Now.AddDays(-1), Acronimo="CCV", Nombre="Caja y captacion", Autoridad="BANXICO", Entidad = "Banco", Periodicidad="Diaria", Tema="Operativo" },
                new Venta { Id = 8, Categoria = "Casa de Bolsa", Producto = "Mesa", Cantidad = 1, Monto = 1, Fecha = DateTime.Now.AddDays(-2), Acronimo="OD", Nombre="Operaciones con Dolares", Autoridad="BANXICO", Entidad = "Casa de Bolsa", Periodicidad="Mensual", Tema="Operativo" },
                new Venta { Id = 9, Categoria = "Seguros", Producto = "Silla", Cantidad = 1, Monto = 1, Fecha = DateTime.Now.AddDays(-4), Acronimo="PLD Relevantes", Nombre="PLD Relevantes", Autoridad="SHCP", Entidad = "Seguros", Periodicidad="Trimestral", Tema="PLD" }
            };
        }

        //public IActionResult Index()
        //{
        //    var ventas = ObtenerDatos();

        //    var ventasPorCategoria = ventas
        //        .GroupBy(v => v.Categoria)
        //        .ToDictionary(g => g.Key, g => (double)g.Sum(v => v.Monto));

        //    var viewModel = new DashboardViewModel
        //    {
        //        VentasPorCategoria = ventasPorCategoria,
        //        DetalleVentas = ventas, // Mostrar todas inicialmente
        //        CategoriaSeleccionada = "Todas"
        //    };

        //    return View(viewModel);
        //}

        public IActionResult Index()
        {
            var ventas = ObtenerDatos();

            var ventasPorCategoria = ventas
                .GroupBy(v => v.Categoria)
                .ToDictionary(g => g.Key, g => (double)g.Sum(v => v.Monto));

            var viewModel = new DashboardViewModel
            {
                VentasPorCategoria = ventasPorCategoria,
                DetalleVentas = ventas, // Mostrar todas inicialmente
                CategoriaSeleccionada = "Todas"
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult FiltrarPorCategoria(string categoria)
        {
            var ventas = ObtenerDatos();

            DashboardViewModel viewModel;

            if (categoria == "Todas" || string.IsNullOrEmpty(categoria))
            {
                viewModel = new DashboardViewModel
                {
                    VentasPorCategoria = ventas
                        .GroupBy(v => v.Categoria)
                        .ToDictionary(g => g.Key, g => (double)g.Sum(v => v.Monto)),
                    DetalleVentas = ventas,
                    CategoriaSeleccionada = "Todas"
                };
            }
            else
            {
                var ventasFiltradas = ventas
                    .Where(v => v.Categoria == categoria)
                    .ToList();

                viewModel = new DashboardViewModel
                {
                    VentasPorCategoria = ventas
                        .GroupBy(v => v.Categoria)
                        .ToDictionary(g => g.Key, g => (double)g.Sum(v => v.Monto)),
                    DetalleVentas = ventasFiltradas,
                    CategoriaSeleccionada = categoria
                };
            }

            return PartialView("_TablaDetalle", viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
