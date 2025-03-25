using DoAnLTW_Nhom4.Models;

namespace DoAnLTW_Nhom4.Repositories.Interfaces
{
    public interface IProductSpecificationRepository
    {
        Task<List<ProductSpecification>> GetAllAsync();
        Task<ProductSpecification> GetByIdAsync(int id);
        Task AddAsync(List<ProductSpecification> productSpecification);
        Task UpdateAsync(ProductSpecification productSpecification);
        Task DeleteAsync(int id);
    }
}
