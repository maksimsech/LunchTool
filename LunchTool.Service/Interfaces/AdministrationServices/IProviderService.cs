using LunchTool.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LunchTool.Service.Interfaces.AdministrationServices
{
    public interface IProviderService
    {
        int Add(ProviderDTO providerDTO, bool isActive = true);
        void Change(ProviderDTO providerDTO);
        void SetActiveStatus(ProviderDTO providerDTO, bool isActive);
        void Activate(ProviderDTO providerDTO);
        void Deactivate(ProviderDTO providerDTO);
    }
}
