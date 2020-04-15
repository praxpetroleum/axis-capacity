using System;
using System.Globalization;
using System.IO;
using System.Net;
using AxisCapacity.Data;
using AxisCapacity.Web.Model;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AxisCapacity.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly ICapacityRepository _repository;

        public UploadController(ICapacityRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Post(IFormFile capacityUpload)
        {
            if (capacityUpload == null)
            {
                return Ok("No action taken");
            }

            if (!IsMultipart(Request))
            {
                return StatusCode((int) HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                using (var stream = new StreamReader(capacityUpload.OpenReadStream()))
                {
                    using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);
                    var values = csv.GetRecords<CsvCapacityValues>();
                    foreach (var value in values)
                    {
                        Validate(value);
                        var capacity = new DbCapacity();
                        capacity.Terminal = value.Terminal;
                        capacity.Day = value.Day;
                        capacity.Date = value.Date;
                        capacity.Shift = value.Shift;
                        capacity.Load = value.AverageLoad;
                        capacity.Deliveries = value.DeliveriesPerShift;
                        capacity.Shifts = value.NumberOfShifts;
                        capacity.Capacity = value.Capacity;

                        if (value.Date.HasValue)
                        {
                            _repository.InsertDateCapacity(capacity);
                        }
                        else
                        {
                            _repository.InsertCapacity(capacity);
                        }
                        
                    }
                }

                return Ok("Success");
            }
            catch (Exception e)
            {
                return Ok("An error ocurred: " + e.Message);
            }
            

            
        }

        private void Validate(CsvCapacityValues value)
        {
            if (string.IsNullOrEmpty(value.Day) && !value.Date.HasValue)
            {
                throw new ArgumentException("One of {date, day} is required: " + value);
            }

            if (!string.IsNullOrEmpty(value.Day) && value.Date.HasValue)
            {
                throw new ArgumentException("Only one of {date, day} must be specified: " + value);
            }
        }

        private static bool IsMultipart(HttpRequest request)
        {
            return !string.IsNullOrEmpty(request.ContentType) && request.ContentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }


    }
}