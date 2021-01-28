using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class RepartoCargo
    {
        public int cargoId { get; set; }
        public int tipoCargoId { get; set; }
        public int repartoId { get; set; }

        public string suministroNumero { get; set; }
        public string nombreApellido { get; set; }
        public string dni { get; set; }
        public string predio { get; set; }
        public int quienRecibeCargo { get; set; }
        public string nombreQuienRecibe { get; set; }
        public string nombreEmpresa { get; set; }
        public string lecturaCargo { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string fecha { get; set; }
        public string hora { get; set; }
        public string fechaMovil { get; set; }
        public int estado { get; set; }
        public List<RepartoCargoFoto> photos { get; set; }
        public List<RepartoCargoSuministro> suministros { get; set; }
    }
}
