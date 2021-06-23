using BE;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL
{
    public class DAL_Regla : MAPPER<Regla>
    {
        public DAL_Regla()
        {
            acceso = new ACCESO();
            accesopropio = true;
        }
        internal DAL_Regla(ACCESO ac)
        {
            acceso = ac;
            accesopropio = false;
        }
        public override int Borrar(Regla objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            Abrir();
            int resultado = acceso.Escribir("Regla_BORRAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Editar(Regla objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));
            parametros.Add(acceso.CrearParametro("@ID_Accion", objeto.accion.id));
            parametros.Add(acceso.CrearParametro("@ID_Condicion", objeto.condicion.id));
            Abrir();
            int resultado = acceso.Escribir("Regla_EDITAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Insertar(Regla objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));
            parametros.Add(acceso.CrearParametro("@ID_Accion", objeto.accion.id));
            parametros.Add(acceso.CrearParametro("@ID_Condicion", objeto.condicion.id));

            Abrir();
            int resultado = acceso.Escribir("Regla_INSERTAR", parametros);
            Cerrar();
            return resultado;
        }

        public override List<Regla> Listar()
        {
            Abrir();
            DAL_Condicion dal_condicion = new DAL_Condicion(acceso);
            List<Condicion> condiciones = dal_condicion.Listar();
            DAL_Accion dal_accion = new DAL_Accion(acceso);
            List<Accion> acciones = dal_accion.Listar();
            DataTable tabla = acceso.Leer("Regla_LISTAR");
            Cerrar();
            List<Regla> reglas = new List<Regla>();
            foreach (DataRow registro in tabla.Rows)
            {
                Regla r = new Regla();
                r.id = int.Parse(registro["ID_Regla"].ToString()); 
                r.condicion = (from Condicion c in condiciones
                               where c.id == int.Parse(registro["ID_Condicion"].ToString())
                               select c
                               ).First();
                r.accion = (from Accion a in acciones
                               where a.id == int.Parse(registro["ID_Accion"].ToString())
                               select a
                        ).First();

                reglas.Add(r);
            }
            return reglas;
        }

        public Condicion Listar(int id)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", id));
            DataTable tabla = acceso.Leer("Regla_LISTAR_ID", parametros);
            DAL_Condicion dal_condicion = new DAL_Condicion(acceso);
            List<Condicion> condiciones = dal_condicion.Listar();
            DAL_Accion dal_accion = new DAL_Accion(acceso);
            List<Accion> acciones = dal_accion.Listar();
            Cerrar();
            List<Regla> reglas = new List<Regla>();
            foreach (DataRow registro in tabla.Rows)
            {
                Regla r = new Regla();
                r.id = int.Parse(registro["ID_Regla"].ToString());
                r.condicion = (from Condicion c in condiciones
                               where c.id == int.Parse(registro["ID_Condicion"].ToString())
                               select c
                               ).First();
                r.accion = (from Accion a in acciones
                            where a.id == int.Parse(registro["ID_Accion"].ToString())
                            select a
                       ).First();

                reglas.Add(r);
            }
            return condiciones[0];
        }
    }
}
