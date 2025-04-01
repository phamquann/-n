using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using DoAnLTW_Nhom4.Data;
using DoAnLTW_Nhom4.Models;
using Microsoft.EntityFrameworkCore;

public class ProductFilterViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public ProductFilterViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(string search, int? categoryId, int? brandId, decimal? minPrice, decimal? maxPrice, string sortOrder, bool? inStock, bool? hasDiscount)
    {
        var products = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(search))
            products = products.Where(p => p.Name.Contains(search));

        if (categoryId.HasValue)
            products = products.Where(p => p.CategoryId == categoryId);

        if (brandId.HasValue)
            products = products.Where(p => p.BrandId == brandId);

        if (minPrice.HasValue)
            products = products.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            products = products.Where(p => p.Price <= maxPrice.Value);

        if (inStock.HasValue && inStock.Value)
            products = products.Where(p => p.Stock > 0);

        if (hasDiscount.HasValue && hasDiscount.Value)
            products = products.Where(p => p.Discount > 0);

        // Sắp xếp
        switch (sortOrder)
        {
            case "price_asc": products = products.OrderBy(p => p.Price); break;
            case "price_desc": products = products.OrderByDescending(p => p.Price); break;
            case "name_asc": products = products.OrderBy(p => p.Name); break;
            case "name_desc": products = products.OrderByDescending(p => p.Name); break;
        }

        return View(await products.ToListAsync());
    }
}
