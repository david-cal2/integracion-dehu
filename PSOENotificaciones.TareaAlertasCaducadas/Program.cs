using PSOENotificaciones.Controllers;
using PSOENotificaciones.Contexto;

namespace PSOENotificaciones.TareaAlertasCaducadas
{
    class Program
    {
        static void Main(string[] args)
        {
            LogTareaAlertasCaducadas log = new LogTareaAlertasCaducadas();
            log.InsertLog("EJECUCIÓN PSOE_Tarea_AlertasCaducadas");

            TareasController.EjecutarTareaAlertasCaducadas();
        }
    }
}
