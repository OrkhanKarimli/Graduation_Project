using MaleFashion.eCommerce.WebUI.Models.DataContext;
using MaleFashion.eCommerce.WebUI.Models.Entity.Membership;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.AppCode.Providers
{
    public class ClaimsTransformationProvider : IClaimsTransformation
    {
        readonly UserManager<AppUser> userManager;
        readonly FashionDbContext context;

        public ClaimsTransformationProvider(UserManager<AppUser> userManager, FashionDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        async public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            //əgər griş edilibsə
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = principal.Identity as ClaimsIdentity;

                //sistemdəki istifadəçini tapmaq
                //var currentUser = await userManager.FindByNameAsync(identity.Name);
                var currentUser = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName.Equals(identity.Name));

                if (currentUser != null)
                {
                    //if (!string.IsNullOrWhiteSpace(currentUser.ImagePath) && !identity.HasClaim(c => c.Type == OrganiClaimTypes.ProfileImage))
                    //    identity.AddClaim(new Claim(OrganiClaimTypes.ProfileImage, currentUser.ImagePath));

                    // istifadəçinin öz və rolları ilə gələn səlahiyyətlərini tapmaq
                    var userClaims = context.UserClaims.Where(x => x.UserId == currentUser.Id)
                        .Select(x => x.ClaimType)
                        .Union(from ur in context.UserRoles
                               join rc in context.RoleClaims on ur.RoleId equals rc.RoleId
                               where ur.UserId == currentUser.Id
                               select rc.ClaimType)
                        .ToArray();

                    Claim roleClaim;

                    //cookie-dəki permissionları silib təzədən əlavə etmək,ola bilər ki sistemə daxil olandan sonra səlahiyyətində dəyişiklik olub
                    foreach (var claimName in userClaims)
                    {
                        roleClaim = identity.Claims.FirstOrDefault(c => c.Type == claimName);
                        identity.TryRemoveClaim(roleClaim);
                    }

                    identity.AddClaims(context.UserClaims.Where(x => x.UserId == currentUser.Id)
                        .Select(x => new Claim(x.ClaimType, x.ClaimValue))
                        .ToList()
                        .Union((from ur in context.UserRoles
                                join rc in context.RoleClaims on ur.RoleId equals rc.RoleId
                                where ur.UserId == currentUser.Id
                                select rc).Distinct().ToList().Select(x => new Claim(x.ClaimType, x.ClaimValue)))
                        );

                    //sonra da rolları silib yenidən əlavə edirik,ola bilər daxil olandan sonra rolu silinsin
                    roleClaim = identity.Claims.FirstOrDefault(c => c.Type == identity.RoleClaimType);

                    while (roleClaim != null)
                    {
                        identity.RemoveClaim(roleClaim);
                        roleClaim = identity.Claims.FirstOrDefault(c => c.Type == identity.RoleClaimType);
                    }

                    var roles = await userManager.GetRolesAsync(currentUser);

                    foreach (var role in roles)
                        identity.AddClaim(new Claim(identity.RoleClaimType, role));
                }
            }

            return principal;
        }
    }
}
