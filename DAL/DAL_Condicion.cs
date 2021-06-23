using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DAL_Condicion : MAPPER<Condicion>
    {
        public DAL_Condicion()
        {
            acceso = new ACCESO();
            accesopropio = true;
        }
        internal DAL_Condicion(ACCESO ac)
        {
            acceso = ac;
            accesopropio = false;
        }
        public override int Borrar(Condicion objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            Abrir();
            int resultado = acceso.Escribir("Condicion_BORRAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Editar(Condicion objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            parametros.Add(acceso.CrearParametro("@atributo", objeto.atributo));
            parametros.Add(acceso.CrearParametro("@operacion", objeto.operacion.ToString()));
            parametros.Add(acceso.CrearParametro("@valor", objeto.valor));
            parametros.Add(acceso.CrearParametro("@entidad", objeto.entidad.ToString()));
            parametros.Add(acceso.CrearParametro("@tipoValor", objeto.tipoValor.ToString()));
            Abrir();
            int resultado = acceso.Escribir("Condicion_EDITAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Insertar(Condicion objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@atributo", objeto.atributo));
            parametros.Add(acceso.CrearParametro("@operacion", objeto.operacion.ToString()));
            parametros.Add(acceso.CrearParametro("@valor", objeto.valor));
            parametros.Add(acceso.CrearParametro("@entidad", objeto.entidad.ToString()));
            parametros.Add(acceso.CrearParametro("@tipoValor", objeto.tipoValor.ToString()));
            Abrir();
            int resultado = acceso.Escribir("Condicion_INSERTAR", parametros);
            Cerrar();
            return resultado;
        }

        public override List<Condicion> Listar()
        {
            Abrir();
            MP_Usuario mp_usuario = new MP_Usuario(acceso);

            List<Usuario> usuarios = mp_usuario.Listar();

            mp_usuario = null;

            DataTable tabla = acceso.Leer("Condicion_LISTAR");
            Cerrar();
            List<Condicion> condiciones = new List<Condicion>();
            foreach (DataRow registro in tabla.Rows)
            {
                Condicion c = new Condicion();
                c.id = int.Parse(registro["ID_Condicion"].ToString());
                c.atributo = registro["Atributo"].ToString();
                c.operacion = (Operacion)Enum.Parse(typeof(Operacion), registro["Operacion"].ToString());
                c.valor = registro["Valor"].ToString();
                c.entidad = (Entidad)Enum.Parse(typeof(Entidad), registro["Entidad"].ToString());
                c.tipoValor = (TipoValor)Enum.Parse(typeof(TipoValor), registro["TipoValor"].ToString());

                condiciones.Add(c);
            }
            return condiciones;
        }

        public Condicion Listar(int id)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", id));
            DataTable tabla = acceso.Leer("Condicion_LISTAR_ID", parametros);
            Cerrar();
            List<Condicion> condiciones = new List<Condicion>();
            foreach (DataRow registro in tabla.Rows)
            {
                Condicion c = new Condicion();
                c.id = int.Parse(registro["ID_Condicion"].ToString());
                c.atributo = registro["Atributo"].ToString();
                c.operacion = (Operacion)Enum.Parse(typeof(Operacion), registro["Operacion"].ToString());
                c.valor = registro["Valor"].ToString();
                c.entidad = (Entidad)Enum.Parse(typeof(Entidad), registro["Entidad"].ToString());
                c.tipoValor = (TipoValor)Enum.Parse(typeof(TipoValor), registro["TipoValor"].ToString());

                condiciones.Add(c);
            }
            return condiciones[0];
        }
    }
}
