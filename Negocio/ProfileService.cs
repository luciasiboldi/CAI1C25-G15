using Datos;
using Persistencia;
using System;
using System.Linq;

namespace Negocio
{
    public class ProfileService
    {
        private readonly UsuarioPersistencia _up = new UsuarioPersistencia();
        private readonly PerfilPersistencia _pp = new PerfilPersistencia();
        private readonly LoginNegocio _login = new LoginNegocio();

        public enum PerfilUsuario { Supervisor, Administrador, Operador }

        /// <summary>
        /// Devuelve el idPerfil del usuario (o null si no existe).
        /// </summary>
        public string GetIdPerfil(string usuario)
        {
            var cred = _up.ObtenerCredencial(usuario);
            if (cred == null)
                throw new Exception("Usuario no registrado.");
            return _pp.ObtenerIdPerfil(cred.Legajo);
        }

        /// <summary>
        /// Devuelve nombres de roles asignados al usuario.
        /// </summary>
        public string[] GetRoles(string usuario)
        {
            var idPerfil = GetIdPerfil(usuario);
            if (string.IsNullOrEmpty(idPerfil))
                return Array.Empty<string>();

            var roles = _pp.ObtenerRolesPorPerfil(idPerfil);
            return roles.Select(r => r.Nombre).ToArray();
        }
    }
}
