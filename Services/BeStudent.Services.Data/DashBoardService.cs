namespace BeStudent.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using BeStudent.Data.Common.Repositories;
    using BeStudent.Data.Models;
    using BeStudent.Services.Messaging;

    public class DashBoardService : IDashBoardService
    {
        private readonly IRepository<Code> codeRepository;
        private readonly IEmailSender sender;

        public DashBoardService(IRepository<Code> codeRepository, IEmailSender sender)
        {
            this.codeRepository = codeRepository;
            this.sender = sender;
        }

        public async Task SendCodeAsync(string email)
        {
            var code = new Code
            {
                ExpiresOn = DateTime.UtcNow.AddHours(5),
                SendTo = email,
                IsUsed = false,
            };

            await this.sender.SendEmailAsync("georgi_yosifov99@abv.bg", "Georgi", email, "Hello!",
                $"<p>Use this code to register in our system as lector: {code.Id}</p>");

            await this.codeRepository.AddAsync(code);
            await this.codeRepository.SaveChangesAsync();
        }
    }
}
