namespace Integration_Test
{
    public class SensorRequest
    {
        
        public String LastCommunication { get; set; }
        public String CuttingDateTimeCalculated { get; set; }
        public String LastForecastData { get; set; }
        public int BatteryStatus { get; set; }
        public int Lat { get; set; }

        public string? State{ get; set; }

        public int Longt { get; set; }
        public int OptimalGDD { get; set; }
        public String? FieldId { get; set; }



    }
}