using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Threading.Tasks;

namespace CleanArchMvc.WebUi.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _enviroment;
        public ProductsController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment enviroment)
        {
            _categoryService = categoryService;
            _productService = productService;
            _enviroment = enviroment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProductsAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryId = new SelectList(await _categoryService.GetCategories(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                await _productService.CreateAsync(productDTO);
                return RedirectToAction(nameof(Index));
            }
            return View(productDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var productDto = await _productService.GetByIdAsync(id);

            if (productDto == null) return NotFound();

            ViewBag.CategoryId = new SelectList(await _categoryService.GetCategories(), "Id", "Name", productDto.CategoryId);

            return View(productDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.UpdateAsync(productDTO);
                }
                catch (System.Exception)
                {

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(productDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var productDto = await _productService.GetByIdAsync(id);

            if (productDto == null) return NotFound();

            return View(productDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                await _productService.RemoveAsync(id);
            }
            catch (System.Exception)
            {

                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var productDto = await _productService.GetByIdAsync(id);

            if (productDto == null) return NotFound();

            var wwroot = _enviroment.WebRootPath;
            var image = Path.Combine(wwroot, "images\\" + productDto.Image);
            var exists = System.IO.File.Exists(image);
            ViewBag.ImageExist = exists;

            return View(productDto);
        }
    }
}
