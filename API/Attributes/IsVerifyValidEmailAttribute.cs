// using System.ComponentModel.DataAnnotations;
// using API.UoW;

// namespace API.Attributes;

// public class IsVerifyValidEmailAttribute : ValidationAttribute
// {
//     protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
//     {
//         var email = value as string;
//         if(email == null)
//             return new ValidationResult("Email is Required");

//         if(!email.Contains("@technzone.com"))
//             return new ValidationResult("Email format is invalid");

//         var unitOfWork = (IUnitOfWork)validationContext.GetService(typeof(IUnitOfWork));
//         var existingUser = unitOfWork.UserRepository.GetByEmail(email);

//         if(existingUser != null)
//             return new ValidationResult("Email is already in use");

//         return ValidationResult.Success;
//     }
// }
