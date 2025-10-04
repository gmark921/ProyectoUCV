// Archivo: CapaNegocio/CN_Usuario.cs
using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Usuario
    {
        private CD_Usuario objcd_usuario = new CD_Usuario();

        public List<Usuario> Listar()
        {
            return objcd_usuario.Listar();
        }
        private void ValidarDatosBase(Usuario obj, ref string Mensaje)
        {
            if (string.IsNullOrWhiteSpace(obj.Documento))
            {
                Mensaje += "Es necesario el documento del usuario.\n";
            }
            if (string.IsNullOrWhiteSpace(obj.NombreCompleto))
            {
                Mensaje += "Es necesario el nombre completo del usuario.\n";
            }
            if (string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje += "Es necesario el correo del usuario.\n";
            }
            else if (!EsCorreoValido(obj.Correo))
            {
                Mensaje += "El formato del correo no es v�lido.\n";
            }
        }
        public string Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            // --- VALIDACIONES DE DATOS B�SICOS ---
            ValidarDatosBase(obj, ref Mensaje);

            // --- VALIDACIONES DE CONTRASE�A (A�ADIDAS) ---
            if (string.IsNullOrWhiteSpace(obj.Clave))
            {
                Mensaje += "Es necesaria la clave del usuario.\n";
            }
            else if (obj.Clave.Length < 8)
            {
                Mensaje += "La clave debe tener al menos 8 caracteres.\n";
            }
            if (obj.Clave != obj.ConfirmarClave)
            {
                Mensaje += "Las contrase�as no coinciden.\n";
            }
            // La nueva validaci�n ahora est� junto a las dem�s.
            if (!obj.Estado)
            {
                Mensaje += "No es posible registrar un usuario como 'No Activo'.\n";
            }

            // --- COMPROBACI�N FINAL ---
            // Solo despu�s de todas las validaciones, decidimos si registrar o no.
            if (!string.IsNullOrEmpty(Mensaje))
            {
                // Si hay alg�n mensaje de error, retornamos "0".
                return "0";
            }
            else
            {
                // Si no hay errores, llamamos a la capa de datos para registrar.
                return objcd_usuario.Registrar(obj, out Mensaje);
            }
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            // --- VALIDACIONES DE DATOS B�SICOS ---
            ValidarDatosBase(obj, ref Mensaje);

            // --- VALIDACIONES DE CONTRASE�A (A�ADIDAS) ---
            if (string.IsNullOrWhiteSpace(obj.Clave))
            {
                Mensaje += "Es necesaria la clave del usuario.\n";
            }
            else if (obj.Clave.Length < 8)
            {
                Mensaje += "La clave debe tener al menos 8 caracteres.\n";
            }
            if (obj.Clave != obj.ConfirmarClave)
            {
                Mensaje += "Las contrase�as no coinciden.\n";
            }

            // --- COMPROBACI�N FINAL ---
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objcd_usuario.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(string id, out string Mensaje)
        {
            return objcd_usuario.Eliminar(id, out Mensaje);
        }

        // Funci�n privada para validar el formato del correo
        private bool EsCorreoValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public Usuario Login(string documento)
        {
            return objcd_usuario.Login(documento);
        }
    }
}