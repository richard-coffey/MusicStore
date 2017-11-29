using System.Collections.Generic;

namespace MusicStore.Auth
{
    public static class MusicStoreClaimTypes
    {
        public static List<string> ClaimsList { get; set; } = new List<string> { "Delete Album", "Add Album", "Age for ordering" };
    }
}
