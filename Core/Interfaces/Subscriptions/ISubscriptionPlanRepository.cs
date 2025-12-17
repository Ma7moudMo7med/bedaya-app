using Core.Models.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Subscriptions
{
    public interface ISubscriptionPlanRepository
    {
        public List<SubscriptionPlan> ShowAll();

        public SubscriptionPlan ShowDetails(Guid id);

        public void SubscriptionPlanAdd(SubscriptionPlan subscriptionplan);
        public void DeleteFromDB(SubscriptionPlan subscriptionplan);
        public void Update(SubscriptionPlan subscriptionPlan);
        public void Save();
    }
}
