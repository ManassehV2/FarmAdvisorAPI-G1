using System;

namespace FarmAdvisor.Models.Models {

    public class CalculatedGDD {
        public Guid Id { get; set; }
        public int date { get; set; }
        public int currentValue { get; set; }
        public Guid sensorId { get; set; }
    }

}
