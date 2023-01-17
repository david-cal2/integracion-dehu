using System.Data;

namespace PSOENotificaciones.Interfaces
{
    public interface IPsoeAdministracion
    {
        DataTable LoginUsuario(string loginUsuario, string loginPassword, string hostName, string clienteIp);

        bool UpdateUsuarioPassword(int idUsuario, string loginPassword);

        string InsertUsuario(string nombre, string apellidos, string nif, string mail, string tlf, string login, int perId, string pass);

        string GetComprobarPassActual(int idUsuario, string passwordActual);
    }
}
