using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InrappAdmin.ApplicationService.Interface;
using InrappAdminDataAccess;

namespace InrappAdmin.ApplicationService
{
    public class PortalAdminService : IPortalAdminService
    {

        private readonly IPortalAdminRepository _portalRepository;

        public PortalAdminService(IPortalAdminRepository portalRepository)
        {
            _portalRepository = portalRepository;
        }

        public string HamtaHistorikForOrganisation(int orgId)
        {
            var test  = _portalRepository.GetLeveranserForOrganisation(orgId);

            return test;
        }
    }
}
