using Application.Configuration;
using Application.Models;
using Application.UseCases.Constant;
using Application.UseCases.Portal.Auth;
using Domain.Constant;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CreateUserConsumer : BaseRabbitMqConsumer
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly RabbitMQSettings _settings;
        public CreateUserConsumer(IServiceScopeFactory scopeFactory, ILogger<BaseRabbitMqConsumer> logger, IOptions<RabbitMQSettings> options) : base("CreateUser", logger, options)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _settings = options.Value;
        }

        protected async override Task ProcessMessageAsync(string message)
        {
            try
            {
                var systemModel = JsonConvert.DeserializeObject<UserModel>(message);
                if (systemModel == null)
                    throw new Exception("Failed to deserialize system message");


                var user = new UserModel
                {
                    Id = systemModel.Id,
                    Name = systemModel.Name,
                    Surname = systemModel.Surname,
                    Patronymic = systemModel.Patronymic,
                    Password = systemModel.Password,
                    Email = systemModel.Email,
                    Pin = systemModel.Pin,
                    IsPasswordChangeRequired = systemModel.IsPasswordChangeRequired,
                    Status = "Active",
                };

                var command = new RegisterCommand(user.Id, user.Pin, user.Name, user.Surname, user.Email, user.Password, user.Status, user.IsPasswordChangeRequired, user.Patronymic);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(command);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CreateUser message: {ex.Message}");
            }
        }
    }
}
