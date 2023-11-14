using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Encodings.Web;

namespace CompressorBackend.Controllers;

public class CustomerController : Controller
{


    private readonly CompressContext _db;

    public CustomerController(CompressContext db)
    {
        _db = db;
    }
       



    public IActionResult Index()
    {
        IEnumerable<Customer> objCustomerList = _db.Customers;
        return View(objCustomerList);
    }


    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Customer obj)
    {
     
        if (ModelState.IsValid)
        {
            _db.Customers.Add(obj);
            _db.SaveChanges();
            TempData["success"] = "Customer created successfully";
            return RedirectToAction("Index");
        }
        return View(obj);   
    }

    public IActionResult Privacy()
    {
        return View();
    }

}