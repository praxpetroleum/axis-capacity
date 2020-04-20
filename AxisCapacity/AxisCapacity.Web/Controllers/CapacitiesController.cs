using System;
using System.Globalization;
using AxisCapacity.Common;
using AxisCapacity.Data;
using AxisCapacity.Engine;
using Microsoft.AspNetCore.Mvc;

namespace AxisCapacity.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CapacitiesController : ControllerBase
    {
        private readonly ICapacityRepository _repository;

        private readonly ICapacityEngine _engine;

        public CapacitiesController(ICapacityRepository repository, ICapacityEngine engine)
        {
            _repository = repository;
            _engine = engine;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string terminal, [FromQuery] string shift, [FromQuery] string date)
        {
            try
            {
                var queryDate = string.IsNullOrEmpty(date)
                    ? (DateTime?) null
                    : DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);
                var queryShift = string.IsNullOrEmpty(shift) ? null : Shift.From(shift);

                if (queryDate.HasValue && !string.IsNullOrEmpty(terminal) && queryShift != null)
                {
                    return HandleExactQuery(terminal, queryShift, queryDate.Value);
                }

                return BadRequest("Terminal, shift and date are required fields.");
            }
            catch (FormatException e)
            {
                return BadRequest(e);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e);
            }
        }

        private IActionResult HandleExactQuery(string terminal, Shift shift, DateTime date)
        {
            var result = _repository.GetCapacity(terminal, shift, date);
            if (result == null)
            {
                return NotFound($"Did not find corresponding depot capacity entry for terminal '{terminal}', shift '{shift.Value()}, date '{date:yyyy/MM/dd}'");
            }

            var overrideResult = _repository.GetDateCapacity(terminal, shift, date);
            if (overrideResult != null)
            {
                result = overrideResult;
            }

            result.Capacity ??= _engine.CalculateCapacity(result.Load, result.Deliveries, result.Shifts);
            if (!result.Capacity.HasValue)
            {
                return BadRequest("Could not calculate capacity for terminal '{terminal}', shift '{shift.Value()}, date '{date:yyyy/MM/dd}'. Missing depot entry parameter values?");
            }

            return Ok(result);
        }
    }
}