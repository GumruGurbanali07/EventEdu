using EventEdu.Application.DTOs.PersonalData;
using EventEdu.Application.DTOs.Sponsor;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventEdu.Application.Services
{
    public interface IAccountServiceForPersonalData
    {
       Task AddPersonalData(CreatePersonalDataDTO addPersonalData);
       Task<GetPersonalDataDTO> GetPersonalDatasById(Guid id);
       Task EditPersonalData(Guid id, CreatePersonalDataDTO updatePersonalDataDTO);
        
    }
}
