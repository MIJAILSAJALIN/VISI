namespace VISI.Models
{
    public class PaginacionViewModel
    {
        public int Pagina { get; set; } = 1;
        private int registrosPorPagina = 50;
        private readonly int MaxRegistrosPorPagina = 500;
        public int TotaldeRegistros { get; set; }

        public int RegistrosPorPagina
        {
            get { return registrosPorPagina; }
            set { registrosPorPagina = (value > MaxRegistrosPorPagina) ? MaxRegistrosPorPagina : value; }
        }
        public int RegistrosASaltar => registrosPorPagina * (Pagina - 1);
        public int TotaldePaginas => (int)Math.Ceiling((double)TotaldeRegistros / registrosPorPagina);
        public string BaseURL { get; set; }
    }

}