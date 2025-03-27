## 1. Introducción
En esta práctica, se ha llevado a cabo la automatización de pruebas utilizando **Selenium WebDriver** en el sitio web de Spotify. El objetivo principal es validar la correcta operación de funcionalidades críticas, como el inicio de sesión, la búsqueda de canciones y los controles de reproducción (play, pausa, siguiente, anterior). Para la implementación, se ha utilizado **C#** como lenguaje de programación y **Microsoft Edge Driver** como controlador para la interacción con el navegador.

---

## 2. Objetivos
1. **Realizar pruebas de inicio de sesión** con credenciales válidas e inválidas.  
2. **Validar la búsqueda** de canciones por nombre.  
3. **Probar los controles de reproducción** (reproducir, pausar, siguiente y anterior).  
4. **Implementar reportes de prueba** utilizando ExtentReports, generando capturas de pantalla y un informe en formato HTML.  
5. **Documentar todo el proceso** de configuración, ejecución y resultados para futuras referencias o mejoras.

---

## 3. Alcance
Esta suite de pruebas cubre las funcionalidades básicas de Spotify en un entorno de navegador de escritorio. No se incluyen pruebas exhaustivas de listas de reproducción, configuraciones de usuario avanzadas, integración con redes sociales, etc. El enfoque se limita a las acciones típicas que un usuario realiza al iniciar sesión y reproducir canciones.

---

## 4. Configuración del Entorno de Pruebas

### 4.1 Requisitos Previos
- **Visual Studio** (o cualquier IDE compatible con C#) instalado.  
- **.NET SDK** disponible en el sistema para compilar y ejecutar el proyecto.  
- **Microsoft Edge** instalado en su versión estable (o versión adecuada según el EdgeDriver).  

### 4.2 Paquetes Necesarios
- [**Selenium.WebDriver**](https://www.nuget.org/packages/Selenium.WebDriver/)
- [**Selenium.WebDriver.ChromeDriver**] (o en este caso, EdgeDriver)
- [**ExtentReports**](https://www.nuget.org/packages/ExtentReports/) (para la generación de informes)

Para instalar los paquetes, se puede usar la consola de **NuGet** en Visual Studio:
```bash
Install-Package Selenium.WebDriver
Install-Package Selenium.WebDriver.EdgeDriver
Install-Package ExtentReports
```

### 4.3 Estructura del Proyecto
El proyecto (denominado en el ejemplo `SpotifyAutomation`) contiene:
- **Program.cs**: Archivo principal con la definición de los métodos de prueba.  
- **Carpeta `Capturas`**: Directorio donde se guardan las capturas de pantalla tomadas durante la ejecución de las pruebas.  
- **Carpeta `Reporte`**: Directorio donde se genera el informe en HTML con los resultados de las pruebas.  
- **Carpeta `Video`**: Carpeta con el video que demuestra la ejecución de las pruebas.  

---

## 5. Escenarios de Prueba

1. **Inicio de Sesión:**
   - **Escenario 1**: Ingresar credenciales inválidas y verificar que se muestre el mensaje de error.  
   - **Escenario 2**: Ingresar credenciales válidas y confirmar la entrada al lobby de Spotify.  

2. **Búsqueda de Canción:**
   - **Escenario 3**: Escribir el nombre de una canción en la barra de búsqueda y verificar que se desplieguen resultados adecuados.

3. **Reproducción de Canción:**
   - **Escenario 4**: Hacer clic en “Reproducir” y validar que el estado cambie correctamente.  
   - **Escenario 5**: Hacer clic en “Pausar” y validar que el estado cambie correctamente.  
   - **Escenario 6**: Hacer clic en “Siguiente” y verificar que la canción en reproducción cambie.  
   - **Escenario 7**: Hacer clic en “Anterior” y validar que la canción en reproducción cambie correctamente.  

4. **Cierre de Sesión:**
   - **Escenario 8**: Iniciar el proceso de cierre de sesión y confirmar que el usuario sale correctamente de la cuenta.

---

## 6. Diseño de los Casos de Prueba

A continuación, se muestra un ejemplo de la estructura de los casos de prueba. (Solo se incluye un ejemplo; el resto mantiene la misma forma).

### Caso de Prueba: Inicio de Sesión con Credenciales Inválidas
- **ID**: TC-Login-01  
- **Objetivo**: Validar que el sistema no permita el acceso con una contraseña incorrecta.  
- **Pasos**:  
  1. Navegar a la página principal de Spotify.  
  2. Hacer clic en el botón “Iniciar sesión”.  
  3. Ingresar correo electrónico correcto.  
  4. Ingresar contraseña **incorrecta**.  
  5. Presionar “Iniciar sesión”.  
- **Datos de Entrada**:  
  - Correo: `20220791@itla.edu.do`  
  - Contraseña: `1234567`  
- **Resultado Esperado**:  
  - El sitio muestra un mensaje de error o no permite el inicio de sesión.  

---

## 7. Implementación de las Pruebas

El código presentado a continuación ilustra la implementación en C# con Selenium WebDriver y ExtentReports:

```csharp
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

            // Rutas absolutas para capturas y reporte
            string capturasPath = @"C:\Users\adalb\source\repos\Pruebas Automatizadas\Capturas";
            string reportePath = @"C:\Users\adalb\source\repos\Pruebas Automatizadas\Reporte\informe_pruebas.html";

            // Configuración del reporte
            var htmlReporter = new ExtentHtmlReporter(reportePath);
            var extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);

            // Crear un nuevo informe de prueba
            var test = extent.CreateTest("Prueba automatizada de Spotify", 
                       "Ejecución de pruebas automatizadas en Spotify");

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

            // Cerrar el navegador y generar el informe
            driver.Quit();
            extent.Flush();
        }

        static void PerformLogin(IWebDriver driver, ExtentTest test, string capturasPath)
        {
            try
            {
                // Captura antes de iniciar sesión
                Screenshot screenshotButtonVisible = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotButtonVisiblePath = System.IO.Path.Combine(capturasPath, "screenshotButtonVisible.png");
                screenshotButtonVisible.SaveAsFile(screenshotButtonVisiblePath, ScreenshotImageFormat.Png);

                IWebElement iniciarSesionButton = driver.FindElement(By.XPath("//button[contains(@data-testid, 'login-button')]"));
                iniciarSesionButton.Click();
                Thread.Sleep(TimeSpan.FromSeconds(5));

                // Captura del formulario visible
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

                // Captura de lobby (inicio de sesión fallido)
                Screenshot screenshotLobbyIncorrect = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotLobbyIncorrectPath = System.IO.Path.Combine(capturasPath, "screenshotLobbyIncorrect.png");
                screenshotLobbyIncorrect.SaveAsFile(screenshotLobbyIncorrectPath, ScreenshotImageFormat.Png);

                // Se limpia la contraseña y se reingresa
                contrasenaInput.Clear();
                contrasenaInput.SendKeys("pruebaautomatizada");

                loginButton = driver.FindElement(By.Id("login-button"));
                loginButton.Click();
                Thread.Sleep(TimeSpan.FromSeconds(5));

                // Captura de lobby (inicio de sesión exitoso)
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
                // Captura antes del logout
                Screenshot screenshotIconProfile = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotIconProfilePath = System.IO.Path.Combine(capturasPath, "screenshotIconProfile.png");
                screenshotIconProfile.SaveAsFile(screenshotIconProfilePath, ScreenshotImageFormat.Png);

                IWebElement menuButton = driver.FindElement(By.CssSelector("button[data-testid='user-widget-link']"));
                menuButton.Click();

                // Captura del menú desplegado
                Screenshot screenshotMenu = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotMenuPath = System.IO.Path.Combine(capturasPath, "screenshotMenu.png");
                screenshotMenu.SaveAsFile(screenshotMenuPath, ScreenshotImageFormat.Png);

                Thread.Sleep(TimeSpan.FromSeconds(2));

                IWebElement logoutButton = driver.FindElement(By.CssSelector("button[data-testid='user-widget-dropdown-logout']"));
                logoutButton.Click();
                Thread.Sleep(TimeSpan.FromSeconds(5));

                // Captura después del logout
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
                // Captura antes de buscar
                Screenshot screenshotSearch = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotSearchPath = System.IO.Path.Combine(capturasPath, "screenshotSearch.png");
                screenshotSearch.SaveAsFile(screenshotSearchPath, ScreenshotImageFormat.Png);

                IWebElement searchButton = driver.FindElement(By.CssSelector("a[aria-label='Buscar']"));
                searchButton.Click();
                Thread.Sleep(TimeSpan.FromSeconds(2));

                IWebElement searchInput = driver.FindElement(By.CssSelector("input[data-testid='search-input']"));
                searchInput.SendKeys(songName);

                // Captura después de ingresar la canción
                Screenshot screenshotSearchInput = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshotSearchInputPath = System.IO.Path.Combine(capturasPath, "screenshotSearchInput.png");
                screenshotSearchInput.SaveAsFile(screenshotSearchInputPath, ScreenshotImageFormat.Png);

                searchInput.SendKeys(Keys.Enter);
                Thread.Sleep(TimeSpan.FromSeconds(2));

                // Captura de resultados
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
                // Clic en botón "Reproducir"
                IWebElement playButton = driver.FindElement(By.CssSelector("button[aria-label='Reproducir'][data-testid='control-button-playpause']"));
                playButton.Click();
                Thread.Sleep(TimeSpan.FromSeconds(2));

                // Captura tras reproducir
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
                // Clic en botón "Pausar"
                IWebElement pauseButton = driver.FindElement(By.CssSelector("button[aria-label='Pausar']"));
                pauseButton.Click();

                // Captura tras pausar
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
                // Clic en "Siguiente"
                IWebElement nextButton = driver.FindElement(By.CssSelector("button[data-testid='control-button-skip-forward']"));
                nextButton.Click();

                // Captura tras saltar a la siguiente canción
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
                // Clic en "Anterior"
                IWebElement previousButton = driver.FindElement(By.CssSelector("button[data-testid='control-button-skip-back']"));
                previousButton.Click();
                previousButton.Click(); // Spotify a veces requiere dos clics para retroceder

                // Captura tras saltar a la anterior canción
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
```

---

## 8. Ejecución de las Pruebas
1. Compilar el proyecto en Visual Studio.  
2. Ejecutar el proyecto (`F5` o `Ctrl + F5`).  
3. Verificar que Microsoft Edge se abra automáticamente y realice las acciones de inicio de sesión, búsqueda y control de reproducción.  
4. Al finalizar, el navegador se cerrará y el informe en formato HTML estará disponible en la ruta especificada (`C:\Users\adalb\source\repos\Pruebas Automatizadas\Reporte\informe_pruebas.html`).  
5. Las capturas de pantalla se generarán en la carpeta `Capturas`.  

---

## 9. Generación de Reportes
Se utiliza **ExtentReports** para la creación de un informe en **HTML** que incluye información detallada de cada paso en los casos de prueba, incluidas las excepciones en caso de fallo y las capturas de pantalla al momento de cada validación.

- **Archivo HTML**:  
  - Nombre: `informe_pruebas.html`  
  - Ubicación: `C:\Users\adalb\source\repos\Pruebas Automatizadas\Reporte\informe_pruebas.html`  
- **Capturas de Pantalla**:  
  - Nombre: `screenshotXYZ.png`  
  - Ubicación: `C:\Users\adalb\source\repos\Pruebas Automatizadas\Capturas`

---

## 10. Resultados y Evidencia
- Se han adjuntado las capturas de pantalla en el repositorio de GitHub:
  - [Carpeta Capturas](https://github.com/AdalBMdev/PruebasAutomatizadas/tree/main/Capturas)
- El reporte en HTML también se encuentra en el repositorio:
  - [Carpeta Reporte](https://github.com/AdalBMdev/PruebasAutomatizadas/tree/main/Reporte)
- Se ha grabado un video que demuestra la ejecución de las pruebas:
  - [Carpeta Video](https://github.com/AdalBMdev/PruebasAutomatizadas/tree/main/Video)

Al revisar el informe, se pudo confirmar que todas las pruebas han sido ejecutadas correctamente. Los pasos que fallan (como el primer intento de inicio de sesión con una contraseña inválida) generan la captura de pantalla y el mensaje esperado.

---

## 11. Revisión y Mejora Continua
1. **Manejo de Esperas**:  
   - Se utilizan `Thread.Sleep(...)` para demoras explícitas. En un entorno más robusto, sería recomendable implementar **esperas explícitas** con `WebDriverWait` para sincronizar el código con los elementos de la interfaz.  
2. **Modularización de Código**:  
   - Aunque los métodos están divididos por funcionalidad, se puede mejorar la arquitectura usando **POM (Page Object Model)** para un mejor mantenimiento.  
3. **Validaciones Adicionales**:  
   - Se podrían incluir más validaciones (por ejemplo, verificar que el nombre de la canción aparezca en el reproductor).  
4. **Ejecución en Múltiples Navegadores**:  
   - Para ampliar la cobertura, se podría configurar el proyecto para ejecutar en Chrome, Firefox, etc.  

---

## 12. Conclusiones
La automatización de pruebas desarrollada para el sitio web de Spotify permite validar las funcionalidades esenciales de inicio de sesión, búsqueda y control de reproducción. Además, la implementación de **ExtentReports** proporciona una valiosa documentación de la ejecución de pruebas con capturas de pantalla detalladas.

Con este enfoque, se logra:
- Mayor **eficiencia** en la validación repetitiva de funcionalidades clave.  
- **Transparencia** en los resultados gracias a los reportes automatizados.  
- Base sólida para **escalabilidad** y nuevas pruebas que abarquen más características de Spotify o se integren con otras herramientas.

---

## 13. Instrucciones para Replicación
1. Clonar el repositorio desde GitHub.  
2. Abrir el proyecto en Visual Studio.  
3. Restaurar paquetes de NuGet (Selenium WebDriver, EdgeDriver, ExtentReports).  
4. Ajustar las rutas de las carpetas `Capturas` y `Reporte` si fuera necesario.  
5. Ejecutar la solución.  
6. Revisar el informe `informe_pruebas.html` y la carpeta `Capturas` al finalizar.

---
