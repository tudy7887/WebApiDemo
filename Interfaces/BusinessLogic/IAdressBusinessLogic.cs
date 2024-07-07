using Learning.Model;

namespace Learning.Interfaces.BusinessLogic
{
    public interface IAdressBusinessLogic
    {
        Task<bool> CreateAdress(Adress adress);
        Task<bool> UpdateAdress(string adressId, Adress newAdress);
        Task<bool> DeleteAdress(string adressId);
        Task<Adress> GetAdress(string adressID);
        Task<ICollection<Adress>> GetAdresses();
    }
}
