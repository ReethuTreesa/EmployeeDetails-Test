using BusinessEntities;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DetailsDL
    {
        public ResultSet SaveUserDetails(DetailsEntity objUserDetails)
        {
            ResultSet result = new ResultSet();
            try
            {

                string json = JsonSerializer.Serialize(objUserDetails);
                System.IO.File.WriteAllTextAsync("formdata.json", json);
               
                result.StatusCode = "200";
                return result;
            }
            catch(Exception ex)
            {
                result.StatusCode = "500";
                return result;
            }
        }
    }
}
