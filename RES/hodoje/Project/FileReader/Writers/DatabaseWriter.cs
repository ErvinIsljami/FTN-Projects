using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Entities.Models;
using FileReader.Interfaces;

namespace FileReader.Writers
{
    public class DatabaseWriter : IWriter
    {
        private IUnitOfWork _unitOfWork;

        public DatabaseWriter(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Write(List<PowerConsumptionData> data, out string errorMessage)
        {
            if (data.Count > 0)
            {
                DateTime minDate = data.Min(x => x.Timestamp);
                DateTime maxDate = data.Max(x => x.Timestamp);
                string areaId = data.First().GeoAreaId;
                errorMessage = "";

                List<PowerConsumptionData> possibleDuplicates = _unitOfWork.PowerConsumptionDataRepository
                                                                           .Find(x => 
                                                                                    (x.Timestamp >= minDate && x.Timestamp <= maxDate) &&
                                                                                     x.GeoAreaId == areaId)
                                                                           .ToList();

                foreach (PowerConsumptionData possibleDuplicate in possibleDuplicates)
                {
                    PowerConsumptionData possibleDataToDelete = data.FirstOrDefault(x => x.Timestamp == possibleDuplicate.Timestamp && x.GeoAreaId == possibleDuplicate.GeoAreaId);
                    if (possibleDataToDelete != null)
                    {
                        data.RemoveAll(x => x.Timestamp == possibleDataToDelete.Timestamp);
                        errorMessage += $"Data with GeoAreaId: '{possibleDataToDelete.GeoAreaId}', Timestamp: {possibleDataToDelete.Timestamp} already exists in database.\n";
                    }
                }

                _unitOfWork.PowerConsumptionDataRepository.AddRange(data);
                _unitOfWork.Complete();
            }
            else
            {
                errorMessage = "";
            }
        }
    }
}