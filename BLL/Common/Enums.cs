using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Common
{
    public class Enums
    {
        public enum Accion { Adicionar, Consultar, Eliminar, Modificar, Acceso }
        public enum TipoDeMensaje { Error = 1, Advertencia = 2, Informacion = 3, Exito = 4 };

        public enum TipoVuelo { Unico = 0, Multiple = 1, Retorno =2}

    }
}
