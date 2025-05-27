using System;
using Persistencia;

namespace Negocio
{
    public class ProfileService
    {
        private readonly PerfilPersistencia _pp = new PerfilPersistencia();

        /// <summary>
        /// Devuelve el array de descripciones de roles para el legajo dado.
        /// </summary>
        public string[] GetRoles(string legajo)
        {
            // 1) Obtengo el id de perfil ("1","2" o "3")
            var idPerfil = _pp.ObtenerIdPerfil(legajo);
            if (string.IsNullOrEmpty(idPerfil))
                return Array.Empty<string>();

            // 2) Obtengo los roles para ese perfil
            return _pp.ObtenerRolesPorPerfil(idPerfil);
        }
    }
}