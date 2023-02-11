using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmAdvisor.Models.Models
{
    [Table("SensorDatas")]
    public class SensorData
    {
        [Key]
        public string? serialNum { get; set; }
        public string? type { get; set; }
        public bool? batteryStatus { get; set; }
        public int measurementPeriodBase { get; set; }
        public string? nextTransmissionAt { get; set; }
        public int signal { get; set; }
        public string? timeStamp { get; set; }
        public int startPoint { get; set; }
        public string? sampleOffsets { get; set; }
        public string? cloudToken { get; set; }
        // public Channel channels { get; set; }

        // public SensorData() {
        //     sampleOffsets = new List<int>();
        // }

    }
}