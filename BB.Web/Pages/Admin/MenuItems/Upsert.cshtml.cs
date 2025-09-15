using BB.Application;
using BB.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BB.Web.Pages.Admin.MenuItems
{
    public class UpsertModel : PageModel
    {
        private readonly UnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        [BindProperty]
        public MenuItem ObjMenuItem { get; set; } = new MenuItem();

        //create dropdowns for associate tables
        public IEnumerable<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> FoodTypeList { get; set; } = new List<SelectListItem>();
        public UpsertModel(UnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public void OnGet(int? id)
        {
            var categories = _UnitOfWork.Category.GetAll();
            var foodTypes = _UnitOfWork.FoodType.GetAll();

            CategoryList = categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
            FoodTypeList = foodTypes.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name });

            //assuming we are creating and not updating existing
            if (id == 0)
            {
                ObjMenuItem = new MenuItem();
            }

            else
            {
                ObjMenuItem = _UnitOfWork.MenuItem.GetById(id);

            }
        }

        public IActionResult OnPost(int? id)
        {
            var files = HttpContext.Request.Form.Files;
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, @"images\menuitems\");

            if (ObjMenuItem.Id == 0)
            {
                HandleImageUpload(files, uploadsFolder, out string? imagePath);
                ObjMenuItem.Image = imagePath;
                _UnitOfWork.MenuItem.Add(ObjMenuItem);
            }
            else
            {
                if (ObjMenuItem == null)
                {
                    return NotFound();
                }
                if (files.Count > 0)
                {
                    //new image has been uploaded
                    var ObjFromDb = _UnitOfWork.MenuItem.Get(m => m.Id == id);
                    DeleteExistingImage(ObjFromDb.Image);
                    HandleImageUpload(files, uploadsFolder, out string? imagePath);
                    ObjMenuItem.Image = imagePath;
                }
                else
                {
                    //no new image, keep existing
                    var ObjFromDb = _UnitOfWork.MenuItem.Get(m => m.Id == id);
                    ObjMenuItem.Image = ObjFromDb.Image;
                }
                _UnitOfWork.MenuItem.Update(ObjMenuItem);
            }
            return RedirectToPage("./Index");
        }

        private void HandleImageUpload(IFormFileCollection files, string uploadsFolder, out string? imagePath)
        {
            imagePath = null;
            if (files.Count > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(files[0].FileName);
                string fullPath = Path.Combine(uploadsFolder, fileName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                imagePath = @"\images\menuitems\" + fileName;
            }
        }

        private void DeleteExistingImage(string? imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('\\'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
        }
    }
}
