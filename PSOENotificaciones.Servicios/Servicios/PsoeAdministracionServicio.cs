using PSOENotificaciones.Interfaces;
using PSOENotificaciones.Contexto;
using System;
using System.Data;
using System.Data.SqlClient;

namespace PSOENotificaciones.Servicios
{
    public class PsoeAdministracionServicio : IPsoeAdministracion
    {
        private readonly string ConnectionString = Servicios.Properties.Settings.Default.conexionPSOE_GestNotif;

        public DataTable LoginUsuario(string loginUsuario, string loginPassword, string hostName, string clienteIp)
        {
            DataTable dt = new DataTable();

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
                    cmd.CommandText = "getLoginUsuario";

                    cmd.Parameters.Add("@loginUsuario", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@loginPassword", SqlDbType.VarChar);
                    cmd.Parameters.Add("@hostName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@clientIP", SqlDbType.VarChar);

                    cmd.Parameters["@loginUsuario"].Value = loginUsuario;
                    cmd.Parameters["@loginPassword"].Value = loginPassword;
                    cmd.Parameters["@hostName"].Value = hostName;
                    cmd.Parameters["@clientIP"].Value = clienteIp;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    //sqlReader = cmd.ExecuteReader();

                    using (SqlDataReader sqlReader = cmd.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        { 
                            object[] fila = null;
                            if (sqlReader.FieldCount > 3)
                            {
                                dt.Columns.Add("ID");
                                dt.Columns.Add("LoginUsuario");
                                dt.Columns.Add("Email");
                                dt.Columns.Add("Perfiles_ID");
                                dt.Columns.Add("PantallasInicio_ID");
                                dt.Columns.Add("ConsentimientoLegal");
                                fila = new object[] { sqlReader[0].ToString(), sqlReader[1].ToString(), sqlReader[2].ToString(), sqlReader[3].ToString(), sqlReader[4].ToString(), sqlReader[5].ToString() };
                            }
                            else
                            {
                                dt.Columns.Add("IdMensajeError");
                                dt.Columns.Add("MensajeError");
                                dt.Columns.Add("IdUsuario");
                                fila = new object[] { sqlReader[0].ToString(), sqlReader[1].ToString(), sqlReader[2].ToString() };
                            }
                                
                            DataRow row = dt.NewRow();
                            row.ItemArray = fila;
                            dt.Rows.Add(row);
                            break;
                        }

                        cmd.Cancel();
                        sqlReader.Close();
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

        public bool UpdateUsuarioPassword(int idUsuario, string loginPassword)
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
                    cmd.CommandText = "updateUsuarioPassword";

                    cmd.Parameters.Add("@id", SqlDbType.Int);
                    cmd.Parameters.Add("@loginPassword", SqlDbType.VarChar);

                    cmd.Parameters["@id"].Value = idUsuario;
                    cmd.Parameters["@loginPassword"].Value = loginPassword;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, 0);
                return false;
            }
        }

        public string InsertUsuario(string nombre, string apellidos, string nif, string mail, string tlf, string login, int perId, string pass)
        {
            string respuesta = "";

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
                    cmd.CommandText = "insertUsuario";

                    cmd.Parameters.Add("@loginUsuario", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@loginPassword", SqlDbType.VarChar);
                   
                    cmd.Parameters.Add("@nif", SqlDbType.VarChar);
                    cmd.Parameters.Add("@telefono", SqlDbType.NVarChar, 50);

                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@apellidos", SqlDbType.NVarChar, 50);
                    
                    cmd.Parameters.Add("@nombre", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@idPerfil", SqlDbType.NVarChar, 50);

                    cmd.Parameters["@loginUsuario"].Value = login;
                    cmd.Parameters["@loginPassword"].Value = pass;

                    cmd.Parameters["@nif"].Value = nif;
                    cmd.Parameters["@telefono"].Value = tlf;

                    cmd.Parameters["@email"].Value = mail;
                    cmd.Parameters["@apellidos"].Value = apellidos;

                    cmd.Parameters["@nombre"].Value = nombre;
                    cmd.Parameters["@idPerfil"].Value = perId;
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    respuesta = cmd.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return respuesta;
        }

        public string GetComprobarPassActual(int idUsuario, string passwordActual)
        {
            string respuesta = "";

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
                    cmd.CommandText = "getComprobarPassActual";

                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int);
                    cmd.Parameters.Add("@passwordActual", SqlDbType.VarChar);

                    cmd.Parameters["@idUsuario"].Value = idUsuario;
                    cmd.Parameters["@passwordActual"].Value = passwordActual;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    respuesta = cmd.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                HistorialExcepciones his = new HistorialExcepciones();
                his.InsertExcepcion(ex.Message, ex.StackTrace, 0);
            }

            return respuesta;
        }
    }
}
