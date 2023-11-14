using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;

public class DiscountTrackingService : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DiscountTrackingService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                string pythonScriptPath = "scraping.py"; // Replace with the correct path to your "scraping.py" script

                PythonScriptExecutor.ExecutePythonScript(pythonScriptPath);

                Console.WriteLine("Python script executed.");
                for (int i = 0; i < 3; i++){
                    string sourceFilePath = "./screenshot" + i + ".png";
                    string destinationFilePath = "./wwwroot/images/screenshot"+ i +".png";
                    try
                    {
                        // Use File.Move to move the file
                        File.Copy(sourceFilePath, destinationFilePath, true);

                        Console.WriteLine("File moved successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }

                }

                // Adjust the delay interval
                await Task.Delay(TimeSpan.FromHours(30), stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

}
