using Learning.Data;
using Learning.DTO;
using Learning.DTO.ElementDTO;
using Learning.DTO.PagesDTO;
using Learning.Interfaces.Repository;
using Learning.Model;
using Learning.Services;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Repository
{
    public class UserInfoRepository : Repository, IUserInfoRepository
    {
        public UserInfoRepository(DataContext dataContext) : base(dataContext) { }

        public async Task<bool> AddProfilePicture(string authorId, string image)
        {
            var user = await userBusiness.GetUser(authorId);
            if (user == null) { return false; }
            user.ProfilePicture.ImageData = new ImageManipulation().SetImage(image);
            return await userBusiness.UpdateUser(authorId, user);
        }

        public async Task<bool> ChangeAdress(string authorId, string adressId, SingleAdressPageDTO newdata)
        {
            var user = await userBusiness.GetUser(authorId);
            if (user == null) { return false; }
            var adress = await adressBusiness.GetAdress(adressId);
            if (adress == null) { return false; }
            if (user.Adresses.Contains(adress))
            {
                var updatedadress = mapper.MappAdress(newdata);
                if(updatedadress == null) { return false; }
                return await adressBusiness.UpdateAdress(adressId, updatedadress);
            }
            return false;
        }

        public async Task<bool> CreateAdress(string authorId, SingleAdressPageDTO adress)
        {
            var user = await userBusiness.GetUser(authorId);
            if (user == null) { return false; }
            user.Adresses.Add(new Adress()
            {
               City = adress.Adress.City,
               Number = adress.Adress.Number,
               Country = adress.Adress.Country,
               Phone = adress.Adress.Phone,
               State = adress.Adress.State,
               Street = adress.Adress.Street
            });
            return await userBusiness.UpdateUser(authorId, user);
        }

        public async Task<SingleAdressPageDTO> GetAdress(string authorId, string adressId)
        {
            var thisuser = await userBusiness.GetUser(authorId);
            if (thisuser == null) { return null; }
            var author = mapper.MappUser(thisuser);
            var adress = await adressBusiness.GetAdress(adressId);
            if (adress == null) { return null; }
            var adressdto = mapper.MappAdress(adress, author);
            return adressdto;
        }

        public async Task<bool> IfMyAdress(string authorId, string adressId)
        {
            var thisadress = await adressBusiness.GetAdress(adressId);
            if (thisadress == null) { return false; }
            if (thisadress.User.Id == authorId) { return true; }
            return false;
        }

        public async Task<UserProfilePageDTO> GetPersonalInfo(string authorId)
        {
            var user = await userBusiness.GetUser(authorId);
            if (user == null) { return null; }
            var personalinfo = mapper.MappUserProfilePageDTO(user);
            return personalinfo;
        }

        public async Task<AdressesPageDTO> GetAdresses(string authorId)
        {
            var user = await userBusiness.GetUser(authorId);
            if (user == null) { return null; }
            var author = mapper.MappUser(user);
            var adresses =  mapper.MappAdressesPageDTO(user.Adresses, author);
            return adresses;
        }

        public async Task<bool> RemoveAdress(string authorId, string adressId)
        {
            var user = await userBusiness.GetUser(authorId);
            if (user == null) { return false; }
            var adress = await adressBusiness.GetAdress(adressId);
            if (user.Adresses.Contains(adress))
            {
                return await adressBusiness.DeleteAdress(adressId);
            }
            return false;
        }

        public async Task<bool> RemoveProfilePicture(string authorId)
        {
            var user = await userBusiness.GetUser(authorId);
            if(user == null) { return false; }
            return await imageBusiness.DeleteImage(user.ProfilePicture.Id);
        }

        public async Task<bool> UpdatePersonalInfo(string authorId, UserProfilePageDTO newdata)
        {
            var user = mapper.MappUser(newdata);
            if (user == null) { return false; }
            return await userBusiness.UpdateUser(authorId, user);
        }
    }
}
