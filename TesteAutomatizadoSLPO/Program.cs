using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TesteAutomatizadoSLPO
{
    class Program
    {
        static ChromeDriver driver;
        static void Main(string[] args)
        {
            driver = new ChromeDriver();

            driver.Navigate().GoToUrl("https://www.facebook.com/");

            IWebElement campoLogin = driver.FindElement(By.Name("email"));
            IWebElement campoSenha = driver.FindElement(By.Name("pass"));

            campoLogin.SendKeys("nelsontecti@gmail.com");
            campoSenha.SendKeys("Bankai09");

            campoLogin.Submit();

            driver.Close();
            driver.Dispose();
        }
    }
}
