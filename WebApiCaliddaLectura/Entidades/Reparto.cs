using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Reparto
    {
        public int id_Reparto { get; set; }
        public int id_Operario_Reparto { get; set; }
        public int foto_Reparto { get; set; }
        public int id_observacion { get; set; }
        public string Suministro_Medidor_reparto { get; set; }
        public string Suministro_Numero_reparto { get; set; }
        public string Direccion_Reparto { get; set; }
        public string Cod_Orden_Reparto { get; set; }

        public string Cod_Actividad_Reparto { get; set; }

        public string Cliente_Reparto { get; set; }
        public string CodigoBarra { get; set; }
        public int estado { get; set; }
        public int activo { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
    }

}
