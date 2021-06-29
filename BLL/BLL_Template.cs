using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Web;

namespace BLL
{

    public class BLL_Template : AbstractBLL<Template>
    {
        private DAL_Template dal = new DAL_Template();

        public override int Borrar(Template e)
        {
            return dal.Borrar(e);
        }

        public override int Grabar(Template e)
        {
            int resultado;
            if (e.id == 0)
            {
                resultado = dal.Insertar(e);
            }
            else
            {
                resultado = dal.Editar(e);
            }


            return resultado;
        }

        public override List<Template> Listar()
        {
            return dal.Listar();
        }

        public override Template Listar(int id)
        {
            return dal.Listar(id);
        }


        public string generateHTML (Template e)
        {
            string html = "";
            try
            { 
                string nombre = e.nroTemplate + ".html";

                string filename = HttpContext.Current.Server.MapPath(@"~\templates\" + nombre);

                string content = System.IO.File.ReadAllText(filename);

                content = content.Replace("string1", e.string1);
                content = content.Replace("string2", e.string2);
                content = content.Replace("string3", e.string3);
                content = content.Replace("string4", e.string4);

                html = content;

                return html;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return html;
        }
    }

   
}
