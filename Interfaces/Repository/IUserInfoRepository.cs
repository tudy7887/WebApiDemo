using Learning.DTO.PagesDTO;
using Learning.DTO.ElementDTO;
using Microsoft.AspNetCore.Mvc;
using Learning.Model;
using Learning.DTO;

namespace Learning.Interfaces.Repository
{
    public interface IUserInfoRepository
    {
        Task<bool> UpdatePersonalInfo(string authorId, UserProfilePageDTO newdata);
        Task<UserProfilePageDTO> GetPersonalInfo(string authorId);
        Task<bool> CreateAdress(string authorId, SingleAdressPageDTO adress);
        Task<bool> ChangeAdress(string authorId, string adressId, SingleAdressPageDTO newdata);
        Task<bool> RemoveAdress(string authorId, string adressId);
        Task<SingleAdressPageDTO> GetAdress(string authorId, string adressId);
        Task<AdressesPageDTO> GetAdresses(string authorId);
        Task<bool> AddProfilePicture(string authorId, string image);
        Task<bool> RemoveProfilePicture(string authorId);
        Task<BaseDTO> GetToken(string authorId);
        Task<bool> IfUserIsMe(BaseDTO author, string userId);
        Task<bool> IfMyAdress(string authorId, string adressId);
    }
}
