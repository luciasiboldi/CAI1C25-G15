using Datos;               
using Persistencia;      
using System;

namespace Negocio
{
    public class LoginNegocio
    {
        private readonly UsuarioPersistencia _up = new UsuarioPersistencia();

        /// Intenta autenticar; tira excepción si falla o bloquea.
        public Credencial Autenticar(string usuario, string password)
        {
            //Sacamos la credencial CSV
            var cred = _up.ObtenerCredencial(usuario);
            if (cred == null)
                throw new Exception("Usuario no registrado.");

            //Verifico bloqueo
            if (_up.EsUsuarioBloqueado(cred.Legajo))
                throw new Exception("Usuario bloqueado.");

            //Valido contraseña
            if (!cred.Contrasena.Equals(password))
            {
                _up.RegistrarIntento(cred.Legajo);
                var intentos = _up.ObtenerIntentos(cred.Legajo);
                if (intentos >= UsuarioPersistencia.MaxIntentos)
                {
                    throw new Exception("3 intentos fallidos. Usuario BLOQUEADO.");
                }
                throw new Exception($"Contraseña incorrecta. Intento {intentos} de {UsuarioPersistencia.MaxIntentos}.");
            }

            return cred;
        }
    }
}