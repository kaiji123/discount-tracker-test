using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System;

public class FileUploadController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

[HttpPost]
public async Task<IActionResult> Upload(IFormFile file)
{
    if (file == null || file.Length == 0)
    {
        ModelState.AddModelError("File", "Please select a file to upload.");
        return View("Index");
    }

    // Process the uploaded file
    if (file.Length > 0)
    {
        try
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                using (var image = Image.FromStream(memoryStream))
                {
                    
                    // Check if the image format is supported (e.g., PNG)
                    if (image.RawFormat.Guid == ImageFormat.Png.Guid)
                    {
                        
                        // Save the image as PNG
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);
                        image.Save(filePath, ImageFormat.Png); // Save as PNG format
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The uploaded image is not in a valid PNG format.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("File", "Error processing the uploaded image: " + ex.Message);
        }
    }

    // You can perform additional actions or save file metadata to a database if needed

    return RedirectToAction("Index");
}



}
