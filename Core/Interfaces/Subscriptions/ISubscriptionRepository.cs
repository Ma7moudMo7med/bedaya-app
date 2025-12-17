using Core.Models.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Subscriptions
{
    public interface ISubscriptionRepository
    {

        public List<Subscription> ShowAll();

        public Subscription ShowDetails(Guid id);

        public void SubscriptionAdd(Subscription subscription);
        public void DeleteFromDB(Subscription subscription);
        public void Update(Subscription subscription);

        public void Save();
    }
}
