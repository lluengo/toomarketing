using BE;
using BLL.Exceptions;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BLL
{
    public class IdiomaBLL
    {

        private MP_Idioma mp = new MP_Idioma();

        public List<Idioma> ListarIdiomas()
        {
            return mp.Listar();
        }

        public Idioma ListarIdioma(int id)
        {
            return mp.Listar(id);
        }

        public Idioma BuscarIdioma(string Nombre)
        {
            return mp.Listar(Nombre);
        }

        public List<Etiqueta> ListarEtiquetas()
        {
            return mp.ListarEtiquetas();
        }
        public List<Etiqueta> ListarEtiquetasPorIdioma(string idioma)
        {
            return mp.ListarEtiquetasPorIdioma(idioma);
        }

        public int AltaIdioma(Idioma idioma)
        {
            try
            {
                return mp.Insertar(idioma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int EditarIdioma(Idioma idioma)
        {
            try
            {
                return mp.Editar(idioma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int EditarEtiqueta(Etiqueta etiqueta)
        {
            try
            {
                return mp.EditarEtiqueta(etiqueta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int BorrarIdioma(Idioma idioma)
        {
            try
            {
                if (mp.BorrarEtiquetasIdioma(idioma) >= 0)
                    return mp.Borrar(idioma);
                else
                    return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AltaEtiqueta(Etiqueta etiqueta, int Id_idioma)
        {
            try
            {
                return mp.InsertarTraduccion(etiqueta, Id_idioma);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Etiqueta ListarEtiqueta(int id)
        {
            return mp.ListarEtiqueta(id);
        }

        public bool BorrarTraduccion(int Id_idioma, int Id_etiqueta)
        {
            try
            {
                mp.BorrarTraduccion(Id_idioma, Id_etiqueta);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        

        public string TranslateText(string input, string languaje)
        {
            try
            {
                string url = String.Format
                ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                 "es", languaje, Uri.EscapeUriString(input));
                HttpClient httpClient = new HttpClient();
                string result = httpClient.GetStringAsync(url).Result;
                var jsonData = new JavaScriptSerializer().Deserialize<List<dynamic>>(result);
                var translationItems = jsonData[0];
                string translation = "";

                foreach (object item in translationItems)
                {
                    IEnumerable translationLineObject = item as IEnumerable;
                    IEnumerator translationLineString = translationLineObject.GetEnumerator();
                    translationLineString.MoveNext();
                    translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
                }

                if (translation.Length > 1) { translation = translation.Substring(1); };
                return translation;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("429"))
                {
                    throw new GoogleException(ex.InnerException.Message.ToString());
                }
                throw;
            }
        }
    }
}

