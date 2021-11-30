using System.Collections.Generic;

namespace PopApi.Models
{
    public class PopData
    {
        public List<EstimateModel> Estimates { get; set; }

        public List<ActualModel> Actuals { get; set; }

        public List<int> States { get; set; }
    }
}
