using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.Events;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Imaging;

namespace TesteAutomatizadoSLPO.Controllers
{
    class BicpBotController
    {
        private string BICP_HOME = "10.64.0.152:8080";
        private string WORK_PATH;
        private Verse WORK_LOG = new Verse();
        static ChromeDriver driver;
        static FirefoxDriver driverF;

        public string screenshotsPasta = @"C:\Users\nmartin\Desktop\Selenium\";
        int contador;

        static string captcha_plain;
        //private Boolean Captcha()
        //{
        //    // Decodificando o captcha
        //    this.RunCurl("validateCode.do?action=createValidateCode", null, this.WORK_PATH + "\\captcha.png");
        //    string captcha_plain = this.DecodeCaptcha();
        //    return true;
        //}
        public BicpBotController(string[] args)
        {
            /*
            string nomeUnico = "D:\\sample.png"; // nome do arqquivo salvo na maquina
            string arquivoNome = "D:\\sample.png";

            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://10.64.0.152:8080/bicp/login.do?forward=loginPage");

            IWebElement element = driver.FindElement(By.Id("rdImg"));

            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(arquivoNome, ScreenshotImageFormat.Png);


            Image img = Bitmap.FromFile(nomeUnico);
            Rectangle rect = new Rectangle();

            if (element != null)
            {
                int width = element.Size.Width;
                int height = element.Size.Height;

                Point p = element.Location;

                rect = new Rectangle(p.X, p.Y, width, height);
            }

            Bitmap bmpImage = new Bitmap(img);
            var cropedImag = bmpImage.Clone(rect, bmpImage.PixelFormat);
            cropedImag.Save("D:\\nelsonteste.png");


            Console.ReadKey();    
             */

            driver = new ChromeDriver();
            //driverF = new FirefoxDriver();
            driver.Navigate().GoToUrl("http://10.64.0.152:8080/bicp/login.do?forward=loginPage");
            //driver.Navigate().GoToUrl("http://only-testing-blog.blogspot.com/2014/09/selectable.html");
            driver.Manage().Window.Maximize();

            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile("C:\\Users\\nmartin\\Desktop\\Selenium\\Exemplo.png", 0);

            //Thread.Sleep(10000);
            /// Teste
            //IWebElement logo = driver.FindElement(By.ClassName(" forum-logo"));
            IWebElement logo = driver.FindElement(By.Id("rdImg"));

            //int width = logo.Size.Width;
            //int height = logo.Size.Height;


            /*
              public Image CaptureElementScreenShot(HTMLElement element, string uniqueName)
              {
              */
                  //Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                  //screenshot.SaveAsFile(filename, System.Drawing.Imaging.ImageFormat.Jpeg);

                  //Image img = Bitmap.FromFile(uniqueName);
                  Rectangle rect = new Rectangle();

                  if (logo != null)
                  {
                      // Obtenha a largura e a altura do WebElement usando
                      int width = logo.Size.Width;
                      int height = logo.Size.Height;

                      // Obtenha o local do WebElement em um ponto.
                      // Isso fornecerá coordenadas X & Y do WebElement
                      Point p = logo.Location;

                      // Crie um retângulo usando a largura, altura e localização do elemento
                      rect = new Rectangle(p.X, p.Y, width, height);
                  }

                  // Cortar a imagem com base no rect. //rectangle
                  Bitmap bmpImage = new Bitmap(@"C:\Users\nmartin\Desktop\Selenium\Exemplo.png");
                  //Bitmap img = new Bitmap(@"C:\Temp\Nelson\RobsmacImg.png");
                    var cropedImag = bmpImage.Clone(rect, bmpImage.PixelFormat);
            cropedImag.Save(@"C:\Temp\Nelson\RobsmacImg.png");
                    //cropedImag
                    var t = 1;
            /*
                  return cropedImag; //Imagem recortada
              }
              */

            /*
            //Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            //screenshot.SaveAsFile(filename, System.Drawing.Imaging.ImageFormat.Jpeg);

            //Image img = Bitmap.FromFile(uniqueName);
            //Rectangle rect = new Rectangle();

            //if (element != null)
            //{
            //    // Obtenha a largura e a altura do WebElement usando
            //    int width = element.Size.Width;
            //    int height = element.Size.Height;

            //    // Obtenha o local do WebElement em um ponto.     
            //    // Isso fornecerá coordenadas X & Y do WebElement
            //    Point p = element.Location;

            //    // Crie um retângulo usando a largura, altura e localização do elemento
            //    rect = new Rectangle(p.X, p.Y, width, height);
            //}

            //Bitmap bmpImage = new Bitmap(img);
            //var cropedImag = bmpImage.Clone(rect, bmpImage.PixelFormat);
            /// Teste
            */

            String logoSRC = logo.GetAttribute("src");

            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData(logoSRC);
            MemoryStream ms = new MemoryStream(bytes);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

            /*
            //URL imageURL = new Url(logoSRC);
            WebRequest imageURL = WebRequest.Create(logoSRC);
            //            imageURL.RequestUri.UserInfo
            var robs = imageURL.RequestUri.AbsoluteUri.ToString();

            byte[] imgData = System.IO.File.ReadAllBytes(robs);

            Image i = Image.FromFile(logoSRC); // FromStream(logoSRC);

            Bitmap j = new Bitmap(logoSRC);

            i.Save(@"C:\Temp\Nelson\RobsmacImg");
            */

            WebClient webClient = new WebClient();
            webClient.DownloadFile(logoSRC, @"C:\Temp\Nelson\RobsmacImg.png");

            IWebElement campoLogin = driver.FindElement(By.Id("userid"));
            IWebElement campoSenha = driver.FindElement(By.Id("pwd"));
            IWebElement campoiMG = driver.FindElement(By.Id("randImg"));
            IWebElement BtnSubmit = driver.FindElement(By.Id("logBtn"));
            this.CreateWorkDir();
            //this.RunCurl("login.do?forward=loginPage");
            //this.RunCurl("validateCode.do?action=createValidateCode", null, this.WORK_PATH + "\\captcha.png");
            string captcha_plain = this.DecodeCaptcha();

            campoLogin.SendKeys("BRASILCENTER");
            campoSenha.SendKeys("Center12");
            campoiMG.SendKeys(captcha_plain);

            BtnSubmit.Submit();

            driver.Close();
            driver.Dispose();
        }

        private void RunCurl(string get, Verse post = null, string outfile = null)
        {
            try
            {
                string AppPath = System.Reflection.Assembly.GetExecutingAssembly().Location; // Local do BicpBot.exe
                string AppFolder = System.IO.Path.GetDirectoryName(AppPath); // Diretorio do BicpBot.exe

                string curl_binary = "\"" + AppFolder + "\\curl.exe\"";
                //string curl_silence = "-s";
                string curl_agente = "-A \"Mozilla/5.0 (Windows NT 6.1; Trident/7.0; rv:11.0) like Gecko\"";
                string curl_cookie = "-b cookie -c cookie";
                string curl_get = "http://" + this.BICP_HOME + "/bicp/" + get;
                string curl_post = "";
                string curl_outfile = "";

                if (post != null) // Resolvendo Post
                {
                    string guid = Guid.NewGuid().ToString("N").ToUpper();

                    string post_string = "";
                    foreach (KeyValuePair<dynamic, dynamic> Pair in post.Get())
                        post_string += Pair.Key + "=" + Uri.EscapeDataString(Pair.Value) + "&";

                    post_string = post_string.Remove(post_string.Length - 1); // removendo ultimo caractere

                    System.IO.File.WriteAllText(this.WORK_PATH + "\\" + guid + ".PostParams", post_string);

                    curl_post = "-d @" + this.WORK_PATH + "\\" + guid + ".PostParams";
                }

                if (outfile != null) // Resolvendo OutPost
                    curl_outfile = "-o " + outfile;

                string curl_command = "\"";
                curl_command += curl_binary + " ";
                //curl_command += curl_silence +" ";
                curl_command += curl_agente + " ";
                curl_command += curl_cookie + " ";
                curl_command += curl_outfile + " ";
                curl_command += curl_post + " ";
                curl_command += curl_get;
                curl_command += "\"";

                //Console.WriteLine();
                //Console.WriteLine("\n\r ######## -----------------------------######## ");
                Console.WriteLine("\n\r Curl: " + curl_get);
                //Console.WriteLine("\n\r ========== curl_command: " + curl_command);
                //Console.WriteLine("\n\r ####### /////////////////////////////// ######## ");
                this.RunCommand(curl_command);
            }
            catch (Exception e)
            {
                //log.Inserir("EXECUTANDO_CURL", "Erro: " + e.Message + "");
            }
        }

        private void RunCommand(string app)
        {
            this.WORK_LOG.Set(null, app);

            System.Diagnostics.Process Proc = new System.Diagnostics.Process();

            Proc.StartInfo.FileName = "cmd.exe";
            Proc.StartInfo.Arguments = "/C " + app;
            Proc.StartInfo.WorkingDirectory = this.WORK_PATH;
            Proc.StartInfo.UseShellExecute = false;
            Proc.StartInfo.RedirectStandardOutput = true;

            Proc.Start();

            string output = Proc.StandardOutput.ReadToEnd();

            //Console.WriteLine(output);

            this.WORK_LOG.Set(null, output);

            Proc.WaitForExit();
        }

        private void CreateWorkDir()
        {
            string guid = Guid.NewGuid().ToString("N").ToUpper();
            this.WORK_PATH = "C:\\Temp\\BICP_Ex_" + guid;
            System.IO.Directory.CreateDirectory(this.WORK_PATH);
            this.WORK_LOG.Set(null, this.WORK_PATH);

        }

        private string DecodeCaptcha()
        {
            string captcha_plain = "-1";

            Verse CapMap = new Verse();
            int threshold = 450;

            //Bitmap img = new Bitmap(this.WORK_PATH + "\\captcha.png");cropedImag
            Bitmap img = new Bitmap(@"C:\Temp\Nelson\RobsmacImg.png");
            for (int x = 0; x < img.Height; x++)
                for (int y = 0; y < img.Width; y++)
                {
                    Color Cl = img.GetPixel(y, x);

                    int PixelForce = Cl.R + Cl.G + Cl.B;

                    if (PixelForce < threshold)
                        CapMap.Set(x, y, 1);
                    else CapMap.Set(x, y, 0);
                }
            img.Dispose();

            // Printando o captcha
            string pre_log = "";
            foreach (KeyValuePair<dynamic, dynamic> X in CapMap.Get())
            {
                foreach (KeyValuePair<dynamic, dynamic> Y in X.Value.Get())
                    pre_log += Y.Value;
                pre_log += "\n";
            }
            this.WORK_LOG.Set(null, pre_log);

            // Numeros da matriz 2D para array unidimensionais		
            Verse char_0 = new Verse(); for (int x = 0; x < 20; x++) for (int y = 0; y < 15; y++) if ((int)CapMap.Get(x, y) == 1) char_0.Set(null, 1); else char_0.Set(null, 0); // SPLIT, 1º NUMERO
            Verse char_1 = new Verse(); for (int x = 0; x < 20; x++) for (int y = 15; y < 30; y++) if ((int)CapMap.Get(x, y) == 1) char_1.Set(null, 1); else char_1.Set(null, 0); // SPLIT, 2º NUMERO
            Verse char_2 = new Verse(); for (int x = 0; x < 20; x++) for (int y = 30; y < 45; y++) if ((int)CapMap.Get(x, y) == 1) char_2.Set(null, 1); else char_2.Set(null, 0); // SPLIT, 3º NUMERO						
            Verse char_3 = new Verse(); for (int x = 0; x < 20; x++) for (int y = 45; y < 60; y++) if ((int)CapMap.Get(x, y) == 1) char_3.Set(null, 1); else char_3.Set(null, 0); // SPLIT, 4º NUMERO

            // Array com o modelo de cada numero em preto e branco
            Verse model_0 = new Verse(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Verse model_1 = new Verse(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Verse model_2 = new Verse(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Verse model_3 = new Verse(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Verse model_4 = new Verse(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Verse model_5 = new Verse(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Verse model_6 = new Verse(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Verse model_7 = new Verse(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Verse model_8 = new Verse(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Verse model_9 = new Verse(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            // para armazenar a decodificação do captcha temporariamente
            Verse TXT_CAPTCHA = new Verse(-1, -1, -1, -1);

            int ok;

            // Comparando primeiro numero com os modelos

            ok = 1; for (int go = 0; go < 300; go++) if (char_0.Get(go) != model_0.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(0, 0);
            ok = 1; for (int go = 0; go < 300; go++) if (char_0.Get(go) != model_1.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(0, 1);
            ok = 1; for (int go = 0; go < 300; go++) if (char_0.Get(go) != model_2.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(0, 2);
            ok = 1; for (int go = 0; go < 300; go++) if (char_0.Get(go) != model_3.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(0, 3);
            ok = 1; for (int go = 0; go < 300; go++) if (char_0.Get(go) != model_4.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(0, 4);
            ok = 1; for (int go = 0; go < 300; go++) if (char_0.Get(go) != model_5.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(0, 5);
            ok = 1; for (int go = 0; go < 300; go++) if (char_0.Get(go) != model_6.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(0, 6);
            ok = 1; for (int go = 0; go < 300; go++) if (char_0.Get(go) != model_7.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(0, 7);
            ok = 1; for (int go = 0; go < 300; go++) if (char_0.Get(go) != model_8.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(0, 8);
            ok = 1; for (int go = 0; go < 300; go++) if (char_0.Get(go) != model_9.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(0, 9);

            // Comparando segundo numero com os modelos	
            ok = 1; for (int go = 0; go < 300; go++) if (char_1.Get(go) != model_0.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(1, 0);
            ok = 1; for (int go = 0; go < 300; go++) if (char_1.Get(go) != model_1.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(1, 1);
            ok = 1; for (int go = 0; go < 300; go++) if (char_1.Get(go) != model_2.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(1, 2);
            ok = 1; for (int go = 0; go < 300; go++) if (char_1.Get(go) != model_3.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(1, 3);
            ok = 1; for (int go = 0; go < 300; go++) if (char_1.Get(go) != model_4.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(1, 4);
            ok = 1; for (int go = 0; go < 300; go++) if (char_1.Get(go) != model_5.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(1, 5);
            ok = 1; for (int go = 0; go < 300; go++) if (char_1.Get(go) != model_6.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(1, 6);
            ok = 1; for (int go = 0; go < 300; go++) if (char_1.Get(go) != model_7.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(1, 7);
            ok = 1; for (int go = 0; go < 300; go++) if (char_1.Get(go) != model_8.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(1, 8);
            ok = 1; for (int go = 0; go < 300; go++) if (char_1.Get(go) != model_9.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(1, 9);

            // Comparando terceiro numero com os modelos		
            ok = 1; for (int go = 0; go < 300; go++) if (char_2.Get(go) != model_0.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(2, 0);
            ok = 1; for (int go = 0; go < 300; go++) if (char_2.Get(go) != model_1.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(2, 1);
            ok = 1; for (int go = 0; go < 300; go++) if (char_2.Get(go) != model_2.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(2, 2);
            ok = 1; for (int go = 0; go < 300; go++) if (char_2.Get(go) != model_3.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(2, 3);
            ok = 1; for (int go = 0; go < 300; go++) if (char_2.Get(go) != model_4.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(2, 4);
            ok = 1; for (int go = 0; go < 300; go++) if (char_2.Get(go) != model_5.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(2, 5);
            ok = 1; for (int go = 0; go < 300; go++) if (char_2.Get(go) != model_6.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(2, 6);
            ok = 1; for (int go = 0; go < 300; go++) if (char_2.Get(go) != model_7.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(2, 7);
            ok = 1; for (int go = 0; go < 300; go++) if (char_2.Get(go) != model_8.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(2, 8);
            ok = 1; for (int go = 0; go < 300; go++) if (char_2.Get(go) != model_9.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(2, 9);

            // Comparando quarto numero com os modelos	
            ok = 1; for (int go = 0; go < 300; go++) if (char_3.Get(go) != model_0.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(3, 0);
            ok = 1; for (int go = 0; go < 300; go++) if (char_3.Get(go) != model_1.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(3, 1);
            ok = 1; for (int go = 0; go < 300; go++) if (char_3.Get(go) != model_2.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(3, 2);
            ok = 1; for (int go = 0; go < 300; go++) if (char_3.Get(go) != model_3.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(3, 3);
            ok = 1; for (int go = 0; go < 300; go++) if (char_3.Get(go) != model_4.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(3, 4);
            ok = 1; for (int go = 0; go < 300; go++) if (char_3.Get(go) != model_5.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(3, 5);
            ok = 1; for (int go = 0; go < 300; go++) if (char_3.Get(go) != model_6.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(3, 6);
            ok = 1; for (int go = 0; go < 300; go++) if (char_3.Get(go) != model_7.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(3, 7);
            ok = 1; for (int go = 0; go < 300; go++) if (char_3.Get(go) != model_8.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(3, 8);
            ok = 1; for (int go = 0; go < 300; go++) if (char_3.Get(go) != model_9.Get(go)) ok = 0; if (ok == 1) TXT_CAPTCHA.Set(3, 9);


            if (TXT_CAPTCHA.Get(0) != -1)
                if (TXT_CAPTCHA.Get(1) != -1)
                    if (TXT_CAPTCHA.Get(2) != -1)
                        if (TXT_CAPTCHA.Get(3) != -1)
                            captcha_plain = Convert.ToString(TXT_CAPTCHA.Get(0)) + "" +
                                            Convert.ToString(TXT_CAPTCHA.Get(1)) + "" +
                                            Convert.ToString(TXT_CAPTCHA.Get(2)) + "" +
                                            Convert.ToString(TXT_CAPTCHA.Get(3));

            return captcha_plain;
        }
    }
}
