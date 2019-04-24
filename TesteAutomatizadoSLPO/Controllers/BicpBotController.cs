using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.Events;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System;
//using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Imaging;
using OpenQA.Selenium.IE;

namespace TesteAutomatizadoSLPO.Controllers
{
    class BicpBotController
    {
        //private string BICP_HOME = "10.64.0.152:8080";
        private string WORK_PATH;
        private Decaptcha WORK_LOG = new Decaptcha();
        static ChromeDriver driver;
        //static InternetExplorerDriver driver;
        //static FirefoxDriver driverF;

        //public string screenshotsPasta = @"C:\Users\nmartin\Desktop\Selenium\";
        //int contador;

        static string captcha_plain;
       
        public BicpBotController(string[] args)
        {
            //driver = new InternetExplorerDriver();
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://10.64.0.152:8080/bicp/login.do?forward=loginPage");
            driver.Manage().Window.Maximize();

            IWebElement campoLogin = driver.FindElement(By.Id("userid"));
            IWebElement campoSenha = driver.FindElement(By.Id("pwd"));
            //  id da do captcha a ser quebrado e recortado
            IWebElement campoiMG = driver.FindElement(By.Id("rdImg"));
            IWebElement campoCaptcha = driver.FindElement(By.Id("randImg"));
            IWebElement BtnSubmit = driver.FindElement(By.Id("logBtn"));
            this.CreateWorkDir();

            #region Capiturar a imagem da tela e tratar o captcha

            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(this.WORK_PATH + "\\ImgParacaptcha.png", ScreenshotImageFormat.Png);

            Image img = Bitmap.FromFile(this.WORK_PATH + "\\ImgParacaptcha.png");
            Rectangle rect = new Rectangle();

            if (campoiMG != null)
            {
                int width = campoiMG.Size.Width;
                int height = campoiMG.Size.Height;

                Point p = campoiMG.Location;

                rect = new Rectangle(p.X, p.Y, width, height);
            }

            Bitmap bmpImage = new Bitmap(img);
            var cropedImag = bmpImage.Clone(rect, bmpImage.PixelFormat);
            cropedImag.Save(this.WORK_PATH + "\\captcha.png");

            #endregion

            string captcha_plain = this.DecodeCaptcha();

            campoLogin.SendKeys("BRASILCENTER");
            campoSenha.SendKeys("Center12");
            campoCaptcha.SendKeys(captcha_plain);

            int j = 1;

            BtnSubmit.Submit();

            IWebElement btnSair = driver.FindElement(By.Id("logOutId"));
            //IWebElement btnSair = driver.FindElement(By.Id("helpId"));

            btnSair.Click();

            driver.Close();
            driver.Dispose();
        }

        private string DecodeCaptcha()
        {
            string captcha_plain = "-1";

            Decaptcha CapMap = new Decaptcha();
            int threshold = 450;

            //Bitmap img = new Bitmap(@"C:\Temp\Nelson\RobsmacImg.png");
            Bitmap img = new Bitmap(this.WORK_PATH + "\\captcha.png");            
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
            Decaptcha char_0 = new Decaptcha(); for (int x = 0; x < 20; x++) for (int y = 0; y < 15; y++) if ((int)CapMap.Get(x, y) == 1) char_0.Set(null, 1); else char_0.Set(null, 0); // SPLIT, 1º NUMERO
            Decaptcha char_1 = new Decaptcha(); for (int x = 0; x < 20; x++) for (int y = 15; y < 30; y++) if ((int)CapMap.Get(x, y) == 1) char_1.Set(null, 1); else char_1.Set(null, 0); // SPLIT, 2º NUMERO
            Decaptcha char_2 = new Decaptcha(); for (int x = 0; x < 20; x++) for (int y = 30; y < 45; y++) if ((int)CapMap.Get(x, y) == 1) char_2.Set(null, 1); else char_2.Set(null, 0); // SPLIT, 3º NUMERO						
            Decaptcha char_3 = new Decaptcha(); for (int x = 0; x < 20; x++) for (int y = 45; y < 60; y++) if ((int)CapMap.Get(x, y) == 1) char_3.Set(null, 1); else char_3.Set(null, 0); // SPLIT, 4º NUMERO

            // Array com o modelo de cada numero em preto e branco
            Decaptcha model_0 = new Decaptcha(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Decaptcha model_1 = new Decaptcha(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Decaptcha model_2 = new Decaptcha(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Decaptcha model_3 = new Decaptcha(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Decaptcha model_4 = new Decaptcha(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Decaptcha model_5 = new Decaptcha(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Decaptcha model_6 = new Decaptcha(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Decaptcha model_7 = new Decaptcha(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Decaptcha model_8 = new Decaptcha(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Decaptcha model_9 = new Decaptcha(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            // para armazenar a decodificação do captcha temporariamente
            Decaptcha TXT_CAPTCHA = new Decaptcha(-1, -1, -1, -1);

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

        private void CreateWorkDir()
        {
            string guid = Guid.NewGuid().ToString("N").ToUpper();
            this.WORK_PATH = "C:\\Temp\\BICP_Selenium_" + guid;
            System.IO.Directory.CreateDirectory(this.WORK_PATH);
            this.WORK_LOG.Set(null, this.WORK_PATH);

        }        
    }
}
