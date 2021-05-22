using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Migracion
    {      
        public List<Servicio> servicios { get; set; }
        public List<Parametro> parametros { get; set; }       
        public List<Reparto> repartoLectura { get; set; }
        public List<FormatoCargo> formatos { get; set; }    
    }
}
