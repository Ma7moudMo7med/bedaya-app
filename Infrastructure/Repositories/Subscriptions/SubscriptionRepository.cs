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
    public class SubscriptionRepository : ISubscriptionRepository
    {
        AppDbContext context;
        public SubscriptionRepository(AppDbContext _context)
        {
            context = _context;
        }
        public List<Subscription> ShowAll()
        {
            return context.Subscriptions.ToList();
        }

        public Subscription ShowDetails(Guid id)
        {
            return context.Subscriptions.FirstOrDefault(E => E.Id == id);
        }

        public void SubscriptionAdd(Subscription subscription)
        {
            context.Subscriptions.Add(subscription);
        }
        public void DeleteFromDB(Subscription subscription)
        {
            context.Subscriptions.Remove(subscription);
        }
        public void Update(Subscription subscription)
        {
            context.Update(subscription);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
