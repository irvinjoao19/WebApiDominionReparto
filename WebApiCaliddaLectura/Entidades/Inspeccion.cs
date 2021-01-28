﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Inspeccion
    {
        public int id { get; set; }
        public int inspeccionId { get; set; }
        public string nroInspeccion { get; set; }
        public string fecha { get; set; }
        public int operarioLecturaId { get; set; }
        public int clienteId { get; set; }
        public int areaId { get; set; }
        public string titulo { get; set; }
        public string lugar { get; set; }
        public int chkCharlaPre { get; set; }
        public int chkRevisarEpp { get; set; }
        public int estado { get; set; }
        public int usuarioId { get; set; }
        public int identity { get; set; }


        public string nombreArea { get; set; }
        public int trasladoId { get; set; }
        public int vehiculoId { get; set; }
        public string nombreTraslado { get; set; }
        public string placa { get; set; }
        public string marca { get; set; }         
        public string kilometraje { get; set; }
        public string nombreOperario { get; set; }


        public List<InspeccionDetalle> detalles { get; set; }
        public List<InspeccionAdicionales> adicionales { get; set; }         
    }
}
