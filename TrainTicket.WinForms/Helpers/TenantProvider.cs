using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;
using TrainTicket.Data.DbContexts;

namespace TrainTicket.WinForms.Helpers
{
    public class TenantProvider : ITenantProvider
    {
        public string GetCurrentRegion()
        {
            return SessionManager.CurrentRegion;
        }
    }
}