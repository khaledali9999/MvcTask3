using System.ComponentModel.DataAnnotations;

namespace MvcTask3.ViewModels
{
    public class ApplicationUserVM
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

    
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public string FullName { get;set; } = string.Empty;
        [DataType(DataType.Password)]
        public string CurrentPassword { get;set; } = string.Empty;
        [DataType(DataType.Password)]

        public string NewPassword { get;set; } = string.Empty;
    }
}
