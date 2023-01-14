namespace FarmAdvisor.Models.Models
{
    public class FarmModel
    {
        public Guid FarmId { get; set; }
        public string? Name { get; set; }
        public string? Postcode{ get; set;}
        public string? City { get; set; }
        public string? Country { get; set; }

        public ICollection<NotificationModel>? Notifications{ get; set; }
        public ICollection<FieldModel>? Fields { get; set; }

        public ICollection<UserModel>? Users { get; set; }

        public static implicit operator FarmModel(bool v)
        {
            throw new NotImplementedException();
        }
    }
}
