using System;
using System.Collections.Generic;
using System.Text;
using static BLL.Common.Enums;

namespace BLL.Common
{
    class Logs
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        protected static string ObtenerMensajeTecnico(Exception ex)
        {
            string Descripcion = string.Empty;
            if (ex != null)
            {
                Descripcion += "\n\n Mensaje técnico: \n" + ex.Message;
                Exception ExInnerException = ex.InnerException;
                while (ExInnerException != null)
                {
                    Descripcion += "\n InnerException: " + ExInnerException.Message;
                    ExInnerException = ExInnerException.InnerException;
                }
                if (ex.TargetSite != null)
                {
                    Descripcion += "\n TargetSite: " + ex.TargetSite.Name;
                }
                if (ex.StackTrace != null)
                {
                    Descripcion += "\n StackTrace: " + ex.StackTrace;
                }
            }
            return Descripcion;
        }

        protected static string MapMensajeUsuario(Accion Evento, TipoDeMensaje Acc)
        {
            string MensajeRet = string.Empty;
            if (Acc == TipoDeMensaje.Exito)
            {
                MensajeRet =
                    Evento == Accion.Adicionar ? "Registro creado"
                    : Evento == Accion.Consultar ? "Registros obtenidos"
                    : Evento == Accion.Eliminar ? "Registros eliminado"
                    : Evento == Accion.Modificar ? "Registros modificado"
                    : "Exíto";
            }
            else
            {
                MensajeRet =
                    Evento == Accion.Adicionar ? "No se logro adicionar el registro"
                    : Evento == Accion.Consultar ? "No se encontro el registro"
                    : Evento == Accion.Eliminar ? "No se logro eliminar el registro"
                    : Evento == Accion.Modificar ? "No se logro modificar el registro"
                    : Evento == Accion.Acceso ? "No logro acceder"
                    : "Error";
            }

            return MensajeRet;
        }
    }
}
