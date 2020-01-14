using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TTIS.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        string sDefaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\default\\");


        string sCrewPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\crew\\");
        [HttpGet]
        [Route("Crew")]
        public IActionResult GetCrew(string Id)
        {
            String sImageName = @"\\" + Id + ".jpeg";
            if (System.IO.File.Exists(sCrewPhotoPath + sImageName))
            {
                return PhysicalFile(sCrewPhotoPath + Id + ".jpeg", "image/jpeg");
            }
            else
            {
                return PhysicalFile(sDefaultImagePath + "no_photo.png", "image/png");
            }
        }

        string sEmployeePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\employee\\");
        [HttpGet]
        [Route("Employee")]
        public IActionResult GetEmployee(string Id)
        {
            String sImageName = @"\\" + Id + ".jpeg";
            if (System.IO.File.Exists(sEmployeePhotoPath + sImageName))
            {
                return PhysicalFile(sEmployeePhotoPath + Id + ".jpeg", "image/jpeg");
            }
            else
            {
                return PhysicalFile(sDefaultImagePath + "no_photo.png", "image/png");
            }
        }
        
        string sLicenseImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\license\\");
        [HttpGet]
        [Route("License")]
        public IActionResult GetLicense(string Id)
        {
            String sImageName = @"\\" + Id + ".jpeg";
            if (System.IO.File.Exists(sLicenseImagePath + sImageName))
            {
                return PhysicalFile(sLicenseImagePath + Id + ".jpeg", "image/jpeg");
            }
            else
            {
                return PhysicalFile(sDefaultImagePath + "no_image_rectangle.png", "image/png");
            }
        }

        string sIdentityImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\identity\\");
        [HttpGet]
        [Route("Identity")]
        public IActionResult GetIdentity(string Id)
        {
            String sImageName = @"\\" + Id + ".jpeg";
            if (System.IO.File.Exists(sIdentityImagePath + sImageName))
            {
                return PhysicalFile(sIdentityImagePath + Id + ".jpeg", "image/jpeg");
            }
            else
            {
                return PhysicalFile(sDefaultImagePath + "no_image_rectangle.png", "image/png");
            }
        }

        string sVehicleImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\vehicle\\");
        [HttpGet]
        [Route("Vehicle")]
        public IActionResult vehicle(string Id)
        {
            String sImageName = @"\\" + Id + ".jpeg";
            if (System.IO.File.Exists(sVehicleImagePath + sImageName))
            {
                return PhysicalFile(sVehicleImagePath + Id + ".jpeg", "image/jpeg");
            }
            else
            {
                return PhysicalFile(sDefaultImagePath + "no_image_rectangle.png", "image/png");
            }
        }
    }
}