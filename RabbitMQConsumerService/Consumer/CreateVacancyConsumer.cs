using MediatR;
using Candidate.Application.UseCases.Portal.General;
using Candidate.Domain.General;
using Newtonsoft.Json;
using RabbitMQConsumerService.Models;

namespace RabbitMQConsumerService.Consumer
{
    public class CreateVacancyConsumer : BaseRabbitMqConsumer
    {
        private readonly IMediator _mediator;
        public CreateVacancyConsumer(IMediator mediator, ILogger<BaseRabbitMqConsumer> logger) : base("CreateVacancy", logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected async override Task ProcessMessageAsync(string message)
        {
            try
            {
                var vacancyModel = JsonConvert.DeserializeObject<VacancyModel>(message);
                if (vacancyModel == null)
                    throw new Exception("Failed to deserialize vacancy message");


                var vacancy = new Vacancy
                {
                    Name = vacancyModel.VacancyName,
                    StaffType = vacancyModel.StaffType, 
                    StructureId = vacancyModel.StructureId,
                    ExternalId = vacancyModel.ExternalId,
                    EndDate = vacancyModel.EndDate,
                    CreatedDate = vacancyModel.CreatedDate,
                    Requirement = vacancyModel.Requirement,
                    Obligation = vacancyModel.Obligation,
                    Status = vacancyModel.VacancyStatus,
                    Address = "Sample Address",
                    FieldId = Guid.NewGuid()
                };

                var command = new CreateVacancyCommand(
                    vacancy.Name,
                    vacancy.StaffType,
                    vacancy.StructureId != null ? vacancy.StructureId.Value : Guid.Empty,
                    vacancy.ExternalId != null ? vacancy.ExternalId.Value : Guid.Empty,
                    vacancy.EndDate.Value,
                    vacancy.CreatedDate.Value,
                    vacancy.Requirement,
                    vacancy.Obligation,
                    vacancy.Status,
                    vacancy.FieldId,
                    vacancy.Address
                );

                await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing RegisterUser message: {ex.Message}");
            }
        }
    }
}
