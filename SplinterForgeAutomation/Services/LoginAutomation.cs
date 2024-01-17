using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

public class AccountLogin
{
    public bool LoginToHiveKeychain(string username, string password, string driverPath, string crxFilePath, string extensionId)
    {
        ChromeOptions options = new ChromeOptions();
        options.AddExtension(crxFilePath);
        IWebDriver driver = new ChromeDriver(driverPath, options);

        try
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Navigate().GoToUrl("chrome-extension://" + extensionId + "/popup.html");

            //driver.Navigate().GoToUrl("https://splinterforge.io/");

            IWebElement saveButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("button.submit-button.default > div.button-label")));


            IWebElement saveButton2 = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[@class='submit-button default' and descendant::div[@class='button-label' and text()='Save']]")));
            saveButton.Click();

            //Click on the login button
            IWebElement loginButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'Save')]")));
            loginButton.Click();

                

            // Fill in the login details
            IWebElement usernameField = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//input[@name='username']")));
            usernameField.SendKeys(username);

            IWebElement passwordField = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//input[@name='password']")));
            passwordField.SendKeys(password);

            IWebElement submitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(),'Submit')]")));
            submitButton.Click();

            // Additional actions after login



            Console.WriteLine("Logged in to Hive Keychain successfully!");  
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during Hive Keychain login: {ex.Message}");
            return false;
        }
        finally
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }
    }
}
