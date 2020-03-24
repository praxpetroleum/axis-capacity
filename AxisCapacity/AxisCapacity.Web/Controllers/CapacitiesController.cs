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
                var queryTerminal = Terminal.From(terminal);
                var queryShift = Shift.From(shift);
                var queryDate = string.IsNullOrEmpty(date) ? DateTime.Today : DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);

                var result = _repository.GetCapacity(queryTerminal, queryShift, queryDate);
                if (result == null)
                {
                    return NotFound();
                }

                var members = result.GroupId != null
                    ? _repository.GetGroupCapacities(queryTerminal, queryShift, queryDate, result.GroupId)
                    : Enumerable.Empty<CapacityResult>();


                result.Capacity = _engine.CalculateCapacity(new ParameterBuilder()
                    .WithAverageLoad(result.AverageLoad)
                    .WithDeliveriesPerShift(result.DeliveriesPerShift)
                    .WithShifts(result.NumberOfShifts).Build());

                
                foreach (var member in members)
                {
                    result.Capacity += _engine.CalculateCapacity(new ParameterBuilder()
                        .WithAverageLoad(member.AverageLoad)
                        .WithDeliveriesPerShift(member.DeliveriesPerShift)
                        .WithShifts(member.NumberOfShifts).Build());
                }
            
                return Ok(new {capacity = result.Capacity});
            } 
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid date");
            }
        }
    }
}