using HR.Data;
using HR.IService;
using HR.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HR.Service
{
    public class AccountService: IAccountService
    {

       
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountService(HRContext hRContext,UserManager<ApplicationUser> _userManager,SignInManager<ApplicationUser> _signInManager,RoleManager<IdentityRole> _roleManager)

        {
                 
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }

        public async Task<(IdentityResult result, string generatedUserId)> CreateAccount(VmEmployeeAndRelatedEntities vm)
        {
            var applicationUser = new ApplicationUser();

            applicationUser.UserName = vm.signUpModel.Username;
            applicationUser.Name = vm.signUpModel.Username;
            applicationUser.PasswordHash = vm.signUpModel.Password;

            var result = await userManager.CreateAsync(applicationUser, vm.signUpModel.Password);

            if (result.Succeeded)
            {
                var createdUser = await userManager.FindByNameAsync(vm.signUpModel.Username);
                string generatedUserId = createdUser.Id;

                return (IdentityResult.Success, generatedUserId);
            }

            return (IdentityResult.Failed(result.Errors.ToArray()), null);
        }

        public async Task<IdentityResult> UpdatePassword(string userId, string oldPassword, string newPassword)
        {
            try
            {
               
                var user = await userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    
                    return IdentityResult.Failed(new IdentityError { Description = "User not found." });
                }

               
                var passwordCheckResult = await userManager.CheckPasswordAsync(user, oldPassword);

                if (!passwordCheckResult)
                {
                    
                    return IdentityResult.Failed(new IdentityError { Description = "Invalid old password." });
                }

               
                var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

                return result;
            }
            catch (Exception ex)
            {
               
                return IdentityResult.Failed(new IdentityError { Description = "Error updating password." });
            }
        }

        public async Task<IdentityResult> UpdateAccount(VmEmployeeAndRelatedEntities vm)
        {
            
            var existingUser = await userManager.FindByIdAsync(vm.employee.ApplicationUserId);

            if (existingUser == null)
            {
                
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            
            existingUser.UserName = vm.signUpModel.Username;
            existingUser.Name = vm.signUpModel.Username;
            existingUser.PasswordHash= vm.signUpModel.Password;

            
            if (!string.IsNullOrEmpty(vm.signUpModel.Password))
            {
                existingUser.PasswordHash = userManager.PasswordHasher.HashPassword(existingUser, vm.signUpModel.Password);
            }

            

            
            var result = await userManager.UpdateAsync(existingUser);

            return result;
        }


        public async Task<SignInResult> SignIn(SignInModel signInModel) 
        {
          var result = await signInManager.PasswordSignInAsync(signInModel.Username,signInModel.Password,signInModel.RemeberMe,false);

            return result;
        }
       
        public async Task<IdentityResult> CreateRole(RoleModel roleModel) 
        {
            IdentityRole newrole = new IdentityRole()
            {
                Name = roleModel.Name
            };

            var result = await roleManager.CreateAsync(newrole);

            return result;
        }

        public async Task<List<UsersDTO>> GetUsers() 
        {
         List<ApplicationUser> allUsers = await  userManager.Users.ToListAsync();
            List<UsersDTO> users = new List<UsersDTO>();
            foreach (ApplicationUser user in allUsers) 
            {

                users.Add(new UsersDTO() 
                {
                    Id= user.Id,
                    Username= user.UserName
                });
            }
            return users;
        }

        public async Task<List<RoleDTO>> getAllRole(string userId) 
        {
            List<IdentityRole> allroles = await roleManager.Roles.ToListAsync();
            List<RoleDTO> roles = new List<RoleDTO>();

            foreach(IdentityRole item in allroles)
            {
                roles.Add(new RoleDTO()
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsSelected = false,
                    UserId= userId

                });
            }
            var user = await userManager.FindByIdAsync(userId);
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (RoleDTO item in roles)
            {
                if (userRoles.Contains(item.Name))
                {
                    item.IsSelected = true;
                }

            }

            return roles;
            
        }

        public async Task<UsersDTO> getUserByid(string Id)
        {
            UsersDTO userDTO = new UsersDTO();

            var users = await userManager.FindByIdAsync(Id);
            userDTO.Username = users.UserName;
            userDTO.Id = users.Id;

            return userDTO;

        }

        public async Task UpdateRole(VmUsersAndRole vm) 
        {
            foreach (RoleDTO item in vm.listroleDTO)
            {
                ApplicationUser user = await userManager.FindByIdAsync(vm.userDTO.Id);
                if (item.IsSelected == true)
                {
                    if (await userManager.IsInRoleAsync(user, item.Name) == false)
                    {
                        await userManager.AddToRoleAsync(user, item.Name);
                    }
                }
                else
                {
                    if (await userManager.IsInRoleAsync(user, item.Name) == true)
                    {
                        await userManager.RemoveFromRoleAsync(user, item.Name);
                    }

                }
            }
        }

        public async Task<IdentityResult> DeleteAccount(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            var result = await userManager.DeleteAsync(user);

            return result;
        }

        public async Task<IList<string>> GetUserRolesByName(string username)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user != null)
            {
                return await userManager.GetRolesAsync(user);
            }

            return new List<string>();
        }

        public async Task SignOut() 
        {
           await signInManager.SignOutAsync();
        }
       
    }
}
