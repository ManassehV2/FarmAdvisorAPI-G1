    using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FarmAdvisor.Models.Models
{
    public class FarmModel
    {
        public Guid FarmId { get; set; }
        public string? Name { get; set; }
        public string? Postcode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public ICollection<NotificationModel>? Notifications { get; set; }
        public ICollection<FieldModel>? Fields { get; set; }

        public Guid UserId { get; set; }


        //[ForeignKey("UserId")]
        [JsonIgnore]
        [IgnoreDataMember]
        public UserModel? User { get; set; }

    }
}
