using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace MyApi;

[ApiController]
public class RegisterController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IUserStore<User> _userStore;
    private readonly IUserEmailStore<User> _emailStore;
    private static readonly EmailAddressAttribute _emailAddressAttribute = new();

    public RegisterController(UserManager<User> userManager, SignInManager<User> signInManager,
        IUserStore<User> userStore,
        IUserEmailStore<User> emailStore)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userStore = userStore;
        _emailStore = emailStore;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserData registration)
    {
        var email = registration.Email;
        var userName = registration.UserName;

        if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
        {
            //show error
        }

        var user = new User();
        await _userStore.SetUserNameAsync(user, userName, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            //show error
        }

        //TODO send email

        var token = await _userManager.GenerateUserTokenAsync(
            user, PasswordlessTokenProvider.ProviderName, "passwordless-auth");

        return Ok(token);
    }

    [HttpPost("newtoken")]
    public async Task<IActionResult> NewToken(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return NotFound();
        }

        //TODO send email

        var token = await _userManager.GenerateUserTokenAsync(
            user, PasswordlessTokenProvider.ProviderName, "passwordless-auth");

        return Ok(token);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginData loginData)
    {
        var user = await _userManager.FindByIdAsync(loginData.UserId);

        if (user is null)
        {
            return NotFound();
        }

        var isValid = await _userManager.VerifyUserTokenAsync(
            user, PasswordlessTokenProvider.ProviderName, "passwordless-auth", loginData.Token);

        if (!isValid)
        {
            return Unauthorized();
        }

        _signInManager.AuthenticationScheme = IdentityConstants.ApplicationScheme;
        await _signInManager.SignInAsync(user, false);

        return Ok();
    }

    public record UserData(string Email, string UserName);

    public record LoginData(string UserId, string Token);
}