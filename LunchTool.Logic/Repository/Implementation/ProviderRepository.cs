using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using LunchTool.Logic.Enities;
using LunchTool.Logic.Context;

namespace LunchTool.Logic.Repository.Implementation
{
    public class ProviderRepository: IRepository<Provider>
    {
        private DataContext dataContext;

        public ProviderRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public IEnumerable<Provider> GetAll() => dataContext.Providers;

        public Provider Get(int id) => dataContext.Providers.Find(id);

        public void Add(Provider item) => dataContext.Providers.Add(item);

        public void Update(Provider item) => dataContext.Entry(item).State = EntityState.Modified;

        public IEnumerable<Provider> Find(Func<Provider, bool> predicate) => dataContext.Providers.Where(predicate).ToList();

        public void Delete(int id)
        {
            var provider = dataContext.Providers.Find(id);
            if (provider != null)
                dataContext.Providers.Remove(provider);
        }
    }
}
