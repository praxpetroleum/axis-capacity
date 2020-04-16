using System;
using System.Globalization;
using System.Linq;
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
                var queryTerminal = string.IsNullOrEmpty(terminal) ? null : Terminal.From(terminal);
                var queryShift = string.IsNullOrEmpty(shift) ? null : Shift.From(shift);

                if (queryDate.HasValue && queryTerminal != null && queryShift != null)
                {
                    return HandleExactQuery(queryTerminal, queryShift, queryDate.Value);
                }

                return HandlePartialQuery(queryTerminal, queryShift, queryDate);
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

        private IActionResult HandlePartialQuery(Terminal terminal, Shift shift, DateTime? date)
        {
            var capacities = _repository.GetCapacities(terminal, shift, date).ToList();
            foreach (var capacity in capacities)
            {
                if (!capacity.Capacity.HasValue)
                {
                    capacity.Capacity = _engine.CalculateCapacity(capacity.Load, capacity.Deliveries, capacity.Shifts);
                }
            }

            return Ok(capacities);
        }

        private IActionResult HandleExactQuery(Terminal terminal, Shift shift, DateTime date)
        {
            var result = _repository.GetCapacity(terminal, shift, date);
            if (result == null)
            {
                return NotFound();
            }

            var capacity = result.Capacity ?? _engine.CalculateCapacity(result.Load, result.Deliveries, result.Shifts);
            if (!capacity.HasValue)
            {
                return BadRequest("Could not calculate capacity due to missing parameters");
            }

            var groupMembers = result.GroupId.HasValue
                ? _repository.GetGroupCapacities(terminal, shift, date, result.GroupId.Value)
                : Enumerable.Empty<DbCapacity>();

            var total = capacity.Value;
            foreach (var groupMember in groupMembers)
            {
                var memberCapacity = groupMember.Capacity ?? _engine.CalculateCapacity(groupMember.Load, groupMember.Deliveries, groupMember.Shifts);
                if (!memberCapacity.HasValue)
                {
                    return BadRequest($"Could not calculate capacity due to missing parameters for {groupMember}");
                }

                total += memberCapacity.Value;
            }

            return Ok(new {capacity = total});
        }
    }
}