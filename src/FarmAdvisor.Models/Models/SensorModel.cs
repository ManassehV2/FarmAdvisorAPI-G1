using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FarmAdvisor.Models.Models
{
    public class SensorModel
    {
        public Guid SensorId { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? LastCommunication { get; set; }

        public int? BatteryStatus { get; set; }
        public int? OptimalGDD { get; set; }
        public DateTime? CuttingDateTimeCalculated { get; set; }

        public DateTime? LastForecastDate { get; set; }

        public double Lat { get; set; }
        public double Long { get; set; }

        public enum StateEnum
        {
            Working,
            Warning,
            Failed
        }
        public StateEnum? State { get; set; }

        public Guid FieldId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public FieldModel? Field { get; set; }
        
        public ICollection<SensorData>? SensorData { get; set; }




    }
}
