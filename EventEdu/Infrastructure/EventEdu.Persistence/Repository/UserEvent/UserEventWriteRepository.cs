﻿using EventEdu.Application.Repository;
using EventEdu.Domain.Entities;
using EventEdu.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Persistence.Repository
{
    public class UserEventWriteRepository : WriteRepository<UserEvent>, IUserEventWriteRepository
    {
        public UserEventWriteRepository(AppDbContext context) : base(context)
        {

        }

    }
    }
