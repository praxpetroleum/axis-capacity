using System;
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
                var result = date != null 
                    ? _repository.GetCapacity(Terminal.From(terminal), Shift.From(shift), DateTime.Parse(date))
                    : _repository.GetCapacity(Terminal.From(terminal), Shift.From(shift), DateTime.Today);

                if (result == null)
                {
                    return NotFound();
                }
                
                result.Capacity = _engine.CalculateCapacity(new ParameterBuilder()
                    .WithAverageLoad(result.AverageLoad)
                    .WithDeliveriesPerShift(result.DeliveriesPerShift)
                    .WithShifts(result.NumberOfShifts).Build());
            
                return Ok(result);
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