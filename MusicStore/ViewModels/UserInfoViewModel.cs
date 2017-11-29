using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MusicStore.Auth;

namespace MusicStore.ViewModels
{
    public class UserInfoViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }

        public IEnumerable<Claim> Claims { get; set; }
    }
}
