using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQConsumerService.Models
{
    public class VacancyModel
    {
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public String VacancyName { get; set; }
        public Guid? StructureId { get; set; }
        public Guid? ExternalId { get; set; }
        public String StaffType { get; set; }
        public String VacancyBudget { get; set; }
        public Guid? ReplacedEmployeeId { get; set; }
        public String VacancyType { get; set; }
        public Guid? RecruiterUserId { get; set; }
        public String Obligation { get; set; }
        public String Requirement { get; set; }
        public Guid? BusinessId { get; set; }
        public String Cause { get; set; }
        public String VacancyStatus { get; set; }
        public String VacancyStage { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public int? VacancyDays { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ApplicantApplyingNumber { get; set; }
        public int? ApplicantProcessNumber { get; set; }
        public String SelectedApplicant { get; set; }
        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }
        public Guid? VacancyAreaId { get; set; }
        public string StagesJson { get; set; }
        public Guid Id { get; set; }
        public int Version { get; set; }
        public bool IsNew { get; set; }
    }
}
