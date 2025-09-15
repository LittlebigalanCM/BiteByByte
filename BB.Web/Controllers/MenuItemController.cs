using BB.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BB.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : Controller
    {
        private readonly UnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public MenuItemController(UnitOfWork UnitOfWork, IWebHostEnvironment WebHostEnvironment)
        {
            _UnitOfWork = UnitOfWork;
            _WebHostEnvironment = WebHostEnvironment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _UnitOfWork.MenuItem.GetAll(includes: "Category,FoodType") });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ObjMenuItem = _UnitOfWork.MenuItem.GetById(id);
            if (ObjMenuItem == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            if (ObjMenuItem.Image != null)
            {
                var imgPath = Path.Combine(_WebHostEnvironment.WebRootPath, ObjMenuItem.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imgPath))
                {
                    System.IO.File.Delete(imgPath);
                }
            }

            _UnitOfWork.MenuItem.Delete(ObjMenuItem);

            return Json(new { success = true, message = "Delete Succesful" });

        }

    }
}
