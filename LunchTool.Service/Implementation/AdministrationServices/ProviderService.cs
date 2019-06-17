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

            public void Add(ProviderDTO providerDTO)
            {

            }

            public void Change(ProviderDTO providerDTO)
            {

            }

            public void SetActiveStatus(bool isActive)
            {

            }
        }
    }
}
