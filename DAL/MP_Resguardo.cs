using BE;
using DAL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MP_Resguardo : MAPPER<Resguardo>
    {
        public MP_Resguardo()
        {
            acceso = new ACCESO();
            accesopropio = true;
        }

        internal MP_Resguardo(ACCESO ac)
        {
            acceso = ac;
            accesopropio = false;
        }

        public override int Borrar(Resguardo objeto)
        {
            return 0;
        }

        public override int Editar(Resguardo objeto)
        {
            return 0;
        }

        public override int Insertar(Resguardo objeto)
        {

            Abrir();
            string pathCompleto = objeto.Path + objeto.Nombre;
            var query = String.Format("BACKUP DATABASE [{0}] TO DISK='{1}'", "toomarketing", pathCompleto);
            acceso.EscribirQuery(query, new List<SqlParameter>());

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@idUsuario", objeto.Usuario.ID));
            parametros.Add(acceso.CrearParametro("@tipo", objeto.Tipo));
            parametros.Add(acceso.CrearParametro("@nombre", objeto.Nombre));
            parametros.Add(acceso.CrearParametro("@fecha", objeto.Fecha));
  
            
            int resultado = acceso.Escribir("RESGUARDO_INSERTAR", parametros);
            Cerrar();
            return resultado;
        }

        public int Restore(Resguardo r)
        {
            try
            {
                Abrir();
                string pathCompleto = r.Path + r.Nombre;

                var query1 = String.Format("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", "toomarketing");
                acceso.EscribirQuery(query1, new List<SqlParameter>());

                var query = String.Format("USE MASTER RESTORE DATABASE [{0}] FROM  DISK='{1}'", "toomarketing", pathCompleto);
                acceso.EscribirQuery(query, new List<SqlParameter>());

                var query2 = String.Format("ALTER DATABASE [{0}] SET MULTI_USER", "toomarketing");
                acceso.EscribirQuery(query2, new List<SqlParameter>());

                Abrir();
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acceso.CrearParametro("@idUsuario", r.Usuario.ID));
                parametros.Add(acceso.CrearParametro("@tipo", r.Tipo));
                parametros.Add(acceso.CrearParametro("@nombre", r.Nombre));
                parametros.Add(acceso.CrearParametro("@fecha", DateTime.Now));


                int resultado = acceso.Escribir("RESGUARDO_INSERTAR", parametros);
                Cerrar();
                return resultado;
            }
            catch (Exception e)
            {
                return -1;
            }
            
        }

        public override List<Resguardo> Listar()
        {
            Abrir();

            MP_Usuario mp_usuario = new MP_Usuario(acceso);

            try
            {
                List<Usuario> usuarios = mp_usuario.Listar();

                mp_usuario = null;

                DataTable tabla = acceso.Leer("RESGUARDO_LISTAR");
                Cerrar();
                List<Resguardo> resguardos = new List<Resguardo>();
                foreach (DataRow registro in tabla.Rows)
                {
                    Resguardo c = new Resguardo();
                    c.Id = int.Parse(registro["id"].ToString());
                    c.Nombre = registro["nombre"].ToString();
                    c.Tipo = registro["tipo"].ToString();
                    c.Fecha = DateTime.Parse(registro["fecha"].ToString());

                    int usuarioId = int.Parse(registro["id_usuario"].ToString());

                    c.Usuario = (from Usuario u in usuarios
                                 where u.ID == usuarioId
                                 select u
                           ).First();

                    resguardos.Add(c);
                }
                return resguardos;
            }
            catch (DvhException e)
            {
                throw new DvhException(e.Message);
            }
        }

        public Resguardo Listar(int ID)
        {
            Abrir();

            MP_Usuario mp_usuario = new MP_Usuario(acceso);

            List<Usuario> usuarios = mp_usuario.Listar();

            mp_usuario = null;

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", ID));
            DataTable tabla = acceso.Leer("RESGUARDO_LISTAR_ID",parametros);
            Cerrar();
            DataRow registro = tabla.Rows[0];

            Resguardo c = new Resguardo();
                c.Id = int.Parse(registro["id"].ToString());
                c.Nombre = registro["nombre"].ToString();
                c.Tipo = registro["tipo"].ToString();
                c.Fecha = DateTime.Parse(registro["fecha"].ToString());

                int usuarioId = int.Parse(registro["id_usuario"].ToString());

                c.Usuario = (from Usuario u in usuarios
                             where u.ID == usuarioId
                             select u
                       ).First();

            return c;
        }
    }
}
