using System;
using System.Collections.Generic;
using System.Text;

namespace ComparaBooks
{
    public class Award
    {
        public enum TipoAward { Ganadora, Nominada, Nada };

        public string Premio { get; set; }
        //public bool Ganadora { get; set; }
        public TipoAward TipoPremio { get; set; }

        public Award()
        {
            this.Premio = string.Empty;
            this.TipoPremio = TipoAward.Nada;
        }
    }
}
