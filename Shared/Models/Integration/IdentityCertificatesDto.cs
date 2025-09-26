namespace Shared.Models.Integration
{
    public sealed class IdentityCertificatesDto
    {
        public LoginDetailDto LoginDetail { get; set; }
    }


    public record LoginDetailDto
    {
        public string? Email { get; set; }
        public string? UserId { get; set; }
    }

    public record CertificateDto
    {
        public string Tin { get; set; }
        public string StructureName { get; set; }
        public string SerialNumber { get; set; }
    }
}
