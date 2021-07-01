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
    public class DAL_Idioma : MAPPER<Idioma>
    {
        public DAL_Idioma()
        {
            acceso = new ACCESO();
            accesopropio = true;
        }

        internal DAL_Idioma(ACCESO ac)
        {
            acceso = ac;
            accesopropio = false;
        }

        #region Idiomas
        public override int Editar(Idioma idioma)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@Id_idioma", idioma.Id));
            parametros.Add(acceso.CrearParametro("@descripcion", idioma.Descripcion));

            Abrir();
            int resultado = acceso.Escribir("IDIOMA_EDITAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Insertar(Idioma idioma)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@descripcion", idioma.Descripcion));

            Abrir();
            int id_idioma = acceso.Escribir_Con_Resultado("IDIOMA_INSERTAR", parametros);
            Cerrar();
            return id_idioma;
        }

        public override List<Idioma> Listar()
        {
            Abrir();
            DataTable tabla = acceso.Leer("IDIOMA_LISTAR");
            Cerrar();
            List<Idioma> idiomas = new List<Idioma>();
            foreach (DataRow registro in tabla.Rows)
            {
                idiomas.Add(new Idioma(int.Parse(registro["Id_idioma"].ToString()), registro["Descripcion"].ToString()));
            }
            return idiomas;
        }

        public Idioma Listar(int id_idioma)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@Id_idioma", id_idioma));

            DataTable tabla = acceso.Leer("Idioma_LISTAR_ID", parametros);
            Cerrar();
            DataRow registro = tabla.Rows[0];
            return new Idioma(int.Parse(registro["Id_idioma"].ToString()), registro["Descripcion"].ToString());
        }

        public Idioma Listar(string nombre_idioma)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@Nombre_idioma", nombre_idioma));

            DataTable tabla = acceso.Leer("Idioma_LISTAR_NOMBRE", parametros);
            Cerrar();
            DataRow registro = tabla.Rows[0];
            return new Idioma(int.Parse(registro["Id_idioma"].ToString()), registro["Nombre_idioma"].ToString());
        }

        public override int Borrar(Idioma idioma)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@Id_idioma", idioma.Id));

            Abrir();
            int resultado = acceso.Escribir("IDIOMA_ELIMINAR", parametros);
            Cerrar();
            return resultado;
        }
        #endregion

        #region Etiquetas

        public int InsertarTraduccion(Etiqueta etiqueta, int Id_idioma)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id_idioma", Id_idioma));
            parametros.Add(acceso.CrearParametro("@id_etiqueta", etiqueta.ID));
            parametros.Add(acceso.CrearParametro("@traduccion", etiqueta.Traduccion));

            Abrir();
            int resultado = acceso.Escribir("Idioma_INSERTAR_TRADUCCION", parametros);
            Cerrar();
            return resultado;
        }

        public int EditarEtiqueta(Etiqueta etiqueta)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@Id_etiqueta", etiqueta.ID));
            parametros.Add(acceso.CrearParametro("@Traduccion", etiqueta.Traduccion));

            Abrir();
            int resultado = acceso.Escribir("Idioma_EDITAR_TRADUCCION", parametros);
            Cerrar();
            return resultado;
        }

        public List<Etiqueta> ListarEtiquetas()
        {
            Abrir();
            DataTable tabla = acceso.Leer("Idioma_LISTAR_ETIQUETAS");
            Cerrar();
            List<Etiqueta> etiquetas = new List<Etiqueta>();
            foreach (DataRow registro in tabla.Rows)
            {
                etiquetas.Add(new Etiqueta(int.Parse(registro["Id_etiqueta"].ToString()), registro["Descripcion"].ToString()));
            }
            return etiquetas;
        }

        public Etiqueta ListarEtiqueta(int id_etiqueta)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@ID", id_etiqueta));

            DataTable tabla = acceso.Leer("Idioma_LISTAR_ETIQUETAS_ID", parametros);
            Cerrar();
            DataRow registro = tabla.Rows[0];
            return new Etiqueta(int.Parse(registro["Id_etiqueta"].ToString()), registro["Descripcion"].ToString());
        }

        public List<Etiqueta> ListarEtiquetasPorIdioma(string idioma)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@idioma", idioma));
            DataTable tabla = acceso.Leer("Idioma_LISTAR_ETIQUETA_ID", parametros);
            Cerrar();
            List<Etiqueta> etiquetas = new List<Etiqueta>();
            foreach (DataRow registro in tabla.Rows)
            {
                etiquetas.Add(new Etiqueta(int.Parse(registro["Id_etiqueta"].ToString()), registro["Descripcion"].ToString(), registro["Traduccion"].ToString()));
            }
            return etiquetas;
        }

        public int BorrarTraduccion(int Id_idioma, int Id_etiqueta)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@Id_idioma", Id_idioma));
            parametros.Add(acceso.CrearParametro("@Id_etiqueta", Id_etiqueta));

            Abrir();
            int resultado = acceso.Escribir("Idioma_ELIMINAR_TRADUCCION", parametros);
            Cerrar();
            return resultado;
        }

        public int BorrarEtiquetasIdioma(Idioma idioma)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@Id_idioma", idioma.Id));

            Abrir();
            int resultado = acceso.Escribir("IDIOMA_ELIMINAR_ETIQUETAS_IDIOMA", parametros);
            Cerrar();
            return resultado;
        }

        #endregion
    }
}

