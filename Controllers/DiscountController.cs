using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

public class DiscountController : Controller
{
    public async Task<IActionResult> Index()
    {
        try
        {
            var url = "https://predajne.kaufland.sk/aktualna-ponuka/aktualny-tyzden/akciove-vyrobky.category=01_M%C3%A4so__hydina__%C3%BAdeniny.html";

            // Scrape the web page and get the modified HTML content
            var modifiedHtml = await ScrapeAndModifyHtmlAsync(url);
            url = "https://predajne.kaufland.sk/aktualna-ponuka/aktualny-tyzden/akciove-vyrobky.category=02_%C4%8Cerstv%C3%A9_ryby.html";

            var rybyHtml = await ScrapeAndModifyHtmlAsync(url);

            // var tescoHTML = await ScrapeAndModifyHtmlAsync("https://tesco.sk/akciove-ponuky/akciove-produkty/maso-ryby-a-udeniny/?page=0");
            // for (int i = 0 ; i< 1; i++){
            //     Console.WriteLine("this is tesco loop");
            //     var tescourl = "https://tesco.sk/akciove-ponuky/akciove-produkty/maso-ryby-a-udeniny/?page=" + i;
            //     Console.WriteLine(tescourl);
                
            //     Console.WriteLine(tescoHTML);
            // }
            Console.WriteLine("this is end of loop");
            
            var combinedHtml = modifiedHtml +  rybyHtml ;
            // Pass the modified HTML content to the view
            return View((object)combinedHtml);
        }
        catch (Exception ex)
        {
            // Handle any errors here
            return View("Error", (object)ex.Message);
        }
    }

    private async Task<string> ScrapeAndModifyHtmlAsync(string url)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("User-Agent", "Your User Agent String");

            var response = await client.GetStringAsync(url);

            // Parse the HTML content using HtmlAgilityPack
            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            var uri = new Uri(url);

            // Select elements with relative URLs (e.g., img, link, script, a)
            var elementsWithRelativeUrls = doc.DocumentNode.SelectNodes("//img|//link|//script|//a");
            
            if (elementsWithRelativeUrls != null)
            {
                foreach (var element in elementsWithRelativeUrls)
                {
                    // Get the attribute value (e.g., src, href) containing the relative URL
                    var attribute = element.GetAttributeValue("src", null) ?? element.GetAttributeValue("href", null);

                    if (!string.IsNullOrEmpty(attribute))
                    {
                        // Construct the absolute URL
                        var absoluteUrl = new Uri(uri, attribute).AbsoluteUri;

                        // Update the attribute with the absolute URL
                        element.SetAttributeValue("src", absoluteUrl);
                        element.SetAttributeValue("href", absoluteUrl);
                    }
                }
            }

            // var elementsToRemove = doc.DocumentNode.SelectNodes("//footer[contains(@class, 'page__footer')]");
            // var elementsToRemove = doc.DocumentNode.SelectNodes("//footer|//figure");
            var elementsToRemove = doc.DocumentNode.SelectNodes("//footer");
            if (elementsToRemove != null)
            {
                foreach (var element in elementsToRemove)
                {
                    element.Remove(); // Remove the selected element from the document
                }
            }
            Console.WriteLine("returning html");
            // Return the modified HTML content
            return doc.DocumentNode.OuterHtml;
        }
    }
}
