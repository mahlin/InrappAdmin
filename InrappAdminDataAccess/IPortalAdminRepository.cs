using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InrappAdmin.DomainModel;

namespace InrappAdminDataAccess
{
    public interface IPortalAdminRepository
    {

        string GetLeveranserForOrganisation(int orgId);
    }
}
