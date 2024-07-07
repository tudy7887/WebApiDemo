using Learning.Data;
using Learning.Interfaces;

namespace Learning.BusinessLogic
{
    public class BusinessLogic
    {
        protected readonly DataContext context;
        public BusinessLogic(DataContext dataContext) { context = dataContext; }
        protected async Task<bool> Update<T>(string id, IBaseModel model) where T : IBaseModel
        {
            if (((T) model).Id != id) { return false; }
            context.Update((T) model);
            return await Save();
        }
        protected async Task<bool> Create<T>(IBaseModel model) where T : IBaseModel
        {
            await context.AddAsync((T)model);
            return await Save();
        }
        protected async Task<bool> Delete<T>(IBaseModel model) where T : IBaseModel
        {
            context.Remove((T)model);
            return await Save();
        }
        protected async Task<bool> Save()
        {
            var saved = context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
