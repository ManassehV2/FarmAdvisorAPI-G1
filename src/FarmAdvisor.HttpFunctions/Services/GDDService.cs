using FarmAdvisor.DataAccess.MSSQL.Functions.Interfaces;
using System;
using System.Collections.Generic;
using FarmAdvisor.Models.Models;
using System.Threading.Tasks;

namespace FarmAdvisor.HttpFunctions.Services {
    public class GDDService {
        private readonly ICrud crud;

        public GDDService(ICrud crud) {
            this.crud = crud;
        }

        public async Task<List<CalculatedGDD>> resetGddByDate(int Day, Guid SensorId) {
            var gdds = await crud.FindAll<CalculatedGDD>();
            List<CalculatedGDD> currSensorGdds = new List<CalculatedGDD>();

            foreach (CalculatedGDD gdd in gdds) {
                if (gdd.sensorId == SensorId) {
                    currSensorGdds.Add(gdd);
                }
            }

            for (int i = 0; i < currSensorGdds.Capacity; i++) {
                var gdd = currSensorGdds[i];
                if (gdd.date >= Day) {
                    await crud.Delete<CalculatedGDD>(gdd.Id);
                    currSensorGdds.RemoveAt(i);
                }
            }

            return currSensorGdds;  
        }


        // public async Task<CalculatedGDD> updateGdd()

    }
}