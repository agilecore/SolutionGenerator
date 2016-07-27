using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Gerador.Connections
{
    public class ConnectionSqlServer : IDisposable
    {
        public bool Success;
        private String ErrorMessage = String.Empty;
        private String LocalQuery = String.Empty;
        private SqlConnection ConnectionDb;
        public SqlCommand CommandSql;
        private SqlDataReader Registro;

        /// <summary>
        /// Construtor desta classe onde é passado a Query a ser usada no banco e a string de conexao para se 
        /// conectar a outro banco, se a string de conexao for String.Empty, é usado a ConnectionDb de dados local.
        /// </summary>
        /// <param name="Query">Comando a ser executado no banco SELECT, INSERT, UPDATE, DELETE, DROP</param>
        /// <param name="LocalConnectionString">String de Conexao se desejar conectar a outro ConnectionDb ou String.Empty se deseja usar a ConnectionDb de dados Local (do Cliente)</param>
        public ConnectionSqlServer(String Query, String LocalConnectionString = "")
        {
            if (LocalConnectionString == String.Empty)
                LocalConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            this.LocalQuery = Query.Replace("#", "'").Trim();
            this.ConnectionDb = new SqlConnection(LocalConnectionString);
            this.CommandSql = new SqlCommand(this.LocalQuery, ConnectionDb);
            this.ErrorMessage = String.Empty;
            this.Success = true;
        }

        /// <summary>
        /// Passagem de parametro para uma query a ser executada no banco de dados, pode ser Select, Insert, Delete
        /// </summary>
        /// <param name="Name">Name do Campo da Tabela</param>
        /// <param name="Value">Value que o Campo da Tabela em questao ira armazenar</param>
        public void Parameter(String Name, String Value)
        {
            this.CommandSql.Parameters.AddWithValue(Name, Value);
        }

        /// <summary>
        /// Simplemente executa uma Query no banco de dados como um Insert, Delete ou Update
        /// </summary>
        /// <returns></returns>
        public bool Execute()
        {
            try
            {
                this.ConnectionDb.Open();
                this.CommandSql.ExecuteNonQuery();
                this.ConnectionDb.Close();
            }
            catch (Exception ex)
            {
                this.Success = false;
                this.ErrorMessage = ex.ToString() + "\n\nQuery: " + this.LocalQuery;
            }

            return this.Success;
        }

        /// <summary>
        ///  Metodo que retorna um DataSet de um comando de Select, DataSet trabalha com dados desconectado do banco
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataSet()
        {
            DataSet Ds = new DataSet("Tables");
            SqlDataAdapter Da = new SqlDataAdapter(this.LocalQuery, this.ConnectionDb);
            Da.Fill(Ds);

            return Ds;
        }

        /// <summary>
        /// Metodo que retorna um DataTable de um comando de Select
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataTable()
        {
            DataTable Dt = new DataTable();
            this.Registro = this.CommandSql.ExecuteReader();
            Dt.Load(this.Registro);
            this.Registro.Close();

            return Dt;
        }

        /// <summary>
        /// Metodo que retorna um DataReader de um comando de Select, DataReader trabalha com dados conectado ao banco
        /// </summary>
        /// <returns></returns>
        public SqlDataReader GetDataReader()
        {
            this.ConnectionDb.Open();
            this.CommandSql = new SqlCommand(this.LocalQuery, this.ConnectionDb);
            this.Registro = CommandSql.ExecuteReader();

            return this.Registro;
        }

        /// <summary>
        /// Metodo que abre a conexao com banco de dados retornando True para conexao aberta e False para conexao Fechada
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            try
            {
                this.ConnectionDb.Open();
                this.Registro = this.CommandSql.ExecuteReader();
            }
            catch (Exception ex)
            {
                this.Success = false;
                this.ErrorMessage = ex.ToString() + "\n\nQuery: " + this.LocalQuery;
            }

            return this.Success;
        }

        /// <summary>
        /// Retorna True enquanto retornar dados do Select e False se o comando Select não encontrar dados ou chegar a
        /// fim do leitor de dados 
        /// </summary>
        /// <returns></returns>
        public bool LerRegistro()
        {
            return this.Registro.Read();
        }

        /// <summary>
        /// Retorna o Value do Campo passado para este metodo
        /// </summary>
        /// <param name="Campo">Name do Campo do Select</param>
        /// <returns></returns>
        public Object Ler(String Campo)
        {
            object Retorno;

            if (this.Registro[Campo] != null)
                Retorno = this.Registro[Campo];
            else
                Retorno = String.Empty;

            return Retorno;
        }

        /// <summary>
        /// Metodo fechar onde é finalizado os objetos decladado nesta classe
        /// </summary>
        public void Close()
        {
            if (this.Registro != null)
                this.Registro.Close();
            if (this.CommandSql != null)
                this.CommandSql = null;
            if (this.ConnectionDb != null)
                this.ConnectionDb.Dispose();
        }

        /// <summary>
        /// Dispensa o uso dos objetos desta classe liberando a memoria do processado para outros fins
        /// </summary>
        public void Dispose()
        {
            Close();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finaliza objetos que por ventura fiquem aberto, instanciados na memoria
        /// </summary>
        ~ConnectionSqlServer()
        {
            Close();
        }
    }
}