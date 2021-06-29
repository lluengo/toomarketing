using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Mensaje : MAPPER<Mensaje>
    {
        public DAL_Mensaje()
        {
            acceso = new ACCESO();
            accesopropio = true;
        }
        internal DAL_Mensaje(ACCESO ac)
        {
            acceso = ac;
            accesopropio = false;
        }
        public override int Borrar(Mensaje objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            Abrir();
            int resultado = acceso.Escribir("Mensaje_BORRAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Editar(Mensaje objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));
            parametros.Add(acceso.CrearParametro("@contenido", objeto.contenido));


            Abrir();
            int resultado = acceso.Escribir("Mensaje_EDITAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Insertar(Mensaje objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));
            parametros.Add(acceso.CrearParametro("@contenido", objeto.contenido));
            Abrir();
            int resultado = acceso.Escribir("Mensaje_INSERTAR", parametros);
            Cerrar();
            return resultado;
        }

        public override List<Mensaje> Listar()
        {
            Abrir();
            DataTable tabla = acceso.Leer("Mensaje_LISTAR");
            Cerrar();
            List<Mensaje> Mensajees = new List<Mensaje>();
            foreach (DataRow registro in tabla.Rows)
            {
                Mensaje c = new Mensaje();
                c.id = int.Parse(registro["ID_Mensaje"].ToString());
                c.nombre = registro["Nombre"].ToString();
                c.contenido = registro["Contenido"].ToString();

                Mensajees.Add(c);
            }
            return Mensajees;
        }

        public Mensaje Listar(int id)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", id));
            DataTable tabla = acceso.Leer("Mensaje_LISTAR_ID", parametros);
            Cerrar();
            List<Mensaje> Mensajees = new List<Mensaje>();
            foreach (DataRow registro in tabla.Rows)
            {
                Mensaje c = new Mensaje();
                c.id = int.Parse(registro["ID_Mensaje"].ToString());
                c.nombre = registro["Nombre"].ToString();
                c.contenido = registro["Contenido"].ToString();

                Mensajees.Add(c);
            }
            return Mensajees[0];
        }
    }
}
