using BugTrakerAPI.Model.ReturnModel;
using System.IdentityModel.Tokens.Jwt;

namespace BugTrakerAPI.Model.ReturnModel;

public class LoginRes : CommonResponse
{
    public LoginCred? data {get; set;}   
}

public class LoginCred {
     public string? Token {get; set;}
    public string? Name {get; set;}
    public string? Email {get; set;}
    public string? PhoneNumber {get; set;} 

}