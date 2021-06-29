using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAL_Segmento : MAPPER<Segmento>
    {
        public DAL_Segmento()
        {
            acceso = new ACCESO();
            accesopropio = true;
        }
        internal DAL_Segmento(ACCESO ac)
        {
            acceso = ac;
            accesopropio = false;
        }
        public override int Borrar(Segmento objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            Abrir();
            int resultadoMM = acceso.Escribir("Segmento_BORRAR_REGLAS", parametros);
            int resultado = acceso.Escribir("Regla_BORRAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Editar(Segmento objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));
            parametros.Add(acceso.CrearParametro("@descripcion", objeto.descripcion));            
            Abrir();
            int resultado = acceso.Escribir("Segmento_EDITAR", parametros);

            List<SqlParameter> parametrosABorrar = new List<SqlParameter>();
            parametrosABorrar.Add(acceso.CrearParametro("@id", objeto.id));
            acceso.Escribir("Segmento_BORRAR_REGLAS", parametrosABorrar);

            if (objeto.reglas.Any()) {
                foreach (Regla regla in objeto.reglas)
                {
                    List<SqlParameter> parametrosReglas = new List<SqlParameter>();
                    parametrosReglas.Add(acceso.CrearParametro("@ID_Segmento", objeto.id));
                    parametrosReglas.Add(acceso.CrearParametro("@ID_Regla", regla.id));
                    acceso.Escribir("Segmento_INSERTAR_Regla", parametrosReglas);
                }
            }
            Cerrar();
            return resultado;
        }

        public override int Insertar(Segmento objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@descripcion", objeto.descripcion));
            parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));
            Abrir();
            int resultado = acceso.Escribir("Segmento_INSERTAR", parametros);
            DataTable tabla = acceso.Leer("Segmento_LISTAR_ULTIMO_ID");
            int ultimoId = 0;
            foreach (DataRow registro in tabla.Rows)
            {
                 ultimoId = int.Parse(registro["ID_Segmento"].ToString());
            }

            foreach (Regla regla in objeto.reglas)
            {
                List<SqlParameter> parametrosReglas = new List<SqlParameter>();
                parametrosReglas.Add(acceso.CrearParametro("@ID_Segmento", ultimoId));
                parametrosReglas.Add(acceso.CrearParametro("@ID_Regla", regla.id));
                acceso.Escribir("Segmento_INSERTAR_Regla", parametrosReglas);
            }
            Cerrar();
            return resultado;
        }

        public override List<Segmento> Listar()
        {
            Abrir();
            DataTable tabla = acceso.Leer("Segmento_LISTAR");

            DAL_Regla dalRegla = new DAL_Regla(acceso);
            
            List<Segmento> segmentos = new List<Segmento>();
            foreach (DataRow registro in tabla.Rows)
            {
                Segmento s = new Segmento();
                s.id = int.Parse(registro["ID_Segmento"].ToString());
                s.nombre = registro["Nombre"].ToString();
                s.descripcion = registro["Descripcion"].ToString();

                s.reglas = dalRegla.ListarReglasSegmentacion(s.id);

                segmentos.Add(s);
            }
                        Cerrar();

            return segmentos;
        }

        public Segmento Listar(int id)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", id));
            DataTable tabla = acceso.Leer("Segmento_LISTAR_ID",parametros);

            DAL_Regla dalRegla = new DAL_Regla(acceso);

            List<Segmento> segmentos = new List<Segmento>();
            foreach (DataRow registro in tabla.Rows)
            {
                Segmento s = new Segmento();
                s.id = int.Parse(registro["ID_Segmento"].ToString());
                s.nombre = registro["Nombre"].ToString();
                s.descripcion = registro["Descripcion"].ToString();

                s.reglas = dalRegla.ListarReglasSegmentacion(s.id);

                segmentos.Add(s);
            }

            Cerrar();

            return segmentos[0];
        }

        public List<Segmento> ListarSegmentosPublicidad(int idPublicidad)
        {
            Abrir();
            DAL_Regla dalRegla = new DAL_Regla(acceso);
            DAL_Accion dal_accion = new DAL_Accion(acceso);
            List<Accion> acciones = dal_accion.Listar();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", idPublicidad));
            DataTable tabla = acceso.Leer("Publicidad_LISTAR_SEGMENTOS", parametros);
            Cerrar();
            List<Segmento> segmentos = new List<Segmento>();
            foreach (DataRow registro in tabla.Rows)
            {
                Segmento s = new Segmento();
                s.id = int.Parse(registro["ID_Segmento"].ToString());
                s.nombre = registro["Nombre"].ToString();
                s.descripcion = registro["Descripcion"].ToString();

                s.reglas = dalRegla.ListarReglasSegmentacion(s.id);

                segmentos.Add(s);
            }
            return segmentos;
        }
    }
}
