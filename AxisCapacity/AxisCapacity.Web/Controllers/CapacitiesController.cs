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
            DateTime queryDate;
            try
            {
                queryDate = string.IsNullOrEmpty(date)
                    ? DateTime.Today
                    : DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                return BadRequest("Date format must be yyyyMMdd, e.g. 20200321");
            }

            if (!string.IsNullOrEmpty(terminal) && !string.IsNullOrEmpty(shift))
            {
                var result = _repository.GetCapacity(terminal, shift, queryDate);
                if (result == null)
                {
                    return NotFound();
                }

                var capacity = result.Capacity ?? _engine.CalculateCapacity(result.Load, result.Deliveries, result.Shifts);
                if (!capacity.HasValue)
                {
                    return Ok(new {error = "Could not calculate capacity due to missing parameters"});
                }

                var groupMembers = result.GroupId.HasValue
                    ? _repository.GetCapacities(terminal, shift, queryDate, result.GroupId.Value)
                    : Enumerable.Empty<DbCapacity>();

                var total = capacity.Value;
                foreach (var groupMember in groupMembers)
                {
                    var memberCapacity = groupMember.Capacity ?? _engine.CalculateCapacity(groupMember.Load, groupMember.Deliveries, groupMember.Shifts);
                    if (!memberCapacity.HasValue)
                    {
                        return Ok(new {error = $"Could not calculate capacity due to missing parameters for {groupMember}"});
                    }

                    total += memberCapacity.Value;
                }

                return Ok(new {capacity = total});
            }

            return Ok(_repository.GetCapacities(terminal, shift, queryDate));


            // var result = _repository.GetCapacities(queryTerminal, queryShift, queryDate);
            // if (result == null)
            // {
            //     return NotFound();
            // }
            //
            // var members = result.GroupId != null
            //     ? _repository.GetGroupCapacities(queryTerminal, queryShift, queryDate, result.GroupId)
            //     : Enumerable.Empty<DbCapacity>();
            //
            //
            // result.Capacity = _engine.CalculateCapacity(new ParameterBuilder()
            //     .WithAverageLoad(result.AverageLoad)
            //     .WithDeliveriesPerShift(result.DeliveriesPerShift)
            //     .WithShifts(result.NumberOfShifts).Build());
            //
            //
            // foreach (var member in members)
            // {
            //     result.Capacity += _engine.CalculateCapacity(new ParameterBuilder()
            //         .WithAverageLoad(member.AverageLoad)
            //         .WithDeliveriesPerShift(member.DeliveriesPerShift)
            //         .WithShifts(member.NumberOfShifts).Build());
            // }
            //
            // return Ok(new {capacity = result.Capacity});
        }
    }
}