using HR.IService;
using HR.Models;
using HR.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace HR.Controllers
{
    
    public class AccountController : Controller
    {
       
        private readonly IAccountService accountService;

        public AccountController(IAccountService _accountService)
        {
            accountService= _accountService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateAccount()
        {
            return View();
        }
        //[Authorize]
        //public async Task<IActionResult> SignUp(SignUpModel signUp)
        //{
        //    var nisa = signUp.Id;
        //   var result = await accountService.CreateAccount(signUp);

        //    ViewData["result"] =result;

        //    return View("CreateAccount");
        //}

        [AllowAnonymous]
        public IActionResult Login()
        {            

            return View("SignIn");
        }

        [AllowAnonymous]
        public async Task<IActionResult> CheckUser(SignInModel signInModel)
        {

            var result = await accountService.SignIn(signInModel);

            if (result.Succeeded)
            {
                var userRoles = await accountService.GetUserRolesByName(signInModel.Username);

                if (userRoles.Any(r => r == "Admin") && userRoles.Any(r => r == "Employee"))
                {
                    return RedirectToAction("AdminAndEmployeeView", "Account");
                   
                }
                else if (userRoles.Any(r => r == "Employee"))
                {
                    return RedirectToAction("MyProfile", "Employee");
                }
                else if (userRoles.Any(r => r == "Admin"))
                {
                    return RedirectToAction("GetAllEmployee", "Employee");
                }
                else
                {
                    
                    return RedirectToAction("AccessDenied", "Account");
                }
            }
            else
            {
                ViewData["result"] = "Invalid Username or Password";
                return View("SignIn");
            }


        }


        public IActionResult AdminAndEmployeeView()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult NewRole() 
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(RoleModel roleModel)
        {
           var result = await accountService.CreateRole(roleModel);

            ViewData["result"] = result;
            return View("NewRole");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList()
        {
           List<UsersDTO> users = await accountService.GetUsers();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllRole(string userId)
        {

            VmUsersAndRole vm = new VmUsersAndRole();

            vm.listroleDTO = await accountService.getAllRole(userId);
            UsersDTO userDtoAll = await accountService.getUserByid(userId);

            vm.userDTO = userDtoAll;

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(VmUsersAndRole vm) 
        {
            await accountService.UpdateRole(vm);

            return View("GetAllRole", vm);
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> EditPassword(string OldPassword, string NewPassword)
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await accountService.UpdatePassword(userId, OldPassword, NewPassword);

            
            if (result.Succeeded)
            {
                return Ok(new { message = "Success" });
            }
            else
            {
                return BadRequest(new { error = "Error" });
            }
        }
        [Authorize(Roles = "Employee")]
        public IActionResult UpdatePassword()
        {
           
            
            
                return View();
            
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        { 
            return View(); 
        }

        public async Task<IActionResult> Logout()
        {
            await accountService.SignOut();
            return View("SignIn");
        }
    }
}
