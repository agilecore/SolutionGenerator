using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Configuration;
using Gerador.Models;
using Gerador.Enums;
using Gerador.Infrastructure;

namespace Gerador
{
    public class Program
    {
        private const int MILLISECONDS = 1000;
        private static bool VERIFY_TIME = Convert.ToBoolean(ConfigurationManager.AppSettings["Temporizador"].ToString());
        public static EDataBase DatabaseType = EDataBase.SqlServer;

        static void Main(string[] args)
        {
            // Header...
            WriteHeader();
            if (VERIFY_TIME) { TimeSleep(MILLISECONDS); }

            // Ask...            
            WriteToConsole("Para gerar contexto completo digite:.............1");
            WriteToConsole("Para gerar somente camada de dados digite:.......2");
            WriteToConsole("Para gerar somente camada de domínio digite:.....3");
            WriteToConsole("Para gerar somente camada de frontend digite:....4");

            var AnswerQuestion = String.Empty;

            // Verify...
            do
            {
                AnswerQuestion = ReadFromConsole();
                if ((AnswerQuestion == "1") || (AnswerQuestion == "2") || (AnswerQuestion == "3") || (AnswerQuestion == "4")) { break; }

            } while ((AnswerQuestion != "1") || (AnswerQuestion != "2") || (AnswerQuestion != "3") || (AnswerQuestion == "4"));

            // Processing...
            MakeProcess(AnswerQuestion);
        }

        #region Métodos privados de geração

        internal static void MakeProcess(string AnswerQuestion)
        {
            Console.ForegroundColor = ConsoleColor.Gray;

            if (AnswerQuestion == "1")            
            {
                MakeData();
                MakeDomain();
                MakeController();
            }
            else if (AnswerQuestion == "2")
            {
                MakeData();
            }
            else if (AnswerQuestion == "3")
            {
                MakeDomain();
            }
            else if (AnswerQuestion == "4")
            {
                MakeController();
            }

            WriteToConsole(" ");
            WriteToConsole("Processo executado com sucesso!");
            ReadFromConsole();
        }

        internal static void MakeData()
        {
            MakeBase();
            MakeModels();
            MakeMappers();
            MakeContext();
            MakeRepository();
            MakeIRepository();
            MakeUnitOfWork();
        }

        internal static void MakeDomain()
        {
            var ConfigTable = new TableToClass();
            var BuildClass = new Domain();
            var GroupTables = ConfigTable.GetTableMapper();

            WriteToConsole(" ");
            WriteToConsole("Gerando Domain Models...");

            foreach (var Table in GroupTables)
            {
                WriteToConsole("-> " + BuildClass.BuildModels(Table));
                if (VERIFY_TIME) { TimeSleep(MILLISECONDS); }

                WriteToConsole("-> " + BuildClass.BuildModelsSpecialized(Table.Value.ClassName));
                if (VERIFY_TIME) { TimeSleep(MILLISECONDS); }

                //WriteToConsole("-> " + BuildClass.BuildModelsValidations(Table));
                //if (VERIFY_TIME) { TimeSleep(MILLISECONDS); }
            }

        }

        internal static void MakeController()
        {
            var ConfigTable = new TableToClass();
            var BuildClass = new Presentation();
            var GroupTables = ConfigTable.GetTableMapper();

            

            WriteToConsole(" ");
            WriteToConsole("Gerando Controllers...");

            WriteToConsole("-> " + BuildClass.BuildBase());
            if (VERIFY_TIME) { TimeSleep(MILLISECONDS); }

            foreach (var Table in GroupTables)
            {
                //CreateController = true entao gera controller na camada de apresentacao.
                if (Table.Value.CreateController)
                {
                    WriteToConsole("-> " + BuildClass.BuildController(Table.Value.ClassName));
                    if (VERIFY_TIME) { TimeSleep(MILLISECONDS); }
                }  
            }
        }

        internal static void MakeBase()
        {
            var BuildClass = new Data();

            WriteToConsole(" ");
            WriteToConsole("Gerando BaseModel...");
            WriteToConsole("-> " + BuildClass.BuildBase());

            if (VERIFY_TIME) { TimeSleep(MILLISECONDS); }
        }

        internal static void MakeModels()
        {
            var ConfigTable = new TableToClass();
            var BuildClass = new Data();
            var GroupTables = ConfigTable.GetTableMapper();

            WriteToConsole(" ");
            WriteToConsole("Gerando Models...");

            foreach (var Table in GroupTables)
            {
                WriteToConsole("-> " + BuildClass.BuildModels(Table));
                if (VERIFY_TIME) { TimeSleep(MILLISECONDS); }
            }
        }

        internal static void MakeMappers()
        {
            var ConfigTable = new TableToClass();
            var BuildClass = new Data();
            var GroupTables = ConfigTable.GetTableMapper();

            WriteToConsole(" ");
            WriteToConsole("Gerando Mappers...");

            foreach (var Table in GroupTables)
            {
                WriteToConsole("-> " + BuildClass.BuildMapper(Table));
                if (VERIFY_TIME) { TimeSleep(MILLISECONDS); }
            }
        }

        internal static void MakeContext()
        {
            var ConfigTable = new TableToClass();
            var BuildClass = new Data();
            var GroupTables = ConfigTable.GetTableMapper();

            WriteToConsole(" ");
            WriteToConsole("Gerando Contexto...");

            WriteToConsole("-> " + BuildClass.BuildContext(GroupTables));
            if (VERIFY_TIME) { TimeSleep(MILLISECONDS); }
        }

        internal static void MakeRepository()
        {
            var BuildClass = new Data();

            WriteToConsole(" ");
            WriteToConsole("Gerando Repositorio...");

            WriteToConsole("-> " + BuildClass.BuildRespository());
            if (VERIFY_TIME) { TimeSleep(MILLISECONDS); }
        }

        internal static void MakeIRepository()
        {
            var BuildClass = new Data();

            WriteToConsole(" ");
            WriteToConsole("Gerando IRepositorio...");

            WriteToConsole("-> " + BuildClass.BuildIRespository());
            if (VERIFY_TIME) { TimeSleep(MILLISECONDS); }
        }

        internal static void MakeUnitOfWork()
        {
            var ConfigTable = new TableToClass();
            var BuildClass = new Data();
            var GroupTables = ConfigTable.GetTableMapper();

            WriteToConsole(" ");
            WriteToConsole("Gerando UnitOfWork...");

            WriteToConsole("-> " + BuildClass.BuildUnitOfWork(GroupTables));
            if (VERIFY_TIME) { TimeSleep(MILLISECONDS); }
        }

        #endregion

        #region Métodos do shellprompt

        private static void TimeSleep(int Milliseconds)
        {
            var StopWatch = Stopwatch.StartNew();
            Thread.Sleep(Milliseconds);
            StopWatch.Stop();
        }

        private static void WriteHeader()
        {
            Console.Title = typeof(Program).Name;
            Console.ForegroundColor = ConsoleColor.Green;

            var ConsoleCopyRight = new StringBuilder();

            Console.WriteLine("#***********************************************************");
            Console.WriteLine("# Projeto: Gerador.v1                                       ");
            Console.WriteLine("# Console: Gerador de Camadas para Aplicações Web           ");
            Console.WriteLine("# Empresa: Agilecore                                        ");
            Console.WriteLine("#***********************************************************");

            WriteToConsole(ConsoleCopyRight.ToString());
        }

        private static ConsoleKeyInfo ReadKeyPress()
        {
            return (Console.ReadKey());
        }

        private static string ReadFromConsole()
        {
            return (Console.ReadLine());
        }

        private static void WriteToConsole(string message)
        {
            if (message.Length > 0)
            {
                Console.WriteLine("prompt: " + message);
            }
        }

        private static void WriteEnterLine()
        {
            Console.WriteLine(String.Empty);
            Console.ReadLine();
        }

        private static string Execute(string command)
        {
            return string.Format("Comando {0} executado.", command);
        }

        #endregion
    }
}