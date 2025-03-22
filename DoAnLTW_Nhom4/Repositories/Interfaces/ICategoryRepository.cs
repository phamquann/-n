using DoAnLTW_Nhom4.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DoAnLTW_Nhom4.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}