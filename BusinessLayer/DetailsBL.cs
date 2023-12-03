using BusinessEntities;
using DataLayer;


namespace BusinessLayer
{
    public class DetailsBL
    {
        public ResultSet SaveUserDetails(DetailsEntity objUserDetails)
        {
            DetailsDL objUserDetailsDL = new DetailsDL();
            return objUserDetailsDL.SaveUserDetails(objUserDetails);
        }
    }
}
