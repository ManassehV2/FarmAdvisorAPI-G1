using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using static FarmAdvisor.Models.Models.SensorModel;

namespace FarmAdvisor.Models.Models
{

    public class NotificationModel
    {


        public Guid NotificationId { get; set; }

        public string? Title { get; set; }

        public string? Message { get; set; }

        public enum Sender
        {
            Sensor,
            User,
            Field,
            Farm
        }
        public Sender SentBy { get; set; }

        public enum Status
        {
            Unknown,
            Done,

        }
        public Status NotificationStatus { get; set; }

        public Guid FarmId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public FarmModel? Farm { get; set; }

    }
}
