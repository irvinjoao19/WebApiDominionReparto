using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class CheckList
    {
        public int checkListId { get; set; }
        public int formatoId { get; set; }
        public int titulo { get; set; }
        public int codigo { get; set; }
        public string descripcion { get; set; }
        public int orden { get; set; }
        public int aplicaFecha { get; set; }
        public int aplicaObs1 { get; set; }
        public int aplicaObs2 { get; set; }
        public int grupo { get; set; }
        public string campo1 { get; set; }
        public string campo2 { get; set; }
        public int estado { get; set; }
        public string valor1 { get; set; }
        public string valor2 { get; set; }
        public string valor3 { get; set; }        
        public string valor4 { get; set; }        
    }
}
