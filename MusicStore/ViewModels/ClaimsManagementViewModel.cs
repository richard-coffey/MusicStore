using System.Collections.Generic;

namespace MusicStore.ViewModels
{
    public class ClaimsManagementViewModel
    {
        public string UserId { get; set; }
        public string ClaimId { get; set; }
        public List<string> AllClaimsList { get; set; }
    }
}
