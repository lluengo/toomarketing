using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Template : MAPPER<Template>
    {
        public DAL_Template()
        {
            acceso = new ACCESO();
            accesopropio = true;
        }
        internal DAL_Template(ACCESO ac)
        {
            acceso = ac;
            accesopropio = false;
        }
        public override int Borrar(Template objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            Abrir();
            int resultado = acceso.Escribir("Template_BORRAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Editar(Template objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            parametros.Add(acceso.CrearParametro("@string1", objeto.string1));
            parametros.Add(acceso.CrearParametro("@string2", objeto.string2));
            parametros.Add(acceso.CrearParametro("@string3", objeto.string3));
            parametros.Add(acceso.CrearParametro("@string4", objeto.string4));
            parametros.Add(acceso.CrearParametro("@nroTemplate", objeto.nroTemplate));
            parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));

            Abrir();
            int resultado = acceso.Escribir("Template_EDITAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Insertar(Template objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@string1", objeto.string1));
            parametros.Add(acceso.CrearParametro("@string2", objeto.string2));
            parametros.Add(acceso.CrearParametro("@string3", objeto.string3));
            parametros.Add(acceso.CrearParametro("@string4", objeto.string4));
            parametros.Add(acceso.CrearParametro("@nroTemplate", objeto.nroTemplate));
            parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));
            Abrir();
            int resultado = acceso.Escribir("Template_INSERTAR", parametros);
            Cerrar();
            return resultado;
        }

        public override List<Template> Listar()
        {
            Abrir();
            DataTable tabla = acceso.Leer("Template_LISTAR");
            Cerrar();
            List<Template> Templatees = new List<Template>();
            foreach (DataRow registro in tabla.Rows)
            {
                Template c = new Template();
                c.id = int.Parse(registro["ID_Template"].ToString());
                c.nombre = registro["Nombre"].ToString();
                c.string1 = registro["String1"].ToString();
                c.string2 = registro["String2"].ToString();
                c.string3 = registro["String3"].ToString();
                c.string4 = registro["String4"].ToString();
                c.nroTemplate = int.Parse(registro["NroTemplate"].ToString());

                Templatees.Add(c);
            }
            return Templatees;
        }

        public Template Listar(int id)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", id));
            DataTable tabla = acceso.Leer("Template_LISTAR_ID", parametros);
            Cerrar();
            List<Template> Templatees = new List<Template>();
            foreach (DataRow registro in tabla.Rows)
            {
                Template c = new Template();
                c.id = int.Parse(registro["ID_Template"].ToString());
                c.nombre = registro["Nombre"].ToString();
                c.string1 = registro["String1"].ToString();
                c.string2 = registro["String2"].ToString();
                c.string3 = registro["String3"].ToString();
                c.string4 = registro["String4"].ToString();
                c.nroTemplate = int.Parse(registro["NroTemplate"].ToString());

                Templatees.Add(c);
            }
            return Templatees[0];
        }
    }
}
