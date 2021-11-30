using PopApi.Models;
using System;
using System.Collections.Generic;

namespace PopApi.Services
{
    public interface IStatsService
    {
        Tuple<bool, string> CheckStateExists(List<int> states);

        IList<ResultPopulationModel> GetPopulation(List<int> states);

        IList<ResultHouseholdModel> GetHouseholds(List<int> states);
    }
}