namespace Web.API.Requests.Users
{
    public class UpdateUserRequest
    {

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Gender { get; set; }
        public string MartialStatus { get; set; }
        public string CurrentAddress { get; set; }
        public string RegisterAddress { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Photo { get; set; }
        public bool IsPasswordChangeRequired { get; set; }
        public bool? IsInitial { get; set; }
    }
}
