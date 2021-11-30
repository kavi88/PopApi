using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PopApi.Models;
using PopApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PopApi.Controllers
{
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly ILogger<StatsController> _logger;
        private readonly IStatsService _statService;

        public StatsController(
            ILogger<StatsController> logger,
            IStatsService statsService)
        {
            _logger = logger;
            _statService = statsService;
        }

        [HttpGet("/population")]
        public IActionResult GetPopulation([FromQuery] string state)
        {
            if (string.IsNullOrEmpty(state))
            {
                _logger.LogError("Input missing.");
                return BadRequest("Input missing.");
            }

            var parseResponse = ParseInputToList(state);
            if (!parseResponse.IsSuccess)
            {
                _logger.LogError(parseResponse.ErrorMessage);
                return BadRequest(parseResponse.ErrorMessage);
            }

            if (parseResponse.Data.Count == 0)
            {
                var errorMessage = "Input missing.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            var (checkStateExists, message) = _statService.CheckStateExists(parseResponse.Data);
            if (!checkStateExists)
            {
                _logger.LogError(message);
                return NotFound(message);
            }

            return Ok(_statService.GetPopulation(parseResponse.Data));
        }

        [HttpGet("/households")]
        public IActionResult GetHouseholds([FromQuery] string state)
        {
            if (string.IsNullOrEmpty(state))
            {
                _logger.LogError("Input missing.");
                return BadRequest("Input missing.");
            }

            var parseResponse = ParseInputToList(state);
            if (!parseResponse.IsSuccess)
            {
                _logger.LogError(parseResponse.ErrorMessage);
                return BadRequest(parseResponse.ErrorMessage);
            }

            if (parseResponse.Data.Count == 0)
            {
                var errorMessage = "Input missing.";
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }

            var (checkStateExists, message) = _statService.CheckStateExists(parseResponse.Data);
            if (!checkStateExists)
            {
                _logger.LogError(message);
                return NotFound(message);
            }

            return Ok(_statService.GetHouseholds(parseResponse.Data));
        }

        private static ResponseModel<List<int>> ParseInputToList(string states)
        {
            try
            {
                var lsStates = states.Split(",").Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => Convert.ToInt32(x)).ToList();
                return new ResponseModel<List<int>>(true, lsStates);
            }
            catch (Exception)
            {
                return new ResponseModel<List<int>>(false, $"Data '{states}' not in valid format. Parsing failed!");
            }
        }
    }
}
