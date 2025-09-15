using BB.Application;
using Microsoft.AspNetCore.Mvc;

namespace BB.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly UnitOfWork _UnitOfWork;
        public CategoryController(UnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _UnitOfWork.Category.GetAll() });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ObjCategory = _UnitOfWork.Category.GetById(id);
            if (ObjCategory == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _UnitOfWork.Category.Delete(ObjCategory);

            return Json(new { success = true, message = "Delete Succesful" });

        }
    }
}
