using PSOENotificaciones.Controllers;
using PSOENotificaciones.Contexto;
using System;

namespace PSOENotificaciones.TareaLocaliza
{
    class Program
    {
        static void Main(string[] args)
        {
            LogTareaLocaliza log = new LogTareaLocaliza();
            log.InsertLog("EJECUCIÓN PSOE_Tarea_Localiza");

            DateTime fechaHoraActual = DateTime.Now;

            DateTime fechaHoy = new DateTime(fechaHoraActual.Year, fechaHoraActual.Month, fechaHoraActual.Day);
            LogTareaLocaliza objLog = log.GetLogHoy(fechaHoy);

            if (objLog == null)
            {
                ConfiguracionPeticiones cp = new ConfiguracionPeticiones();
                int diaSemana = (int)fechaHoraActual.DayOfWeek;
                ConfiguracionPeticiones horario = cp.GetHorario(diaSemana);

                if (horario != null)
                {
                    TimeSpan horaActual = new TimeSpan(fechaHoraActual.Hour, fechaHoraActual.Minute, 0);

                    //La tarea de 'localiza' se ejecuta cuando la hora es igual o mayor y la tarea no se ha ejecutado en el día de hoy
                    if (horaActual >= horario.Horario)
                    {
                        TareasController.EjecutarTareaLocalizaAutomatico();
                    }
                }
            }
        }
    }
}
