using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Vehiculo
    {
        public int vehiculoId { get; set; }
        public string  placa  { get; set; }
        public string  marca  { get; set; }
        public string  modelo  { get; set; }
        public string  anioFabricacion { get; set; }
        public string  cilindrada  { get; set; }
        public string  inspeccionTecnica  { get; set; }
        public string  soat  { get; set; }        
        public int estado { get; set; }        
    }
}
