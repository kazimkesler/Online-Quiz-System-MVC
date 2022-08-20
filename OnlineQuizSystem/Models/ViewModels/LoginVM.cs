using Microsoft.AspNetCore.Mvc;
using OnlineQuizSystem.Models.Validators;

namespace OnlineQuizSystem.Models.ViewModels
{
    [ModelMetadataType(typeof(LoginVMValidator))]
    public class LoginVM
    {
        public string? User { get; set; }
        public string? Password { get; set; }
    }
}
