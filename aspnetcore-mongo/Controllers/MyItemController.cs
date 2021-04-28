using System;
using System.Threading.Tasks;
using aspnetcore_mongo.Services;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_mongo.Controllers
{
    public class MyItemController : Controller
    {
        private readonly ICosmosDbService _cosmosDbService;
        public MyItemController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _cosmosDbService.GetItemsAsync());
        }

        //[ActionName("Create")]
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ActionName("Create")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> CreateAsync([Bind("Id,Name")] MyItem item)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        item.Id = Guid.NewGuid().ToString();
        //        await _cosmosDbService.AddItemAsync(item);
        //        return RedirectToAction("Index");
        //    }

        //    return View(item);
        //}

        //[HttpPost]
        //[ActionName("Edit")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> EditAsync([Bind("Id,Name")] MyItem item)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _cosmosDbService.UpdateItemAsync(item.Id, item);
        //        return RedirectToAction("Index");
        //    }

        //    return View(item);
        //}

        //[ActionName("Edit")]
        //public async Task<ActionResult> EditAsync(string id)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }

        //    MyItem item = await _cosmosDbService.GetItemAsync(id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(item);
        //}

        //[ActionName("Delete")]
        //public async Task<ActionResult> DeleteAsync(string id)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }

        //    MyItem item = await _cosmosDbService.GetItemAsync(id);
        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(item);
        //}

        //[HttpPost]
        //[ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmedAsync([Bind("Id")] string id)
        //{
        //    await _cosmosDbService.DeleteItemAsync(id);
        //    return RedirectToAction("Index");
        //}

        //[ActionName("Details")]
        //public async Task<ActionResult> DetailsAsync(string id)
        //{
        //    return View(await _cosmosDbService.GetItemAsync(id));
        //}
    }
}
