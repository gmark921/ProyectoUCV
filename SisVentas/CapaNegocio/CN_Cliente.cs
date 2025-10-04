using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq; // Necesario para la validación de teléfono
using System.Text.RegularExpressions; // Necesario para la validación de correo

namespace CapaNegocio
{
    public class CN_Cliente
    {
        private CD_Cliente objcd_cliente = new CD_Cliente();

        public List<Cliente> Listar()
        {
            return objcd_cliente.Listar();
        }

        public string Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            // --- VALIDACIONES NUEVAS ---
            if (string.IsNullOrEmpty(obj.Documento) || string.IsNullOrWhiteSpace(obj.Documento))
            {
                Mensaje += "El documento del cliente no puede ser vacío.\n";
            }
            if (string.IsNullOrEmpty(obj.NombreCompleto) || string.IsNullOrWhiteSpace(obj.NombreCompleto))
            {
                Mensaje += "El nombre del cliente no puede ser vacío.\n";
            }
            if (!EsCorreoValido(obj.Correo))
            {
                Mensaje += "El formato del correo no es válido (ej: correo@dominio.com).\n";
            }
            if (!string.IsNullOrEmpty(obj.Telefono) && !obj.Telefono.All(char.IsDigit))
            {
                Mensaje += "El teléfono solo debe contener números.\n";
            }
            if (obj.Estado == false)
            {
                Mensaje += "No se puede registrar un cliente con estado 'No Activo'.\n";
            }
            // --- FIN DE VALIDACIONES ---

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objcd_cliente.Registrar(obj, out Mensaje);
            }
            else
            {
                return "0";
            }
        }

        public bool Editar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            // --- VALIDACIONES NUEVAS ---
            if (string.IsNullOrEmpty(obj.Documento) || string.IsNullOrWhiteSpace(obj.Documento))
            {
                Mensaje += "El documento del cliente no puede ser vacío.\n";
            }
            if (string.IsNullOrEmpty(obj.NombreCompleto) || string.IsNullOrWhiteSpace(obj.NombreCompleto))
            {
                Mensaje += "El nombre del cliente no puede ser vacío.\n";
            }
            if (!EsCorreoValido(obj.Correo))
            {
                Mensaje += "El formato del correo no es válido (ej: correo@dominio.com).\n";
            }
            if (!string.IsNullOrEmpty(obj.Telefono) && !obj.Telefono.All(char.IsDigit))
            {
                Mensaje += "El teléfono solo debe contener números.\n";
            }
            // --- FIN DE VALIDACIONES ---

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objcd_cliente.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(string id, out string Mensaje)
        {
            return objcd_cliente.Eliminar(id, out Mensaje);
        }

        // --- FUNCIÓN AUXILIAR PARA VALIDAR CORREO ---
        private bool EsCorreoValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Expresión regular para validar un formato de email estándar.
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}