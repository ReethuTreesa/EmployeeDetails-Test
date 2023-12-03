using BusinessEntities;
using BusinessLayer;
using EmployeeDetails.Models;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeDetails.Controllers
{
    public class PersonalDetailsController : Controller
    {
     
        
        public IActionResult Index()
        {
            return View("~/Views/Master/Register.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> SaveUserDetails([FromBody] DetailsModel formData)
        {
            try
            {
                DetailsBL objdetails = new DetailsBL();
                DetailsEntity deatils = new DetailsEntity();
                deatils.FirstName= formData.FirstName;
                deatils.LastName = formData.LastName;
                var result = objdetails.SaveUserDetails(deatils);
                if (result.StatusCode == "200")
                {

                    return Ok(new { Message = "User Details saved successfully!" });
                }
                else
                {
                    return StatusCode(500, new { Message = $"Failed" });
                }
                    
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Internal Server Error: {ex.Message}" });
            }
        }
    }

  

}

