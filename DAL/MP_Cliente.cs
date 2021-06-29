using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MP_Cliente : MAPPER<Cliente>
    {
        public MP_Cliente()
        {
            acceso = new ACCESO();
            accesopropio = true;
        }
        internal MP_Cliente(ACCESO ac)
        {
            acceso = ac;
            accesopropio = false;
        }
        public override int Borrar(Cliente objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.Id));
            Abrir();
            int resultado = acceso.Escribir("Cliente_BORRAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Editar(Cliente objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.Id));
            parametros.Add(acceso.CrearParametro("@dni", objeto.Dni));
            parametros.Add(acceso.CrearParametro("@nombre", objeto.Nombre));
            parametros.Add(acceso.CrearParametro("@apellido", objeto.Apellido));
            parametros.Add(acceso.CrearParametro("@direccion", objeto.Direccion));
            parametros.Add(acceso.CrearParametro("@fechaNacimiento", objeto.FechaNacimiento));
            Abrir();
            int resultado = acceso.Escribir("Cliente_EDITAR", parametros);
            Cerrar();
            return resultado;
        }

        public Cliente ListarPorUsuario(int iD)
        {
            Abrir();

            
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@idUsuario", iD));

            DataTable tabla = acceso.Leer("Cliente_LISTAR_USUARIO_ID", parametros);
            Cerrar();

            if (tabla.Rows.Count < 1)
                return null;

            DataRow registro = tabla.Rows[0];
                        
            Cliente c = new Cliente();
            c.Id = int.Parse(registro["Id"].ToString());
            c.Nombre = registro["Nombre"].ToString();
            c.Apellido = registro["Apellido"].ToString();
            c.Dni = long.Parse(registro["Dni"].ToString());
            c.Direccion = registro["Direccion"].ToString();
            c.FechaNacimiento = DateTime.Parse(registro["Fecha_Nacimiento"].ToString());
            
            return c;
        }

        public override int Insertar(Cliente objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@dni", objeto.Dni));
            parametros.Add(acceso.CrearParametro("@nombre", objeto.Nombre));
            parametros.Add(acceso.CrearParametro("@apellido", objeto.Apellido));
            parametros.Add(acceso.CrearParametro("@direccion", objeto.Direccion));
            parametros.Add(acceso.CrearParametro("@fechaNacimiento", objeto.FechaNacimiento));
            Abrir();
            int resultado = acceso.Escribir("Cliente_INSERTAR", parametros);
            Cerrar();
            return resultado;
        }

        public override List<Cliente> Listar()
        {
            Abrir();
            MP_Usuario mp_usuario = new MP_Usuario(acceso);

            List<Usuario> usuarios = mp_usuario.Listar();

            mp_usuario = null;

            DataTable tabla = acceso.Leer("Cliente_LISTAR");
            Cerrar();
            List<Cliente> Clientes = new List<Cliente>();
            foreach (DataRow registro in tabla.Rows)
            {
                Cliente c = new Cliente();
                c.Id = int.Parse(registro["Id"].ToString());
                c.Nombre = registro["Nombre"].ToString();
                c.Apellido = registro["Apellido"].ToString();
                c.Dni = long.Parse(registro["Dni"].ToString());
                c.Direccion = registro["Direccion"].ToString();
                c.FechaNacimiento = DateTime.Parse(registro["Fecha_Nacimiento"].ToString());

                Clientes.Add(c);
            }
            return Clientes;
        }

        public Cliente Listar(int id)
        {
            Abrir();

            MP_Usuario mp_usuario = new MP_Usuario(acceso);

            List<Usuario> usuarios = mp_usuario.Listar();

            mp_usuario = null;

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", id));

            DataTable tabla = acceso.Leer("Cliente_LISTAR_ID", parametros);
            Cerrar();

            DataRow registro = tabla.Rows[0];

            Cliente c = new Cliente();
            c.Id = int.Parse(registro["Id"].ToString());
            c.Nombre = registro["Nombre"].ToString();
            c.Apellido = registro["Apellido"].ToString();
            c.Dni = long.Parse(registro["Dni"].ToString());
            c.Direccion = registro["Direccion"].ToString();
            c.FechaNacimiento = DateTime.Parse(registro["Fecha_Nacimiento"].ToString());

             return c;
        }
    }
}
