using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace DAL
{
    internal class ACCESO
    {                    

        SqlConnection conexion;
        
        SqlTransaction transaccion;

        public void Abrir()
        {
            conexion = new SqlConnection();
            conexion.ConnectionString = "Initial Catalog=toomarketing;User=sa; Password=Admin1234; Data Source=.\\SQLEXPRESS;";
            conexion.Open();
        }

        public void Cerrar()
        {
            conexion.Close();
            conexion.Dispose();
            conexion = null;
            GC.Collect();
        }

        public void IniciarTransaccion()
        {
            transaccion= conexion.BeginTransaction();
            
        }
        
        public void ConfirmarTransaccion()
        {
            transaccion.Commit();
            
        }

        public void DeshacerTransaccion()
        {
            transaccion.Rollback();
        }

        private SqlCommand CrearComando(string nombre, List<SqlParameter> parametros = null)
        {
            SqlCommand comando = new SqlCommand();
            comando.CommandText = nombre;
            comando.CommandType = CommandType.StoredProcedure;
            comando.Connection = conexion;
            if (transaccion != null)
            {
                comando.Transaction = transaccion;
            }


            if (parametros != null && parametros.Count > 0)
            {
                comando.Parameters.AddRange(parametros.ToArray());
            }
            return comando;            
        }

        public int Escribir(string nombre, List<SqlParameter> parameters)
        {
            SqlCommand comando = CrearComando(nombre, parameters);
            int filasafectadas;
            try
            {
                filasafectadas = comando.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                filasafectadas = -1;
            }
            comando.Parameters.Clear();
            comando.Dispose();
            comando = null;
            GC.Collect();
            return filasafectadas;  
        }

        public DataTable Leer(string nombre, List<SqlParameter> parameters = null)
        {
            SqlDataAdapter adaptador = new SqlDataAdapter();
            adaptador.SelectCommand = CrearComando(nombre, parameters);
            DataTable tabla = new DataTable();

            adaptador.Fill(tabla);

            return tabla;
        }

        public SqlParameter CrearParametro(string nombre, string valor)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = nombre;
            p.Value = valor;
            p.SqlDbType = SqlDbType.Text;
            return p;
        }

        public SqlParameter CrearParametro(string nombre, int valor)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = nombre;
            p.Value = valor;
            p.SqlDbType = SqlDbType.Int;
            return p;
        }

        public SqlParameter CrearParametro(string nombre, DateTime valor)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = nombre;
            p.Value = valor;
            p.SqlDbType = SqlDbType.DateTime;
            return p;
        }

        public SqlParameter CrearParametro(string nombre, double valor)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = nombre;
            p.Value = valor;
            p.SqlDbType = SqlDbType.Float;
            return p;
        }

        public SqlParameter CrearParametro(string nombre, bool valor)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = nombre;
            p.Value = valor;
            p.SqlDbType = SqlDbType.Bit;
            return p;
        }

        public void EscribirQuery(string query, List<SqlParameter> parametros)
        {
            SqlCommand unComando = new SqlCommand();

            unComando.Connection = conexion;

            if (transaccion != null)
            {
                unComando.Transaction = transaccion;
            }

            unComando.CommandType = CommandType.Text;
            //unComando.CommandText = "INSERT INTO Cliente (cuit,razon_social,mail,direccion,descripcion) VALUES (2,'algo','ll@pepe.com','unadure', 'lala')";
            unComando.CommandText = query;

            if (parametros != null && parametros.Count > 0)
            {
                unComando.Parameters.AddRange(parametros.ToArray());
            }

            unComando.ExecuteNonQuery();

        }

        public int Escribir_Con_Resultado(string nombre, List<SqlParameter> parameters)
        {
            SqlCommand comando = CrearComando(nombre, parameters);
            int filasafectadas;
            try
            {
                filasafectadas = int.Parse(comando.ExecuteScalar().ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                filasafectadas = -1;
            }
            comando.Parameters.Clear();
            comando.Dispose();
            comando = null;
            GC.Collect();
            return filasafectadas;
        }

         public SqlDataReader Read(string consulta)
        {
            SqlDataReader dr = null;
            SqlCommand cmd = new SqlCommand(consulta, conexion);
            dr = cmd.ExecuteReader();
            return dr;

        }




    }
}
