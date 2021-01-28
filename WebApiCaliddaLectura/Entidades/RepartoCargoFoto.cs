using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class RepartoCargoFoto
    {
        public int fotoCargoId { get; set; }
        public int cargoId { get; set; }
        public string rutaFoto { get; set; }
        public int tipo { get; set; }
        public int estado { get; set; }
    }
}
