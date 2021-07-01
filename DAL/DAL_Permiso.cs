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
    public class DAL_Permiso : MAPPER<Permiso>
    {
        public DAL_Permiso()
        {
            acceso = new ACCESO();
            accesopropio = true;
        }

        internal DAL_Permiso(ACCESO ac)
        {
            acceso = ac;
            accesopropio = false;
        }

        public int EliminarRoles(Permiso permiso)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id_permiso", permiso.ID));
            Abrir();
            int resultado = acceso.Escribir("PERMISO_ELIMINAR_ROL", parametros);
            Cerrar();
            return resultado;
        }

        public int EliminarPermisos(Permiso permiso)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id_permiso", permiso.ID));
            Abrir();
            int resultado = acceso.Escribir("PERMISO_ELIMINAR_PERMISO", parametros);
            Cerrar();
            return resultado;
        }

        private Componente GetComponent(int id, List<Componente> lista)
        {
            Componente component = lista != null ? lista.Where(i => i.ID.Equals(id)).FirstOrDefault() : null;
            if (component == null && lista != null)
            {
                foreach (var c in lista)
                {
                    var l = GetComponent(id, c.ObtenerHijos());
                    if (l != null && l.ID == id) return l;
                    else
                    if (l != null)
                        return GetComponent(id, l.ObtenerHijos());
                }
            }
            return component;
        }

        public List<Componente> Listar(string familia)
        {
            Abrir();
            var where = "is NULL";

            if (!String.IsNullOrEmpty(familia))
            {
                where = familia;
            }

            var consulta = $@"with recursivo as (
                        select sp2.Id_padre, sp2.Id_hijo  from PermisoPermiso SP2
                        where sp2.Id_padre {where} --acá se va variando la familia que busco
                        UNION ALL 
                        select sp.Id_padre, sp.Id_hijo from PermisoPermiso sp 
                        inner join recursivo r on r.Id_hijo= sp.Id_padre
                        )
                        select r.Id_padre,r.Id_hijo,p.Id_permiso,p.Nombre, p.Tipo
                        from recursivo r 
                        inner join permiso p on r.Id_hijo = p.Id_permiso 
						
                        ";

            var reader = acceso.Read(consulta);

            var lista = new List<Componente>();

            while (reader.Read())
            {
                int id_padre = 0;
                if (reader["Id_padre"] != DBNull.Value)
                {
                    id_padre = reader.GetInt32(reader.GetOrdinal("Id_padre"));
                }

                var id = reader.GetInt32(reader.GetOrdinal("Id_permiso"));
                var nombre = reader.GetString(reader.GetOrdinal("Nombre"));
                var patente = reader.GetString(reader.GetOrdinal("Tipo"));


                Componente c;

                if (patente == "familia")
                    c = new Familia();
                else
                    c = new Patente();

                c.ID = id;
                c.Nombre = nombre;

                var padre = GetComponent(id_padre, lista);

                if (padre == null)
                {
                    lista.Add(c);
                }
                else
                {
                    padre.AgregarHijos(c);
                }

            }
            Cerrar();
            return lista;
        }

        public override int Insertar(Permiso permiso)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            int Id_permiso = 0;
            parametros.Add(acceso.CrearParametro("@Nombre", permiso.Nombre));
            parametros.Add(acceso.CrearParametro("@Tipo", permiso.Tipo));

            Abrir();
            Id_permiso = acceso.Escribir_Con_Resultado("Permiso_INSERTAR", parametros);
            Cerrar();
            return Id_permiso;
        }

        public int AsignarPermisoUsuario(int id_permiso, int id_usuario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id_permiso", id_permiso));
            parametros.Add(acceso.CrearParametro("@id_usuario", id_usuario));

            Abrir();
            int resultado = acceso.Escribir("PERMISO_ASIGNAR", parametros);
            Cerrar();
            return resultado;
        }

        public int DesasignarPermisoUsuario(int id_permiso, int id_usuario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id_permiso", id_permiso));
            parametros.Add(acceso.CrearParametro("@id_usuario", id_usuario));

            Abrir();
            int resultado = acceso.Escribir("PERMISO_DESASIGNAR", parametros);
            Cerrar();
            return resultado;
        }


        public int AsignacionRoles(Permiso permiso, Permiso permisoPadre)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            string SP = "";
            if (permisoPadre.Nombre == "NULL")
            {
                parametros.Add(acceso.CrearParametro("@Id_hijo", permiso.ID));
                SP = "PERMISOPermiso_INSERTAR_VACIO";
            }
            else
            {
                parametros.Add(acceso.CrearParametro("@Id_padre", permisoPadre.ID));
                parametros.Add(acceso.CrearParametro("@Id_hijo", permiso.ID));
                SP = "PERMISOPermiso_INSERTAR";
            }

            int resultado = acceso.Escribir(SP, parametros);
            Cerrar();
            return resultado;
        }


        public override List<Permiso> Listar()
        {
            Abrir();
            DataTable tabla = acceso.Leer("PERMISO_LISTAR_TODO");
            Cerrar();
            List<Permiso> permisos = new List<Permiso>();
            foreach (DataRow registro in tabla.Rows)
            {
                Permiso permiso = new Permiso();
                permiso.ID = Convert.ToInt32(registro["Id_permiso"]);
                permiso.Nombre = registro["Nombre"].ToString();
                permiso.Tipo = registro["Tipo"].ToString();
                permisos.Add(permiso);
            }
            return permisos;
        }


        public Permiso BuscarPermiso(int id)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", id));
            DataTable tabla = acceso.Leer("PERMISO_LISTAR_ID", parametros);
            Cerrar();

            Permiso permiso = new Permiso();
            DataRow registro = tabla.Rows[0];
            permiso.ID = Convert.ToInt32(registro["Id_permiso"]);
            permiso.Nombre = registro["Nombre"].ToString();
            permiso.Tipo = registro["Tipo"].ToString();

            return permiso;
        }

        public List<Permiso> VerificarPermisosUsuario(int id_usuario)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@ID", id_usuario));
            DataTable tabla = acceso.Leer("USUARIO_CONSULTA_PERMISOS", parametros);
            Cerrar();

            List<Permiso> lista = new List<Permiso>();

            foreach (DataRow registro in tabla.Rows)
            {
                Permiso permiso = new Permiso();
                permiso.ID = int.Parse(registro["Id_permiso"].ToString());
                permiso.Nombre = registro["Nombre"].ToString();
                permiso.Tipo = registro["Tipo"].ToString();
                lista.Add(permiso);
            }

            return lista;
        }

        public List<Permiso> ListarPermisosDeFamilia(int id_permiso)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@ID", id_permiso));
            DataTable tabla = acceso.Leer("PERMISO_CONSULTA_PERMISOS_DE_FAMILIA", parametros);
            Cerrar();

            List<Permiso> lista = new List<Permiso>();

            foreach (DataRow registro in tabla.Rows)
            {
                Permiso permiso = new Permiso();
                permiso.ID = int.Parse(registro["Id_permiso"].ToString());
                lista.Add(permiso);
            }

            return lista;
        }

        public List<Permiso> ListarFamilias()
        {
            Abrir();
            DataTable tabla = acceso.Leer("Permiso_LISTAR_FAMILIAS");
            Cerrar();

            List<Permiso> lista = new List<Permiso>();

            foreach (DataRow registro in tabla.Rows)
            {
                Permiso permiso = new Permiso();
                permiso.ID = int.Parse(registro["Id_permiso"].ToString());
                permiso.Nombre = registro["Nombre"].ToString();
                permiso.Tipo = registro["Tipo"].ToString();
                lista.Add(permiso);
            }

            return lista;
        }

        public List<Permiso> ListarPatentes()
        {
            Abrir();
            DataTable tabla = acceso.Leer("Permiso_LISTAR_PATENTES");
            Cerrar();

            List<Permiso> lista = new List<Permiso>();

            foreach (DataRow registro in tabla.Rows)
            {
                Permiso permiso = new Permiso();
                permiso.ID = int.Parse(registro["Id_permiso"].ToString());
                permiso.Nombre = registro["Nombre"].ToString();
                permiso.Tipo = registro["Tipo"].ToString();
                lista.Add(permiso);
            }

            return lista;
        }

        public override int Editar(Permiso permiso)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@id", permiso.ID));
            parametros.Add(acceso.CrearParametro("@Nombre", permiso.Nombre));

            Abrir();
            int resultado = acceso.Escribir("PERMISO_EDITAR", parametros);
            Cerrar();
            return resultado;
        }

        public override int Borrar(Permiso objeto)
        {
            throw new NotImplementedException();
        }

        public bool BusquedaRecursiva(string familia, string permiso)
        {
            Abrir();
            var where = "is NULL";

            if (!String.IsNullOrEmpty(familia))
            {
                if (!familia.Equals("1"))
                    where = "= " + familia;
            }

            var consulta = $@"with recursivo as (
                        select sp2.Id_padre, sp2.Id_hijo  from PermisoPermiso SP2
                        where sp2.Id_padre {where} --acá se va variando la familia que busco
                        UNION ALL 
                        select sp.Id_padre, sp.Id_hijo from PermisoPermiso sp 
                        inner join recursivo r on r.Id_hijo= sp.Id_padre
                        )
                        select r.Id_padre,r.Id_hijo,p.Id_permiso,p.Nombre, p.Tipo
                        from recursivo r 
                        inner join permiso p on r.Id_hijo = p.Id_permiso 
						
                        ";

            var reader = acceso.Read(consulta);

            var lista = new List<Componente>();

            while (reader.Read())
            {
                int id_padre = 0;
                if (reader["Id_padre"] != DBNull.Value)
                {
                    id_padre = reader.GetInt32(reader.GetOrdinal("Id_padre"));
                }

                var id = reader.GetInt32(reader.GetOrdinal("Id_permiso"));
                var nombre = reader.GetString(reader.GetOrdinal("Nombre"));
                var patente = reader.GetString(reader.GetOrdinal("Tipo"));

                if (nombre.Equals(permiso))
                {
                    Cerrar();
                    return true;
                }

            }
            Cerrar();

            Permiso patenteSinFamilia = new Permiso();
            patenteSinFamilia = BuscarPermiso(int.Parse(familia));
            if (patenteSinFamilia.Nombre.Equals(permiso))
                return true;
            return false;
        }

        public Permiso BuscarPermiso(string nombre)
        {
            Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@Nombre", nombre));
            DataTable tabla = acceso.Leer("Permiso_LISTAR_NOMBRE", parametros);
            Cerrar();

            Permiso permiso = new Permiso();
            DataRow registro = tabla.Rows[0];
            permiso.ID = Convert.ToInt32(registro["Id_permiso"]);
            permiso.Nombre = registro["Nombre"].ToString();
            permiso.Tipo = registro["Tipo"].ToString();

            return permiso;
        }

    }
}
