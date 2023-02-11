using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FarmAdvisor.Models.Models
{
    [Table("Channels")]
    public class Channel
    {

        public int Id { get; set; }
        public string? type { get; set; }
        public string? timeStamp { get; set; }
        public int startPoint { get; set; }
        public List<int>? sampleOffsets { get; set; }
        public SensorData? sensorData { get; set; }
        public string? sensorDataSerialNum { get; set; }
    }
}