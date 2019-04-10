using System;
using TesteAutomatizadoSLPO.Controllers;

namespace TesteAutomatizadoSLPO
{
    class Program
    {

        static void Main(string[] args)
        {
            Startup.Engage(args);
        }

        static class Startup
        {
            static public void Engage(string[] args)
            {
                //Console.WriteLine("BICP - BOT - VERSÃO: " + System.Configuration.ConfigurationManager.AppSettings["VersaoSistema"].ToString());
                new BicpBotController(args);
            }
        }
    }
}
