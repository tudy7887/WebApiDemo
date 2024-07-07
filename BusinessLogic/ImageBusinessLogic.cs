using Learning.Data;
using Learning.Interfaces.BusinessLogic;
using Learning.Model;
using Microsoft.EntityFrameworkCore;

namespace Learning.BusinessLogic
{
    public class ImageBusinessLogic : BusinessLogic, IImageBusinessLogic
    {
        public ImageBusinessLogic(DataContext dataContext) : base(dataContext) { }

        public async Task<bool> CreateImage(Image image)
        {
            return await Create<Image>(image);
        }

        public async Task<bool> DeleteImage(string imageId)
        {
            var image = await GetImage(imageId);
            return await Delete<Image>(image);
        }

        public async Task<Image> GetImage(string imageId)
        {
            return await context.Images.FirstOrDefaultAsync(i => i.Id == imageId);
        }

        public async Task<ICollection<Image>> GetImages()
        {
            return await context.Images.ToListAsync();
        }

        public async Task<bool> UpdateImage(string imageId, Image newImage)
        {
            return await Update<Image>(imageId, newImage);
        }
    }
}
