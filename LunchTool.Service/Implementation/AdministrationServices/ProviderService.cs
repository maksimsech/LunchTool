using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using LunchTool.Logic.Entities;
using LunchTool.Logic.Repository.Implementation;
using LunchTool.Logic.Repository.Interfaces;
using LunchTool.Service.DTO;
using LunchTool.Service.Interfaces.AdministrationServices;
using LunchTool.Service.Implementation;

namespace LunchTool.Service.Implementation
{
    partial class AdministrationService
    {
        public class ProviderService : IProviderService
        {
            private IUnitOfWork db;
            private IMapper mapper;

            public ProviderService(string connectionString)
            {
                db = new UnitOfWork(connectionString);
                mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<ProviderDTO, Provider>();
                }).CreateMapper();
            }

            public void Add(ProviderDTO providerDTO, bool isActive = true)
            {
                providerDTO.Active = true;
                var provider = Map(providerDTO);               
                db.Providers.Add(provider);
                db.Save();
            }

            public void Change(ProviderDTO providerDTO)
            {
                var provider = Map(providerDTO);
                db.Providers.Update(provider);
                db.Save();
            }

            public void SetActiveStatus(ProviderDTO providerDTO, bool isActive)
            {
                providerDTO.Active = isActive;
                var provider = Map(providerDTO);
                db.Providers.Update(provider);
                db.Save();
            }

            public void Activate(ProviderDTO providerDTO)
            {
                providerDTO.Active = true;
                var provider = Map(providerDTO);
                db.Providers.Update(provider);
                db.Save();
            }

            public void Deactivate(ProviderDTO providerDTO)
            {
                providerDTO.Active = false;
                var provider = Map(providerDTO);
                db.Providers.Update(provider);
                db.Save();
            }

            private Provider Map(ProviderDTO providerDTO) => mapper.Map<ProviderDTO, Provider>(providerDTO);
        }
    }
}
