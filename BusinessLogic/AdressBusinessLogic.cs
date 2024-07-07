using Learning.Data;
using Learning.Interfaces.BusinessLogic;
using Learning.Model;
using Microsoft.EntityFrameworkCore;

namespace Learning.BusinessLogic
{
    public class AdressBusinessLogic : BusinessLogic, IAdressBusinessLogic
    {
        public AdressBusinessLogic(DataContext dataContext) : base(dataContext) { }

        public async Task<bool> CreateAdress(Adress adress)
        {
            adress.Orders = new List<Order>();
            return await Create<Adress>(adress);
        }

        public async Task<bool> DeleteAdress(string adressId)
        {
            var adress = await GetAdress(adressId);
            return await Delete<Adress>(adress);
        }

        public async Task<Adress> GetAdress(string adressID)
        {
            return await context.Adresses.FirstOrDefaultAsync(a => a.Id == adressID);
        }

        public async Task<ICollection<Adress>> GetAdresses()
        {
            return await context.Adresses.ToListAsync();
        }

        public async Task<bool> UpdateAdress(string adressId, Adress newAdress)
        {
            return await Update<Adress>(adressId, newAdress);
        }
    }
}
