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
    public class DAL_Usuario : MAPPER<Usuario>
    {
        public DAL_Usuario()
    {
        acceso = new ACCESO();
        accesopropio = true;
    }

        public bool RecalcularDigitos()
        {
            try
            {
                Abrir();
                DataTable tabla = acceso.Leer("USUARIO_LISTAR");
                Cerrar();


                DAL_Idioma mp_idioma = new DAL_Idioma();
                int dvv = 0;
                foreach (DataRow registro in tabla.Rows)
                {
                    Idioma idioma = new Idioma();

                    idioma = mp_idioma.Listar(int.Parse(registro["ID_idioma"].ToString()));

                    Usuario usu = new Usuario(int.Parse(registro["ID_Usuario"].ToString()), registro["usuario"].ToString(), registro["estado"].ToString(), registro["nombre"].ToString(), registro["apellido"].ToString(), registro["correo"].ToString(), registro["dvh"].ToString(), idioma);

                    string digitoBase = registro["dvh"].ToString();

                    string digitoCalculado = usu.CalcularDigito();

                    bool valido = digitoBase.Equals(digitoCalculado);
                    if (!valido)
                    {
                        List<SqlParameter> parametros = new List<SqlParameter>();
                        parametros.Add(acceso.CrearParametro("@id", usu.ID));
                        parametros.Add(acceso.CrearParametro("@dvh", digitoCalculado));
                        Abrir();
                        int resultado = acceso.Escribir("Usuario_DVH", parametros);
                        Cerrar();
                    }

                    dvv += Int32.Parse(digitoCalculado);

                }
                actualizarDVV(dvv.ToString());
                return true;
            }
            catch (Exception e) {
                return false;
            }
        }

        internal DAL_Usuario(ACCESO ac)
    {
        acceso = ac;
        accesopropio = false;
    }

    public override int Borrar(Usuario objeto)
    {
        List<SqlParameter> parametros = new List<SqlParameter>();
        parametros.Add(acceso.CrearParametro("@id", objeto.ID));
        Abrir();
        int resultado = acceso.Escribir("Usuario_ELIMINAR", parametros);
        Cerrar();
        return resultado;
    }

    public override int Editar(Usuario objeto)
    {
        List<SqlParameter> parametros = new List<SqlParameter>();
            string SP = "USUARIO_EDITAR";
            parametros.Add(acceso.CrearParametro("@id", objeto.ID));
        parametros.Add(acceso.CrearParametro("@usuario", objeto.usuario));
        if (objeto.password != null)
        {
             parametros.Add(acceso.CrearParametro("@password", objeto.password));
             SP = "USUARIO_EDITAR_PASS";
        }
        parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));
        parametros.Add(acceso.CrearParametro("@apellido", objeto.apellido));
        parametros.Add(acceso.CrearParametro("@correo", objeto.correo));
        parametros.Add(acceso.CrearParametro("@dvh", objeto.CalcularDigito()));
        parametros.Add(acceso.CrearParametro("@ID_idioma", objeto.idioma.Id));

        Abrir();
        int resultado = acceso.Escribir(SP, parametros);
        Cerrar();
        return resultado;
    }

    public override int Insertar(Usuario objeto)
    {
        List<SqlParameter> parametros = new List<SqlParameter>();
        parametros.Add(acceso.CrearParametro("@usuario", objeto.usuario));
        parametros.Add(acceso.CrearParametro("@password", objeto.password));
        parametros.Add(acceso.CrearParametro("@estado", "Activo"));
        parametros.Add(acceso.CrearParametro("@nombre", objeto.nombre));
        parametros.Add(acceso.CrearParametro("@apellido", objeto.apellido));
        parametros.Add(acceso.CrearParametro("@correo", objeto.correo));
       
        parametros.Add(acceso.CrearParametro("@dvh", objeto.CalcularDigito()));
        
        parametros.Add(acceso.CrearParametro("@ID_idioma", objeto.idioma.Id));

        Abrir();
        int resultado = acceso.Escribir("USUARIO_INSERTAR", parametros);
        Cerrar();
        return resultado;
    }

        public int EditarIdioma(Usuario usuario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", usuario.ID));
            parametros.Add(acceso.CrearParametro("@ID_idioma", usuario.idioma.Id));

            Abrir();
            int resultado = acceso.Escribir("Usuario_EDITAR_IDIOMA", parametros);
            Cerrar();
            return resultado;
        }

        public override List<Usuario> Listar()
    {
        Abrir();
        DataTable tabla = acceso.Leer("USUARIO_LISTAR");
        Cerrar();

        List<Usuario> usuarios = new List<Usuario>();
            DAL_Idioma mp_idioma = new DAL_Idioma();
            foreach (DataRow registro in tabla.Rows)
        {
                Idioma idioma = new Idioma();
                
                idioma = mp_idioma.Listar(int.Parse(registro["ID_idioma"].ToString()));

                Usuario usu = new Usuario(int.Parse(registro["ID_Usuario"].ToString()), registro["usuario"].ToString(), registro["estado"].ToString(), registro["nombre"].ToString(), registro["apellido"].ToString(), registro["correo"].ToString(), registro["dvh"].ToString(), idioma);
                string digitoBase = registro["dvh"].ToString();
                string digitoCalculado = usu.CalcularDigito();
                bool valido = digitoBase.Equals(digitoCalculado);
                if (!valido)
                {
                    throw new DvhException("DVH incorrecto usuario ID: " + usu.ID.ToString());
                }

                usuarios.Add(usu);

        }
        return usuarios;
    }

    public Usuario Listar(int ID)
    {
        Abrir();
        List<SqlParameter> parametros = new List<SqlParameter>();
        parametros.Add(acceso.CrearParametro("@id", ID));

        DataTable tabla = acceso.Leer("USUARIO_LISTAR_ID", parametros);
        Cerrar();
            if (tabla.Rows.Count < 1)
                return null;
            DataRow registro = tabla.Rows[0];
            string digitoBase = registro["dvh"].ToString();

            Idioma idioma = new Idioma();
            DAL_Idioma mp_idioma = new DAL_Idioma();
            idioma = mp_idioma.Listar(int.Parse(registro["ID_idioma"].ToString()));

            Usuario u = new Usuario(int.Parse(registro["ID_Usuario"].ToString()), registro["usuario"].ToString(), registro["estado"].ToString(), registro["nombre"].ToString(), registro["apellido"].ToString(), registro["correo"].ToString(), registro["dvh"].ToString(), idioma);
            string digitoCalculado = u.CalcularDigito();
            bool valido = digitoBase.Equals(digitoCalculado);
            if (!valido)
            {
                throw new DvhException("DVH incorrecto usuario ID: " + u.ID.ToString() );
            }
            return u;
    }

    public Usuario Consulta_login(string usuario, string password)
    {
        Abrir();
        List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@usuario", usuario));
            parametros.Add(acceso.CrearParametro("@password", password));
        DataTable tabla = acceso.Leer("USUARIO_CONSULTA", parametros);
        Cerrar();
            if (tabla.Rows.Count < 1)
                return null;
            string username = tabla.Rows[0]["usuario"].ToString();
            Usuario u = new Usuario();
            if (usuario.Equals(username))
            {
                
                u.ID = int.Parse(tabla.Rows[0]["Id_Usuario"].ToString());
                u.usuario = username;
                u.nombre = tabla.Rows[0]["nombre"].ToString();
                u.apellido = tabla.Rows[0]["apellido"].ToString();
                u.correo = tabla.Rows[0]["correo"].ToString();

                Idioma idioma = new Idioma();
                DAL_Idioma mp_idioma = new DAL_Idioma();
                idioma = mp_idioma.Listar(int.Parse(tabla.Rows[0]["ID_idioma"].ToString()));
                u.idioma = idioma;

                return u;
            }
            else
            {
                return null;
            }
    }

        public int Activar(Usuario objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.ID));
            Abrir();
            int resultado = acceso.Escribir("USUARIO_ACTIVAR", parametros);
            Cerrar();
            return resultado;
        }

        public int Desactivar(Usuario objeto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", objeto.ID));
            Abrir();
            int resultado = acceso.Escribir("USUARIO_DESACTIVAR", parametros);
            Cerrar();
            return resultado;
        }

        public int Bloquear(string username)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@usuario", username));
            Abrir();
            int resultado = acceso.Escribir("USUARIO_BLOQUEAR", parametros);
            Cerrar();
            return resultado;
        }

        public string getDVV(string tabla1)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@tabla", tabla1));

            DataTable tabla = acceso.Leer("GET_DVV", parametros);
            Cerrar();
            DataRow registro = tabla.Rows[0];
            string DVV = registro["DVV"].ToString();
            return DVV;
        }

        public string calcularDVV()
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            
            DataTable tabla = acceso.Leer("CALCULAR_DVV_USUARIO", parametros);
            Cerrar();
            DataRow registro = tabla.Rows[0];
            string DVV = registro["total"].ToString();
            return DVV;
            
        }

        public void actualizarDVV(string valor)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@dvv", valor));

            DataTable tabla = acceso.Leer("[DVV_USUARIO_ACTUALIZAR]", parametros);
            Cerrar();
        }

        public Usuario Listar(string usuario)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@usuario", usuario));

            DataTable tabla = acceso.Leer("USUARIO_LISTAR_USERNAME", parametros);
            Cerrar();
            if (tabla.Rows.Count < 1)
                return null;
            DataRow registro = tabla.Rows[0];

          Idioma idioma = new Idioma();
           DAL_Idioma mp_idioma = new DAL_Idioma();
           idioma = mp_idioma.Listar(int.Parse(registro["ID_idioma"].ToString()));

            return new Usuario(int.Parse(registro["ID_Usuario"].ToString()), registro["usuario"].ToString(), registro["estado"].ToString(), registro["nombre"].ToString(), registro["apellido"].ToString(), registro["correo"].ToString(), registro["dvh"].ToString(), idioma);
        }


    }
}
