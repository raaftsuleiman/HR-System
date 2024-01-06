using HR.Data;
using HR.Models;
using Microsoft.AspNetCore.Identity;

namespace HR.IService
{
    public interface IAccountService
    {
         Task<(IdentityResult result, string generatedUserId)> CreateAccount(VmEmployeeAndRelatedEntities vm);
         Task<SignInResult> SignIn(SignInModel signInModel);
         Task<IdentityResult> CreateRole(RoleModel roleModel);
         Task<IdentityResult> UpdateAccount(VmEmployeeAndRelatedEntities vm);
         Task<List<UsersDTO>> GetUsers();
         Task<List<RoleDTO>> getAllRole(string userId);
         Task UpdateRole(VmUsersAndRole vm);
         Task<UsersDTO> getUserByid(string Id);
         Task SignOut();
         Task<IdentityResult> DeleteAccount(string userId);

         Task<IList<string>> GetUserRolesByName(string username);
       
         Task<IdentityResult> UpdatePassword(string userId, string oldPassword, string newPassword);


    }
}
