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
            if (!IsMultipart(Request))
            {
                return StatusCode((int) HttpStatusCode.UnsupportedMediaType);
            }

            using (var stream = new StreamReader(capacityUpload.OpenReadStream()))
            {
                using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                var values = csv.GetRecords<CsvCapacityValues>();
                foreach (var value in values)
                {
                    _repository.InsertCapacity(new DbCapacity
                    {
                        Terminal = value.Terminal,
                        Day = value.Day,
                        Shift =  value.Shift,
                        Load =  value.AverageLoad,
                        Deliveries = value.DeliveriesPerShift,
                        Shifts = value.NumberOfShifts,
                        Capacity = value.Capacity
                    });
                }
            }

            return Ok("Success");
        }


        private static bool IsMultipart(HttpRequest request)
        {
            return !string.IsNullOrEmpty(request.ContentType) && request.ContentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }


    }
}