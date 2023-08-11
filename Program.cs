using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Threading;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace SpotifyAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            EdgeOptions options = new EdgeOptions();
            IWebDriver driver = new EdgeDriver(options);

            driver.Manage().Window.Maximize();

            driver.Navigate().GoToUrl("https://www.spotify.com");

            // Definir rutas absolutas para las capturas y el informe
            string capturasPath = @"C:\Users\adalb\source\repos\Pruebas Automatizadas\Capturas";
            string reportePath = @"C:\Users\adalb\source\repos\Pruebas Automatizadas\Reporte\informe_pruebas.html";

            // Configurar el reporte
            var htmlReporter = new ExtentHtmlReporter(reportePath);
            var extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);

            // Crear un nuevo informe de prueba
            var test = extent.CreateTest("Prueba automatizada de Spotify", "Ejecución de pruebas automatizadas en Spotify");

            PerformLogin(driver, test, capturasPath);
            SearchSong(driver, "Self Care", test, capturasPath);
            PlaySong(driver, test, capturasPath);
            Thread.Sleep(TimeSpan.FromSeconds(5));
            PauseSong(driver, test, capturasPath);
            Thread.Sleep(TimeSpan.FromSeconds(5));
            SkipNextSong(driver, test, capturasPath);
            Thread.Sleep(TimeSpan.FromSeconds(5));
            SkipPreviousSong(driver, test, capturasPath);
            PerformLogout(driver, test, capturasPath);

            // Cerrar el navegador y finalizar el informe
            driver.Quit();
            extent.Flush();
        }

        static void PerformLogin(IWebDriver driver, ExtentTest test, string capturasPath)
        {
            try
            {
                Screenshot screenshotButtonVisible = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotButtonVisiblePath = System.IO.Path.Combine(capturasPath, "screenshotButtonVisible.png");
                screenshotButtonVisible.SaveAsFile(screenshotButtonVisiblePath, ScreenshotImageFormat.Png);

                IWebElement iniciarSesionButton = driver.FindElement(By.XPath("//button[contains(@data-testid, 'login-button')]"));
                iniciarSesionButton.Click();

                Thread.Sleep(TimeSpan.FromSeconds(5));

                Screenshot screenshotFormVisible = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotFormVisiblePath = System.IO.Path.Combine(capturasPath, "screenshotFormVisible.png");
                screenshotFormVisible.SaveAsFile(screenshotFormVisiblePath, ScreenshotImageFormat.Png);

                IWebElement correoInput = driver.FindElement(By.Id("login-username"));

                correoInput.SendKeys("20220791@itla.edu.do");

                IWebElement contrasenaInput = driver.FindElement(By.Id("login-password"));

                contrasenaInput.SendKeys("1234567");

                IWebElement loginButton = driver.FindElement(By.Id("login-button"));
                loginButton.Click();

                Thread.Sleep(TimeSpan.FromSeconds(5));

                Screenshot screenshotLobbyIncorrect = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotLobbyIncorrectPath = System.IO.Path.Combine(capturasPath, "screenshotLobbyIncorrect.png");
                screenshotLobbyIncorrect.SaveAsFile(screenshotLobbyIncorrectPath, ScreenshotImageFormat.Png);

                contrasenaInput.Clear();

                contrasenaInput.SendKeys("pruebaautomatizada");

                loginButton = driver.FindElement(By.Id("login-button"));
                loginButton.Click();

                Thread.Sleep(TimeSpan.FromSeconds(5));

                Screenshot screenshotLobby = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotLobbyPath = System.IO.Path.Combine(capturasPath, "screenshotLobby.png");
                screenshotLobby.SaveAsFile(screenshotLobbyPath, ScreenshotImageFormat.Png);

                test.Pass("Inicio de sesión realizado correctamente");

            }

            catch (Exception ex)
            {
                test.Fail(ex);
            }



        }

        static void PerformLogout(IWebDriver driver, ExtentTest test, string capturasPath)
        {
            try
            {
                Screenshot screenshotIconProfile = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotIconProfilePath = System.IO.Path.Combine(capturasPath, "screenshotIconProfile.png");
                screenshotIconProfile.SaveAsFile(screenshotIconProfilePath, ScreenshotImageFormat.Png);

                IWebElement menuButton = driver.FindElement(By.CssSelector("button[data-testid='user-widget-link']"));
                menuButton.Click();

                Screenshot screenshotMenu = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotMenuPath = System.IO.Path.Combine(capturasPath, "screenshotMenu.png");
                screenshotMenu.SaveAsFile(screenshotMenuPath, ScreenshotImageFormat.Png);

                Thread.Sleep(TimeSpan.FromSeconds(2));

                IWebElement logoutButton = driver.FindElement(By.CssSelector("button[data-testid='user-widget-dropdown-logout']"));
                logoutButton.Click();

                Thread.Sleep(TimeSpan.FromSeconds(5));

                Screenshot screenshotLogout = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotLogoutPath = System.IO.Path.Combine(capturasPath, "screenshotLogout.png");
                screenshotLogout.SaveAsFile(screenshotLogoutPath, ScreenshotImageFormat.Png);

                test.Pass("Cierre de sesión realizado correctamente");

            }
            catch (Exception ex)
            {
                test.Fail(ex);
            }


        }

        static void SearchSong(IWebDriver driver, string songName, ExtentTest test, string capturasPath)
        {
            try
            {
                Screenshot screenshotSearch = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotSearchPath = System.IO.Path.Combine(capturasPath, "screenshotSearch.png");
                screenshotSearch.SaveAsFile(screenshotSearchPath, ScreenshotImageFormat.Png);

                IWebElement searchButton = driver.FindElement(By.CssSelector("a[aria-label='Buscar']"));
                searchButton.Click();

                Thread.Sleep(TimeSpan.FromSeconds(2));

                IWebElement searchInput = driver.FindElement(By.CssSelector("input[data-testid='search-input']"));

                searchInput.SendKeys(songName);

                Screenshot screenshotSearchInput = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotSearchInputPath = System.IO.Path.Combine(capturasPath, "screenshotSearchInput.png");
                screenshotSearchInput.SaveAsFile(screenshotSearchInputPath, ScreenshotImageFormat.Png);

                searchInput.SendKeys(Keys.Enter);

                Thread.Sleep(TimeSpan.FromSeconds(2));

                Screenshot screenshotSearchResult = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotSearchResultPath = System.IO.Path.Combine(capturasPath, "screenshotSearchResult.png");
                screenshotSearchResult.SaveAsFile(screenshotSearchResultPath, ScreenshotImageFormat.Png);

                Thread.Sleep(TimeSpan.FromSeconds(5));
                test.Pass("Búsqueda de canción realizada correctamente");

            }
            catch (Exception ex)
            {
                test.Fail(ex);
            }


        }

        static void PlaySong(IWebDriver driver, ExtentTest test, string capturasPath)
        {
            try
            {
                // Encontrar y hacer clic en el botón de reproducir
                IWebElement playButton = driver.FindElement(By.CssSelector("button[aria-label='Reproducir'][data-testid='control-button-playpause']"));
                playButton.Click();

                // Esperar 2 segundos
                Thread.Sleep(TimeSpan.FromSeconds(2));

                Screenshot screenshotPlay = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotPlayPath = System.IO.Path.Combine(capturasPath, "screenshotPlay.png");
                screenshotPlay.SaveAsFile(screenshotPlayPath, ScreenshotImageFormat.Png);

                test.Pass("Canción reproducida exitosamente");

            }
            catch (Exception ex)
            {
                test.Fail(ex);
            }


        }

        static void PauseSong(IWebDriver driver, ExtentTest test, string capturasPath)
        {
            try
            {
                // Encontrar y hacer clic en el botón de pausar
                IWebElement pauseButton = driver.FindElement(By.CssSelector("button[aria-label='Pausar']"));
                pauseButton.Click();

                Screenshot screenshotPause = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotPausePath = System.IO.Path.Combine(capturasPath, "screenshotPause.png");
                screenshotPause.SaveAsFile(screenshotPausePath, ScreenshotImageFormat.Png);

                test.Pass("Canción pausada exitosamente");

            }
            catch (Exception ex)
            {
                test.Fail(ex);
            }


        }

        static void SkipNextSong(IWebDriver driver, ExtentTest test, string capturasPath)
        {
            try
            {
                // Encontrar y hacer clic en el botón de siguiente canción
                IWebElement nextButton = driver.FindElement(By.CssSelector("button[data-testid='control-button-skip-forward']"));
                nextButton.Click();

                Screenshot screenshotSkipNextSong = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotSkipNextSongPath = System.IO.Path.Combine(capturasPath, "screenshotSkipNextSong.png");
                screenshotSkipNextSong.SaveAsFile(screenshotSkipNextSongPath, ScreenshotImageFormat.Png);


                test.Pass("Siguiente canción reproducida exitosamente");

            }
            catch (Exception ex)
            {
                test.Fail(ex);
            }


        }

        static void SkipPreviousSong(IWebDriver driver, ExtentTest test, string capturasPath)
        {
            try
            {
                // Encontrar y hacer clic en el botón de canción anterior
                IWebElement previousButton = driver.FindElement(By.CssSelector("button[data-testid='control-button-skip-back']"));
                previousButton.Click();
                previousButton.Click();

                Screenshot screenshotSkipPreviousSong = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotSkipPreviousSongPath = System.IO.Path.Combine(capturasPath, "screenshotSkipPreviousSong.png");
                screenshotSkipPreviousSong.SaveAsFile(screenshotSkipPreviousSongPath, ScreenshotImageFormat.Png);

                test.Pass("Canción anterior reproducida exitosamente");

            }
            catch (Exception ex)
            {
                test.Fail(ex);
            }


        }
    }
}
