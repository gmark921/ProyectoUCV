using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq; // Necesario para la validación de teléfono
using System.Text.RegularExpressions; // Necesario para la validación de correo

namespace CapaNegocio
{
    public class CN_Proveedor
    {
        private CD_Proveedor objcd_proveedor = new CD_Proveedor();

        public List<Proveedor> Listar()
        {
            return objcd_proveedor.Listar();
        }

        public string Registrar(Proveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.RazonSocial))
            {
                Mensaje += "La razón social del proveedor no puede ser vacía.\n";
            }
            if (string.IsNullOrWhiteSpace(obj.NombreComercial))
            {
                Mensaje += "El nombre comercial del proveedor no puede ser vacío.\n";
            }

            // --- INICIO DE NUEVAS VALIDACIONES ---
            if (!EsCorreoValido(obj.Correo))
            {
                Mensaje += "El formato del correo no es válido (ej: correo@dominio.com).\n";
            }
            if (!string.IsNullOrEmpty(obj.Telefono) && (!obj.Telefono.All(char.IsDigit) || obj.Telefono.Length < 6))
            {
                Mensaje += "El teléfono solo debe contener números y tener al menos 6 dígitos.\n";
            }
            if (obj.Estado == false)
            {
                Mensaje += "No se puede registrar un proveedor con estado 'No Activo'.\n";
            }
            // --- FIN DE NUEVAS VALIDACIONES ---

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objcd_proveedor.Registrar(obj, out Mensaje);
            }
            else
            {
                return "0";
            }
        }

        public bool Editar(Proveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.RazonSocial))
            {
                Mensaje += "La razón social del proveedor no puede ser vacía.\n";
            }
            if (string.IsNullOrWhiteSpace(obj.NombreComercial))
            {
                Mensaje += "El nombre comercial del proveedor no puede ser vacío.\n";
            }

            // --- INICIO DE NUEVAS VALIDACIONES ---
            if (!EsCorreoValido(obj.Correo))
            {
                Mensaje += "El formato del correo no es válido (ej: correo@dominio.com).\n";
            }
            if (!string.IsNullOrEmpty(obj.Telefono) && (!obj.Telefono.All(char.IsDigit) || obj.Telefono.Length < 6))
            {
                Mensaje += "El teléfono solo debe contener números y tener al menos 6 dígitos.\n";
            }
            // --- FIN DE NUEVAS VALIDACIONES ---

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objcd_proveedor.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(string id, out string Mensaje)
        {
            return objcd_proveedor.Eliminar(id, out Mensaje);
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