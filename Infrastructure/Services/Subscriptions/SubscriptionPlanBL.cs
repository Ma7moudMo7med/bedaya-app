using Infrastructure.Data;
using Core.Models.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Subscriptions
{
    public class SubscriptionPlanBL
    {
        AppDbContext context = new AppDbContext();
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

            context.SaveChanges();
        }

        public void SaveInDB()
        {
            context.SaveChanges();
        }

        public void DeleteFromDB(SubscriptionPlan subscriptionplan)
        {
            context.SubscriptionPlans.Remove(subscriptionplan);

            context.SaveChanges();
        }
    }
}
