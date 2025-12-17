using Core.Interfaces.Subscriptions;
using Core.Models.Subscriptions;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Subscriptions
{
    public class SubscriptionPlanRepository : ISubscriptionPlanRepository
    {
        AppDbContext context;
        public SubscriptionPlanRepository(AppDbContext _context)
        {
            context = _context;
        }
        //basic CRUD
        public List<SubscriptionPlan> ShowAll()
        {
            return context.SubscriptionPlans.ToList();
        }

        public SubscriptionPlan ShowDetails(Guid id)
        {
            return context.SubscriptionPlans.FirstOrDefault(E => E.Id == id);
        }

        public void SubscriptionPlanAdd(SubscriptionPlan subscriptionplan)
        {
            context.SubscriptionPlans.Add(subscriptionplan);
        }
        public void DeleteFromDB(SubscriptionPlan subscriptionplan)
        {
            context.SubscriptionPlans.Remove(subscriptionplan);
        }
        public void Update(SubscriptionPlan subscriptionPlan)
        {
            context.Update(subscriptionPlan);
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
