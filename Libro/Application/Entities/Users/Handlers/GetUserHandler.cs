﻿using Application.DTOs;
using Application.Entities.Users.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Users.Handlers
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, ActionResult>
    {
        public readonly IUserRepository _userRepository;
        public readonly IMapper _mapper;
        public readonly ILogger<GetUserHandler> _logger;

        public GetUserHandler(IUserRepository userRepository, IMapper mapper, ILogger<GetUserHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Get User with Id {0}", request.UserId);
            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            if(user == null)
            {
                return new NotFoundObjectResult("User does not exist");
            }

            return new OkObjectResult(_mapper.Map<UserDTO>(user));
        }
    }
}
