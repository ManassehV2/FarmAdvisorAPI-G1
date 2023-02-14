
namespace FarmAdvisor.Models.Models
{
    public class UserModel
    {

        public Guid UserID { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Email { get; set; } 
        public string? Phone { get; set; }
        public string? AuthId{ get; set; }

       public ICollection<FarmModel>? Farms { get; set; }


    }
}
