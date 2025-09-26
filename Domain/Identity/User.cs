using Domain.Base;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Identity
{
    public class User : BaseEntity<Guid>
    {
        public string Pin { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public DateTimeOffset? PasswordExpiryDate { get; private set; }
        public string? Patronymic { get; set; }
        public string Status { get; private set; }
        public string? Gender { get; set; }
        public string? MartialStatus { get; set; }
        public string? CurrentAddress { get; set; }
        public string? RegisterAddress { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? PhotoPath { get; set; }
        public bool IsPasswordChangeRequired { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool? IsInitial { get; set; }
        public static User Create(string pin, string name, string surname,string patronymic, string email, string password, string status, string gender, string martialStatus, string currentAddress, string registerAddress, DateTime? birthDate,string photoPath,bool isPasswordChangeRequired, Guid? id = null, bool? isInitial = true)
        {
            return new User
            {
                Id = id.HasValue? id.Value: Guid.NewGuid(),
                Pin = pin,
                Name = name,
                Surname = surname,
                Email = new Email(email).Value,
                Password = password,
                Status = status,
                Gender = gender,
                MartialStatus = martialStatus,
                CurrentAddress = currentAddress,
                RegisterAddress = registerAddress,
                BirthDate = birthDate?.ToUniversalTime() ?? null,
                Patronymic= patronymic,
                PhotoPath= photoPath,
                IsPasswordChangeRequired= isPasswordChangeRequired,
                IsInitial= isInitial
            };
        }

        public void Update(string? name = null, string? surName = null, string? patronymic = null,
                string? gender = null, string? martialStatus = null,
                string? currentAddress = null, string? registerAddress = null, DateTime? birthDate =null, string? photoPath = null, bool isPasswordChangeRequired =false,  bool? isInitial = null)
        {
            Name = name ?? Name;
            Surname = surName ?? Surname;
            Patronymic = patronymic ?? Patronymic;
            Gender = gender ?? Gender;
            MartialStatus = martialStatus ?? MartialStatus;
            CurrentAddress = currentAddress ?? CurrentAddress;
            RegisterAddress = registerAddress ?? RegisterAddress;
            BirthDate = birthDate?.ToUniversalTime() ?? BirthDate;
            PhotoPath = photoPath == null ? null : photoPath;
            IsPasswordChangeRequired= isPasswordChangeRequired;
            IsInitial = isInitial ?? IsInitial;
        }


        public void ChangePassword(string password)
        {
            Password = password;
            IsPasswordChangeRequired = false;
        }
        public void ResetPassword(string newPassword)
        {
            Password = newPassword;
            IsPasswordChangeRequired= true;
        }
        public void ChangeInitial()
        {
            IsInitial = false;
        }
    }
}
