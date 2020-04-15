using System;
using System.Globalization;
using System.Linq;
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
            DateTime? queryDate;
            try
            {
                queryDate = string.IsNullOrEmpty(date) ? (DateTime?) null : DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            catch (FormatException e)
            {
                return BadRequest(e.Message);
            }

            if (!string.IsNullOrEmpty(terminal) && !string.IsNullOrEmpty(shift) && queryDate.HasValue)
            {
                var result = _repository.GetCapacity(terminal, shift, queryDate.Value);
                if (result == null)
                {
                    return NotFound();
                }

                var capacity = result.Capacity ?? _engine.CalculateCapacity(result.Load, result.Deliveries, result.Shifts);
                if (!capacity.HasValue)
                {
                    return BadRequest(new {error = "Could not calculate capacity due to missing parameters"});
                }

                var groupMembers = result.GroupId.HasValue
                    ? _repository.GetGroupCapacities(terminal, shift, queryDate.Value, result.GroupId.Value)
                    : Enumerable.Empty<DbCapacity>();

                var total = capacity.Value;
                foreach (var groupMember in groupMembers)
                {
                    var memberCapacity = groupMember.Capacity ?? _engine.CalculateCapacity(groupMember.Load, groupMember.Deliveries, groupMember.Shifts);
                    if (!memberCapacity.HasValue)
                    {
                        return BadRequest(new {error = $"Could not calculate capacity due to missing parameters for {groupMember}"});
                    }

                    total += memberCapacity.Value;
                }

                return Ok(new {capacity = total});
            }

            var capacities = _repository.GetCapacities(terminal, shift, queryDate).ToList();
            foreach (var capacity in capacities)
            {
                if (!capacity.Capacity.HasValue)
                {
                    capacity.Capacity = _engine.CalculateCapacity(capacity.Load, capacity.Deliveries, capacity.Shifts);
                }
            }

            return Ok(capacities);
        }
    }
}