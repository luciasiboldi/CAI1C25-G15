using System;
using Persistencia;

namespace Negocio
{
    public class PasswordService
    {
        private readonly UsuarioPersistencia _up = new UsuarioPersistencia();
        private readonly LoginNegocio _loginNeg = new LoginNegocio();
        private const int ExpiracionDias = 30;

    
        public bool NecesitaCambiar(string usuario)
        {
            var cred = _up.ObtenerCredencial(usuario);
            if (cred == null)
                throw new Exception("Usuario no existe.");

            return cred.FechaUltimoLogin == DateTime.MinValue
                || (DateTime.Today - cred.FechaUltimoLogin).TotalDays >= ExpiracionDias;
        }

        /// <summary>
        /// Valida la contraseña actual y escribe la nueva en el CSV.
        /// </summary>
        public void CambiarPassword(string usuario, string actualPwd, string nuevaPwd)
        {
            // 1) valida credenciales y bloqueo
            _loginNeg.Autenticar(usuario, actualPwd);
            // 2) actualiza CSV (con nuevaPwd y fecha de hoy)
            _up.ActualizarCredencial(usuario, nuevaPwd);
        }
    }
}
