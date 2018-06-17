using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrganizationChart.Models;

namespace OrganizationChart.Controllers
{
    public class EmployeeController : Controller
    {
        public ActionResult Index()
        {
            return View(new FileUploadModel());
        }
        // GET: Employee
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase uploadFile)
        {
            string fileName;
            string filePath;
            FileUploadModel response = new FileUploadModel();
            try
            {
                if (uploadFile != null)
                {
                    fileName = Path.GetFileName(uploadFile.FileName);
                    filePath = Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName);
                    uploadFile.SaveAs(filePath); //Upload CSV file 
                    response.UploadFileFlag = true;
                    response.FilePathUploaded = filePath.Replace("\\", "||");
                    ViewBag.Message = "File Uploaded Successfully";
                }
                else
                    response.UploadFileFlag = false;
            }
            catch (IOException ioException)
            {
                Console.WriteLine(ioException.Message);
                ViewBag.Message = "File Upload Failed";
                response.UploadFileFlag = false;
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(response);
        }

        [HttpPost]
        public JsonResult GetOrgChart(string fileName)
        {
            List<EmployeeDetailsModel> employeeDetailsList = new List<EmployeeDetailsModel>();
            string employeeContent;
            try
            {
                //Read employee Details from CVS file
                var testFile = fileName.Replace("||", "\\");
                employeeContent = System.IO.File.ReadAllText(testFile);

                foreach (var employee in employeeContent.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(employee))
                    {
                        employeeDetailsList.Add(new EmployeeDetailsModel
                        {
                            EmployeeId = employee.Split(',')[0],
                            FirstName = employee.Split(',')[1] + " " + employee.Split(',')[2],
                            Title = employee.Split(',')[3],
                            ManagerId = employee.Split(',')[4] == "NULL\r" ? "0" : employee.Split(',')[4]

                        });
                    }
                }
            }
            catch (IOException ioException)
            {
                Console.WriteLine(ioException.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
          
            return Json(employeeDetailsList, JsonRequestBehavior.AllowGet);
        }
    }
}