using Microsoft.AspNetCore.Mvc;
using DoAnLTW_Nhom4.Repositories.Interfaces;

namespace DoAnLTW_Nhom4.Components
{
    public class CategoryListViewComponent : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryListViewComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
        }
    }
} 