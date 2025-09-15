using BB.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BB.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodTypeController : Controller
    {
        private readonly UnitOfWork _UnitOfWork;
        public FoodTypeController(UnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _UnitOfWork.FoodType.GetAll() });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ObjFoodType = _UnitOfWork.FoodType.GetById(id);
            if (ObjFoodType == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _UnitOfWork.FoodType.Delete(ObjFoodType);

            return Json(new { success = true, message = "Delete Succesful" });

        }
    }
}
