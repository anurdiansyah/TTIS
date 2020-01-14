using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using Newtonsoft.Json;
using RD.Lib;
using TTIS.WebUi.Data;
using TTIS.WebUi.Models;
using TTIS.WebUi.Services;

namespace TTIS.WebUi.Controllers
{
    public class CrewsController : RDController
    {

        #region Variable

        private readonly IConfiguration _configuration;
        private readonly IGitternsHttpClient _gitternsHttpClient;

        #endregion

        #region Constructor

        public CrewsController(IGitternsHttpClient gitternsHttpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _gitternsHttpClient = gitternsHttpClient;
        }
        
        #endregion

        #region Method

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CrewListAsync(string p_sKeyword, int draw, int start, int length)
        { 
            HttpClient httpClient = await _gitternsHttpClient.GetClient();
            RDDatatable dtReturn = new RDDatatable();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("p_sKeyword", p_sKeyword);
                dictParam.Add("draw", draw.ToString());
                dictParam.Add("start", start.ToString());
                dictParam.Add("length", length.ToString());

                string jsonResults = GetAsync("Crews/DataTable", httpClient, dictParam);
                if (!string.IsNullOrEmpty(jsonResults)) {
                    dtReturn = JsonConvert.DeserializeObject<RDDatatable>(jsonResults);
                }

                return Json(new
                {
                    draw = dtReturn.draw,
                    recordsFiltered = dtReturn.recordsFiltered,
                    recordsTotal = dtReturn.recordsTotal,
                    data = dtReturn.data
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IActionResult> GetCrew(string p_iCrewsId)
        {
            HttpClient httpClient = await _gitternsHttpClient.GetClient();
            Crews oReturn = new Crews();
            try
            {
                Dictionary<string, string> dictParam = new Dictionary<string, string>();
                dictParam.Add("id", p_iCrewsId);
                
                string jsonResults = GetAsync("Crews/Crew", httpClient, dictParam);
                if (!string.IsNullOrEmpty(jsonResults))
                {
                    oReturn = JsonConvert.DeserializeObject<Crews>(jsonResults);
                    oReturn.BirthDate = oReturn.BirthDate.ToLocalTime();
                    oReturn.JoinDate = oReturn.JoinDate.ToLocalTime();
                }

                return Json(oReturn);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public async Task<ActionResult> PutAsync(Crews p_oCrews, IFormFile fileCrewPhoto, IFormFile fileLicenseImage, IFormFile fileIdentityPhoto)
        {
            HttpClient httpClient = await _gitternsHttpClient.GetClient();
            try
            {
                List<IFormFile> formFiles = new List<IFormFile>();
                formFiles.Add(fileCrewPhoto);
                formFiles.Add(fileLicenseImage);
                formFiles.Add(fileIdentityPhoto);

                RDCustomResponse response = new JavaScriptSerializer().Deserialize<RDCustomResponse>(PutAsync("Crews/PutAll", httpClient, p_oCrews, formFiles));
                await SaveCrewPhoto(fileCrewPhoto, fileLicenseImage);

                if (response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = response.Message
                    });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private async Task SaveCrewPhoto(IFormFile fileCrewPhoto, IFormFile fileLicenseImage)
        {
            string crewFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\crew\\");
            if (!Directory.Exists(crewFilePath)) Directory.CreateDirectory(crewFilePath);
            string licenseFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\license\\");
            if (!Directory.Exists(licenseFilePath)) Directory.CreateDirectory(licenseFilePath);
            
            if (fileCrewPhoto != null)
            {
                using (var stream = new FileStream(crewFilePath + fileCrewPhoto.FileName, FileMode.Create))
                {
                    await fileCrewPhoto.CopyToAsync(stream);
                }
            }

            if (fileLicenseImage != null)
            {
                using (var stream = new FileStream(licenseFilePath + fileLicenseImage.FileName, FileMode.Create))
                {
                    await fileLicenseImage.CopyToAsync(stream);
                }
            }
        }

        #endregion
    }
}
