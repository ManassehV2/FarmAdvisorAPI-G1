using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace FarmAdvisor.Models.Models {

    public class CalculatedGDD {
        public Guid Id { get; set; }
        public int date { get; set; }
        public int currentValue { get; set; }
        public Guid SensorId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public SensorModel Sensor { get; set; }
    }

}
