using System;
using System.Globalization;
using System.IO;
using System.Net;
using AxisCapacity.Web.Model;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace AxisCapacity.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(IFormFile capacityUpload)
        {
            if (!IsMultipart(Request))
            {
                return StatusCode((int) HttpStatusCode.UnsupportedMediaType);
            }

            using (var stream = new StreamReader(capacityUpload.OpenReadStream()))
            {
                using (var csv = new CsvReader(stream, CultureInfo.InvariantCulture))
                {
                    var record = csv.GetRecords<CsvUploadValues>();
                    {

                    }
                }


            }


            return Ok("Success");
        }


        private static bool IsMultipart(HttpRequest request)
        {
            var contentType = request.ContentType;
            return !string.IsNullOrEmpty(contentType) && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }


    }
}