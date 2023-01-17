using PSOENotificaciones.Interfaces;
using PSOENotificaciones.Contexto;
using System;
using System.Data;
using System.Data.SqlClient;

namespace PSOENotificaciones.Servicios
{
    public class PsoeRespuestasLocalizaServicio : IPsoeRespuestasLocaliza
    {
        private readonly string ConnectionString = Servicios.Properties.Settings.Default.conexionPSOE_GestNotif;

        public DataTable ActulizarEstadosEnvios()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("identificador");

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
                    cmd.CommandText = "updateEstadoNotificaciones";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    using (SqlDataReader sqlReader = cmd.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            DataRow row = dt.NewRow();
                            row.ItemArray = new object[] { sqlReader[0].ToString() };
                            dt.Rows.Add(row);
                        }

                        if (sqlReader.NextResult())
                        {
                            while (sqlReader.Read())
                            {
                                DataRow row = dt.NewRow();
                                row.ItemArray = new object[] { sqlReader[0].ToString() };
                                dt.Rows.Add(row);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, 0);
            }

            return dt;
        }
    }
}
