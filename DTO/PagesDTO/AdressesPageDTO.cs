using Learning.DTO.ElementDTO;

namespace Learning.DTO.PagesDTO
{
    public class AdressesPageDTO : BaseDTO
    {
        public ICollection<AdressDTO> Adresses { get; set; }
        public int PageNumber {  get; set; } = 1;
        public int PageSize {  get; set; } = 20;
    }
}
