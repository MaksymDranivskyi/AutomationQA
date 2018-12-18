using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Linq;
using System.Threading;
using System.IO;
using System;


namespace HomeworkAdvancedSelenium2
{
    public class Homework
    {
        IWebDriver driver;
        IJavaScriptExecutor jexec;
        [Test]
        [Order(1)]

        public void ScrollAndDownload()
        {
            var pathcHrome = Path.Combine("E:\\", "ChromeDirectory", DateTime.Now.ToString("yy-MM-dd HH-mm-ss"));
            var options = new ChromeOptions();
            options.AddArguments("--start-fullscreen");
            options.AddUserProfilePreference("download.default_directory", pathcHrome);

            var driver = new ChromeDriver(options);
            jexec = (IJavaScriptExecutor)driver;
            driver.Url = "https://unsplash.com/search/photos/test";
            driver.Manage().Window.Maximize();

           
            while (true)
            {
                var prevScrollY = (long)jexec.ExecuteScript("return window.scrollY");

                jexec.ExecuteScript("window.scrollBy(0, 300)");
                Thread.Sleep(100);

                var ScrollY = (long)jexec.ExecuteScript("return window.scrollY");

                if (prevScrollY == ScrollY)
                    break;
            }

            var images = driver.FindElements(By.CssSelector("figure[itemprop = 'image']"));
            var biggestY = images.Max(x => x.Location.Y + x.Size.Height);
            var lowestImages = images.Where(x => x.Location.Y + x.Size.Height == biggestY).ToList();
            var biggestX = lowestImages.Max(x => x.Location.X);
            var mostRightImage = lowestImages.First(x => x.Location.X == biggestX);

            mostRightImage.Click();
            Thread.Sleep(200);
            var downloadButton = driver.FindElement(By.LinkText("Download free"));
            jexec.ExecuteScript($"arguments[0].click()", downloadButton);
            Thread.Sleep(5000);
            Assert.That(Directory.GetFiles(pathcHrome, "*.jpg"), Is.Not.Empty);
            driver.Quit();
           
        }



    }
}
