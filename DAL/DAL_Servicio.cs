using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Servicio : MAPPER<Servicio>
    {
        public DAL_Servicio()
        {
            acceso = new ACCESO();
            accesopropio = true;
        }
        internal DAL_Servicio(ACCESO ac)
        {
            acceso = ac;
            accesopropio = false;
        }
        public override int Borrar(Servicio objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            Abrir();
            int resultado = acceso.Escribir("Servicio_BORRAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Editar(Servicio objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));
            parametros.Add(acceso.CrearParametro("@descripcion", objeto.descripcion));
            parametros.Add(acceso.CrearParametro("@cateogoria", objeto.categoria));
            parametros.Add(acceso.CrearParametro("@precio", objeto.precio));
            Abrir();
            int resultado = acceso.Escribir("Servicio_EDITAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Insertar(Servicio objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));
            parametros.Add(acceso.CrearParametro("@descripcion", objeto.descripcion));
            parametros.Add(acceso.CrearParametro("@cateogoria", objeto.categoria));
            parametros.Add(acceso.CrearParametro("@precio", objeto.precio));
            Abrir();
            int resultado = acceso.Escribir("Servicio_INSERTAR", parametros);
            Cerrar();
            return resultado;
        }

        public override List<Servicio> Listar()
        {
            Abrir();
            MP_Usuario mp_usuario = new MP_Usuario(acceso);

            List<Usuario> usuarios = mp_usuario.Listar();

            mp_usuario = null;

            DataTable tabla = acceso.Leer("Servicio_LISTAR");
            Cerrar();
            List<Servicio> servicios = new List<Servicio>();
            foreach (DataRow registro in tabla.Rows)
            {
                Servicio c = new Servicio();
                c.id = int.Parse(registro["ID_Servicio"].ToString());
                c.nombre = registro["Nombre"].ToString();
                c.descripcion = registro["Descripcion"].ToString();
                c.categoria = registro["Categoria"].ToString();
                c.precio = double.Parse(registro["Precio"].ToString());

                servicios.Add(c);
            }
            return servicios;
        }

        public Servicio Listar(int id)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", id));
            DataTable tabla = acceso.Leer("Servicio_LISTAR_ID", parametros);
            Cerrar();
            List<Servicio> condiciones = new List<Servicio>();
            foreach (DataRow registro in tabla.Rows)
            {
                Servicio c = new Servicio();
                c.id = int.Parse(registro["ID_Servicio"].ToString());
                c.nombre = registro["Nombre"].ToString();
                c.descripcion = registro["Descripcion"].ToString();
                c.categoria = registro["Categoria"].ToString();
                c.precio = double.Parse(registro["Precio"].ToString());

                condiciones.Add(c);
            }
            return condiciones[0];
        }
    }
}
