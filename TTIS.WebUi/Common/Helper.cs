using Newtonsoft.Json;
using RD.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTIS.WebUi.Models;

namespace TTIS.WebUi.Common
{
    public class Helper : RDController
    {

        //public void getDepartments()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        //        CustomResponse response = JsonConvert.DeserializeObject<CustomResponse>(GetAsync("MasDepartment/Departments", httpClient, dictParam));
        //        if (!string.IsNullOrEmpty(response.JsonData))
        //        {
        //            HttpContext.Session.SetString("Departments", response.JsonData);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public void getUnit()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        //        CustomResponse response = JsonConvert.DeserializeObject<CustomResponse>(GetAsync("MasUnit/Units", httpClient, dictParam));
        //        if (!string.IsNullOrEmpty(response.JsonData))
        //        {
        //            HttpContext.Session.SetString("Units", response.JsonData);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public void getTitle()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        //        CustomResponse response = JsonConvert.DeserializeObject<CustomResponse>(GetAsync("MasTitle/Titles", httpClient, dictParam));
        //        if (!string.IsNullOrEmpty(response.JsonData))
        //        {
        //            HttpContext.Session.SetString("Titles", response.JsonData);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public void getEmployeeDocumentTypeList()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        //        CustomResponse response = JsonConvert.DeserializeObject<CustomResponse>(GetAsync("EmployeeDocumentType/EmployeeDocumentTypes", httpClient, dictParam));
        //        if (!string.IsNullOrEmpty(response.JsonData))
        //        {
        //            HttpContext.Session.SetString("EmployeeDocumentTypes", response.JsonData);
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public void getEmployeeStatusList()
        //{
        //    try
        //    {
        //        Dictionary<string, string> dictParam = new Dictionary<string, string>();
        //        CustomResponse response = JsonConvert.DeserializeObject<CustomResponse>(GetAsync("EmployeeStatus/EmployeeStatusList", httpClient, dictParam));
        //        if (!string.IsNullOrEmpty(response.JsonData))
        //        {
        //            HttpContext.Session.SetString("EmployeeStatusList", response.JsonData);
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
    }
}
