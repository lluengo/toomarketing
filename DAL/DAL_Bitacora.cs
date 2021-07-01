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
    public class DAL_Bitacora: MAPPER<Bitacora>
    {
        public DAL_Bitacora()
        {
            acceso = new ACCESO();
            accesopropio = true;
        }

        internal DAL_Bitacora(ACCESO ac)
        {
            acceso = ac;
            accesopropio = false;
        }

        public override int Borrar(Bitacora objeto)
        {
            throw new NotImplementedException();
        }

        public override int Editar(Bitacora objeto)
        {
            throw new NotImplementedException();
        }

        public override int Insertar(Bitacora objeto)
        {
            Abrir();
          
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@idUsuario",objeto.Usuario.ID));
            parametros.Add(acceso.CrearParametro("@tipo", objeto.Tipo.ToString()));
            parametros.Add(acceso.CrearParametro("@mensaje", objeto.mensaje));
            parametros.Add(acceso.CrearParametro("@fecha", objeto.fecha));


            int resultado = acceso.Escribir("BITACORA_INSERTAR", parametros);
            Cerrar();
            return resultado;
        }

        public override List<Bitacora> Listar()
        { 
        Abrir();

        DAL_Usuario mp_usuario = new DAL_Usuario(acceso);

        List<Usuario> usuarios = mp_usuario.Listar();

        mp_usuario = null;

            DataTable tabla = acceso.Leer("BITACORA_LISTAR");
            Cerrar();
        List<Bitacora> resguardos = new List<Bitacora>();
            foreach (DataRow registro in tabla.Rows)
            {
                Bitacora c = new Bitacora();
        c.Id = int.Parse(registro["Id"].ToString());
                c.mensaje = registro["Mensaje"].ToString();
        c.Tipo = (TipoLog) Enum.Parse(typeof(TipoLog), registro["Tipo"].ToString(), true);
        c.fecha = DateTime.Parse(registro["Fecha"].ToString());

                int usuarioId = int.Parse(registro["Id_Usuario"].ToString());

        c.Usuario = (from Usuario u in usuarios
                      where u.ID == usuarioId
                      select u
                       ).First();

        resguardos.Add(c);
            }
            return resguardos;
        }
    }
}
