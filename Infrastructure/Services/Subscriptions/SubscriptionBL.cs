using Infrastructure.Data;
using Core.Models.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Subscriptions
{
    public class SubscriptionBL
    {
        AppDbContext context = new AppDbContext();
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

            context.SaveChanges();
        }

        public void SaveInDB()
        {
            context.SaveChanges();
        }

        public void DeleteFromDB(Subscription subscription)
        {
            context.Subscriptions.Remove(subscription);

            context.SaveChanges();
        }
    }
}
