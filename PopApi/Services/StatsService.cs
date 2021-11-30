using PopApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PopApi.Services
{
    public class StatsService : IStatsService
    {
        private readonly PopData _data;

        public StatsService(PopData data)
        {
            _data = data;
        }
      
        //returns true if all the states are found. Otherwise false.
        public Tuple<bool, string> CheckStateExists(List<int> states)
        {
            var statesNotFound = states.Where(x => !_data.States.Contains(x)).ToList();
            var message = $"States {string.Join(',', statesNotFound)} not found";
            return new Tuple<bool, string>(!statesNotFound.Any(), message);
        }

        // Gets the households for the given states.
        public IList<ResultHouseholdModel> GetHouseholds(List<int> states)
        {
            var result = new List<ResultHouseholdModel>();
            foreach (var state in states)
            {
                var popData = _data.Actuals.FirstOrDefault(x => x.State == state);
                var households = 0.0D;
                if (popData == null)
                {
                    households = _data.Estimates.Where(x => x.State == state)
                        .Select(x => x.EstimatesHouseholds).Sum();
                }
                else
                {
                    households = popData.ActualHouseholds;
                }

                result.Add(new ResultHouseholdModel()
                {
                    State = state,
                    Households = households
                });
            }

            return result;
        }

        // Gets the population for the given states.
        public IList<ResultPopulationModel> GetPopulation(List<int> states)
        {
            var result = new List<ResultPopulationModel>();
            foreach (var state in states)
            {
                var popData = _data.Actuals.FirstOrDefault(x => x.State == state);
                var population = 0.0D;
                if (popData == null)
                {
                    population = _data.Estimates.Where(x => x.State == state)
                        .Select(x => x.EstimatesPopulation).Sum();
                }
                else
                {
                    population = popData.ActualPopulation;
                }

                result.Add(new ResultPopulationModel()
                {
                    State = state,
                    Population = population
                });
            }

            return result;
        }
    }
}
