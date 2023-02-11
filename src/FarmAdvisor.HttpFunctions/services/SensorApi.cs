using System;
using System.Collections.Generic;
using FarmAdvisor.Models.Models;


namespace FarmAdvisor.HttpFunctions.Services
{
    public class SensorApi
    {

        List<string> sensorIds = new List<string>() { "12", "32", "34", "643", "23" };

        public SensorApi() {}

        public List<SensorData> getReadings()
        {
            Random rnd = new Random();
            List<SensorData> readings = new List<SensorData>();
            foreach (string id in sensorIds)
            {
                var sensorData = new SensorData();
                sensorData.serialNum = id;
                sensorData.batteryStatus = rnd.Next(0, 2) == 1 ? true : false;
                sensorData.measurementPeriodBase = 3600;
                sensorData.nextTransmissionAt = DateTime.Now.AddDays(1).ToLongDateString();
                sensorData.signal = rnd.Next(23, 28);
                sensorData.cloudToken = "357518080489909";
                string offsets = "";
                for (int i = 0; i < 12; i++)
                {
                    offsets += rnd.Next(-5, 5).ToString();
                    offsets += " ";
                }
                sensorData.sampleOffsets = offsets;
                sensorData.type = "measurement_type_temperature";
                sensorData.timeStamp = DateTime.Now.ToLongDateString();
                sensorData.startPoint = rnd.Next(10, 35);
                readings.Add(sensorData);
            }

            return readings;
        }

    }
}