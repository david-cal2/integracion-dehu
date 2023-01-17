using PSOENotificaciones.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;

namespace PSOENotificaciones.Servicios
{
    public class PsoeHistorialesServicio : IPsoeHistoriales
    {
        private readonly string ConnectionString = Servicios.Properties.Settings.Default.conexionPSOE_GestNotif;

        public void InsertExcepcion(string excepcion, string traza, int? idUsuario)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString)
                {
                    ConnectTimeout = 2500
                };
                SqlConnection con = new SqlConnection(builder.ConnectionString);
                con.Open();

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "insertHistorialExcepciones";

                    cmd.Parameters.Add("@excepcion", SqlDbType.NVarChar, 255);
                    cmd.Parameters.Add("@traza", SqlDbType.VarChar);
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int);

                    cmd.Parameters["@excepcion"].Value = excepcion;
                    cmd.Parameters["@traza"].Value = traza;
                    cmd.Parameters["@idUsuario"].Value = (idUsuario ?? null);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
