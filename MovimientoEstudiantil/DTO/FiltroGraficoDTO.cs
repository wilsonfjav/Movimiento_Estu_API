namespace MovimientoEstudiantil.DTO
{
    public class FiltroGraficoDTO
    {
        public int AnioInicio { get; set; }
        public int AnioFin { get; set; }

        public string Provincia { get; set; }
        public string Sede { get; set; }

        public string TrasladoResidencia { get; set; } // "Sí", "No", ""
        public string IngresoCarreraDeseada { get; set; } // "Sí", "No", ""
        public string TipoGrafico { get; set; } // bar, pie, doughnut, etc.
    }
}