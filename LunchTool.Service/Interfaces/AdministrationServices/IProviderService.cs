using LunchTool.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.Interfaces.AdministrationServices
{
    public interface IProviderService
    {
        void Add(ProviderDTO providerDTO);
        void Change(ProviderDTO providerDTO);
        void SetActiveStatus(bool isActive);
    }
}
