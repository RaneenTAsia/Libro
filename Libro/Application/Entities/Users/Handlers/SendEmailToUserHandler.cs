using Application.Entities.Users.Queries;
using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Users.Handlers
{
    public class SendEmailToUserHandler : IRequestHandler<SendEmailToUserQuery, ActionResult>
    {
        public readonly IUserRepository _userRepository;
        public readonly IMailService _mailService;
        public readonly ILogger<SendEmailToUserHandler> _logger;
        public readonly IMapper _mapper;

        public SendEmailToUserHandler(IUserRepository userRepository, IMailService mailService, ILogger<SendEmailToUserHandler> logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _mailService = mailService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActionResult> Handle(SendEmailToUserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Check if user email exists");
            if (!(await _userRepository.UserExistsByEmailAsync(request.ToEmail)))
            {
                return new NotFoundObjectResult("This email does not exist for any user");
            }

            _logger.LogDebug("Map Retrieved email request to MailRequest");
            var mailRequest = _mapper.Map<MailRequest>(request);

            try
            {
                await _mailService.SendEmailAsync(mailRequest);
                return new OkObjectResult("Email successfully sent");
            }catch (Exception ex)
            {
                throw;
            }
        }
    }
}
