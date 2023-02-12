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

        public enum SenderEnum
        {
            Sensor,
            User,
            Field,
            Farm
        }
        public SenderEnum SentBy { get; set; }

        public enum StatusEnum
        {
            Unknown,
            Done,

        }
        public StatusEnum Status { get; set; }

        public Guid FarmId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public FarmModel? Farm { get; set; }

    }
}
