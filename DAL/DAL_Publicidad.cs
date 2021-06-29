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
    public class DAL_Publicidad : MAPPER<Publicidad>
    {
        public DAL_Publicidad()
        {
            acceso = new ACCESO();
            accesopropio = true;
        }
        internal DAL_Publicidad(ACCESO ac)
        {
            acceso = ac;
            accesopropio = false;
        }
        public override int Borrar(Publicidad objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            Abrir();
            int resultadoMM = acceso.Escribir("Publicidad_BORRAR_SEGMENTOS", parametros);
            int resultado = acceso.Escribir("Publicidad_BORRAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Editar(Publicidad objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.id));
            parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));
            parametros.Add(acceso.CrearParametro("@descripcion", objeto.descripcion));
            parametros.Add(acceso.CrearParametro("@fechaInicio", objeto.fechaInicio));
            parametros.Add(acceso.CrearParametro("@fechaFin", objeto.fechaFin));
            if (objeto.template != null)
                parametros.Add(acceso.CrearParametro("@ID_Template", objeto.template.id));
            else
                parametros.Add(acceso.CrearParametro("@ID_Template", DBNull.Value));

            if (objeto.mensaje != null)
                parametros.Add(acceso.CrearParametro("@ID_Mensaje", objeto.mensaje.id));
            else
                parametros.Add(acceso.CrearParametro("@ID_Mensaje", DBNull.Value));


            Abrir();
            int resultado = acceso.Escribir("Publicidad_EDITAR", parametros);

            List<SqlParameter> parametrosABorrar = new List<SqlParameter>();
            parametrosABorrar.Add(acceso.CrearParametro("@id", objeto.id));
            acceso.Escribir("Publicidad_BORRAR_SEGMENTOS", parametrosABorrar);

            if (objeto.segmentos.Any()) {
                foreach (Segmento segmento in objeto.segmentos)
                {
                    List<SqlParameter> parametrosSegmento = new List<SqlParameter>();
                    parametrosSegmento.Add(acceso.CrearParametro("@ID_Publicidad", objeto.id));
                    parametrosSegmento.Add(acceso.CrearParametro("@ID_Segmento", segmento.id));
                    acceso.Escribir("Publicidad_INSERTAR_SEGMENTO", parametrosSegmento);
                }
            }
            Cerrar();
            return resultado;
        }

        public override int Insertar(Publicidad objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));
            parametros.Add(acceso.CrearParametro("@descripcion", objeto.descripcion));
            parametros.Add(acceso.CrearParametro("@fechaInicio", objeto.fechaInicio));
            parametros.Add(acceso.CrearParametro("@fechaFin", objeto.fechaFin));
            if (objeto.mensaje != null)
                parametros.Add(acceso.CrearParametro("@ID_Mensaje", objeto.mensaje.id));
            else
                parametros.Add(acceso.CrearParametro("@ID_Mensaje", DBNull.Value));
            if (objeto.template != null)
                parametros.Add(acceso.CrearParametro("@ID_Template", objeto.template.id));
            else
                parametros.Add(acceso.CrearParametro("@ID_Template", DBNull.Value));

            Abrir();
            int resultado = acceso.Escribir("Publicidad_INSERTAR", parametros);
            DataTable tabla = acceso.Leer("Publicidad_LISTAR_ULTIMO_ID");
            int ultimoId = 0;
            foreach (DataRow registro in tabla.Rows)
            {
                 ultimoId = int.Parse(registro["ID_Publicidad"].ToString());
            }

            if (objeto.segmentos.Any())
            {
                foreach (Segmento segmento in objeto.segmentos)
                {
                    List<SqlParameter> parametrosSegmento = new List<SqlParameter>();
                    parametrosSegmento.Add(acceso.CrearParametro("@ID_Publicidad", objeto.id));
                    parametrosSegmento.Add(acceso.CrearParametro("@ID_Segmento", segmento.id));
                    acceso.Escribir("Publicidad_INSERTAR_SEGMENTO", parametrosSegmento);
                }
            }
            Cerrar();
            return resultado;
        }

        public override List<Publicidad> Listar()
        {
            Abrir();
            DataTable tabla = acceso.Leer("Publicidad_LISTAR");

            DAL_Segmento dalSegmentos = new DAL_Segmento(acceso);

            DAL_Template dalTemplate = new DAL_Template(acceso);
            List<Template> templates = dalTemplate.Listar();

            DAL_Mensaje dalMensaje = new DAL_Mensaje(acceso);
            List<Mensaje> mensajes = dalMensaje.Listar();

            List<Publicidad> segmentos = new List<Publicidad>();
            foreach (DataRow registro in tabla.Rows)
            {
                Publicidad p = new Publicidad();
                p.id = int.Parse(registro["ID_Publicidad"].ToString());
                p.nombre = registro["Nombre"].ToString();
                p.descripcion = registro["Descripcion"].ToString();
                p.fechaInicio = (DateTime)registro["FechaInicio"];
                p.fechaFin = (DateTime)registro["FechaFin"];

                p.segmentos = dalSegmentos.ListarSegmentosPublicidad(p.id);

                if (registro["ID_Template"] != null) { 
                p.template = (from Template t in templates
                              where t.id == int.Parse(registro["ID_Template"].ToString())
                               select t
                          ).First();
                }

                if (registro["ID_Mensaje"] != null)
                {
                    p.mensaje = (from Mensaje m in templates
                                  where m.id == int.Parse(registro["ID_Mensaje"].ToString())
                                  select m
                              ).First();
                }

                segmentos.Add(p);
            }
            
            Cerrar();

            return segmentos;
        }

        public Publicidad Listar(int id)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", id));
            DataTable tabla = acceso.Leer("Publicidad_LISTAR_ID",parametros);

            DAL_Segmento dalSegmentos = new DAL_Segmento(acceso);

            DAL_Template dalTemplate = new DAL_Template(acceso);
            List<Template> templates = dalTemplate.Listar();

            DAL_Mensaje dalMensaje = new DAL_Mensaje(acceso);
            List<Mensaje> mensajes = dalMensaje.Listar();

            List<Publicidad> segmentos = new List<Publicidad>();
            foreach (DataRow registro in tabla.Rows)
            {
                Publicidad p = new Publicidad();
                p.id = int.Parse(registro["ID_Publicidad"].ToString());
                p.nombre = registro["Nombre"].ToString();
                p.descripcion = registro["Descripcion"].ToString();
                p.fechaInicio = (DateTime)registro["FechaInicio"];
                p.fechaFin = (DateTime)registro["FechaFin"];

                p.segmentos = dalSegmentos.ListarSegmentosPublicidad(p.id);

                if (registro["ID_Template"] != null)
                {
                    p.template = (from Template t in templates
                                  where t.id == int.Parse(registro["ID_Template"].ToString())
                                  select t
                              ).First();
                }

                if (registro["ID_Mensaje"] != null)
                {
                    p.mensaje = (from Mensaje m in templates
                                 where m.id == int.Parse(registro["ID_Mensaje"].ToString())
                                 select m
                              ).First();
                }

                segmentos.Add(p);
            }

            Cerrar();

            return segmentos[0];
        }
    }
}
