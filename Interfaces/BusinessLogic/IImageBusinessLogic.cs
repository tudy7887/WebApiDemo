using Learning.Model;

namespace Learning.Interfaces.BusinessLogic
{
    public interface IImageBusinessLogic
    {
        Task<bool> CreateImage(Image image);
        Task<bool> UpdateImage(string imageId, Image newImage);
        Task<bool> DeleteImage(string imageId);
        Task<Image> GetImage(string imageId);
        Task<ICollection<Image>> GetImages();
    }
}
