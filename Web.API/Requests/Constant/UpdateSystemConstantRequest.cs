namespace Web.API.Requests.Constant
{
    public class UpdateSystemConstantRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }
    }
}
