using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Accion : MAPPER<Accion>
    {
        public DAL_Accion()
        {
            acceso = new ACCESO();
            accesopropio = true;
        }
        internal DAL_Accion(ACCESO ac)
        {
            acceso = ac;
            accesopropio = false;
        }
        public override int Borrar(Accion objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            Abrir();
            int resultado = acceso.Escribir("Accion_BORRAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Editar(Accion objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));
            parametros.Add(acceso.CrearParametro("@numero", objeto.numero));
            parametros.Add(acceso.CrearParametro("@tipoAccion", objeto.tipoAccion.ToString()));
            Abrir();
            int resultado = acceso.Escribir("Accion_EDITAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Insertar(Accion objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));
            parametros.Add(acceso.CrearParametro("@numero", objeto.numero));
            parametros.Add(acceso.CrearParametro("@tipoAccion", objeto.tipoAccion.ToString()));
            Abrir();
            int resultado = acceso.Escribir("Accion_INSERTAR", parametros);
            Cerrar();
            return resultado;
        }

        public override List<Accion> Listar()
        {
            Abrir();
            DataTable tabla = acceso.Leer("Accion_LISTAR");
            Cerrar();
            List<Accion> Acciones = new List<Accion>();
            foreach (DataRow registro in tabla.Rows)
            {
                Accion c = new Accion();
                c.id = int.Parse(registro["ID_Accion"].ToString());
                c.nombre = registro["Nombre"].ToString();
                c.numero = int.Parse(registro["Numero"].ToString());
                c.tipoAccion = (TipoAccion)Enum.Parse(typeof(TipoAccion), registro["TipoAccion"].ToString());

                Acciones.Add(c);
            }
            return Acciones;
        }

        public Accion Listar(int id)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", id));
            DataTable tabla = acceso.Leer("Accion_LISTAR_ID", parametros);
            Cerrar();
            List<Accion> Acciones = new List<Accion>();
            foreach (DataRow registro in tabla.Rows)
            {
                Accion c = new Accion();
                c.id = int.Parse(registro["ID_Accion"].ToString());
                c.nombre = registro["Nombre"].ToString();
                c.numero = int.Parse(registro["Numero"].ToString());
                c.tipoAccion = (TipoAccion)Enum.Parse(typeof(TipoAccion), registro["TipoAccion"].ToString());

                Acciones.Add(c);
            }
            return Acciones[0];
        }
    }
}
