using Core.Interfaces;
using Core.Interfaces.Subscriptions;
using Core.Models.Subscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skillup_Academy.ViewModels.SubscriptionsViewModels;

namespace Skillup_Academy.Controllers.Subscriptions
{
    [Authorize(Roles = "Admin")]
    public class SubscriptionController : Controller
    {
        public readonly ISubscriptionRepository _subscriptionRepository;
		private readonly IRepository<Subscription> _repoSubscription;

		public SubscriptionController(ISubscriptionRepository subscriptionRepository,IRepository<Subscription> repoSubscription)
        {
            _subscriptionRepository = subscriptionRepository;
			_repoSubscription = repoSubscription;
		}

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ShowAllPlanInHome() 
        {
			var listSubcription = _repoSubscription.Query()
                .Where(a=>a.IsActive==true)
                .ToList();

			return View(listSubcription);
        }

        public IActionResult ShowAll()
        {
            List<Subscription> subscriptionList = _subscriptionRepository.ShowAll();
            return View("ShowAll", subscriptionList);
        }

        public IActionResult ShowDetails(Guid id)
        {
            Subscription subscriptiondetails = _subscriptionRepository.ShowDetails(id);
            return View("ShowDetails", subscriptiondetails);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public IActionResult SaveCreate(SubscriptionViewModel subscription)
        {
            if (ModelState.IsValid)
            {
				Subscription subscriptionE = new Subscription();
				subscriptionE.DurationDays = subscription.DurationDays;
 				subscriptionE.MaxCourses = subscription.MaxCourses;
				subscriptionE.Name = subscription.Name;
				subscriptionE.Description = subscription.Description;
				subscriptionE.Price = subscription.Price;
				subscriptionE.Type = subscription.Type;
                subscriptionE.IsActive = subscription.IsActive;

				_subscriptionRepository.SubscriptionAdd(subscriptionE);
                _subscriptionRepository.Save();
                return RedirectToAction("ShowAll");
            }
            return View(nameof(Create), subscription);
        }


        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            Subscription subscriptionEdit = _subscriptionRepository.ShowDetails(id);
			EditSubscriptionViewModel subscriptionViewModel = new EditSubscriptionViewModel
			{
                Id = subscriptionEdit.Id,
				DurationDays = subscriptionEdit.DurationDays,
                MaxCourses = subscriptionEdit.MaxCourses,
                Name = subscriptionEdit.Name,
                Description = subscriptionEdit.Description,
                Price = subscriptionEdit.Price,
                Type = subscriptionEdit.Type,
                IsActive = subscriptionEdit.IsActive 
			};

			return View(subscriptionViewModel);
        }
        [HttpPost]
        public IActionResult SaveEdit(EditSubscriptionViewModel subscriptionSent, Guid id)
        {
            Subscription Oldsubscription = _subscriptionRepository.ShowDetails(id);
            if (ModelState.IsValid)
            {
                Oldsubscription.DurationDays = subscriptionSent.DurationDays;
                Oldsubscription.MaxCourses = subscriptionSent.MaxCourses;
                Oldsubscription.Name = subscriptionSent.Name;
                Oldsubscription.Description = subscriptionSent.Description;
                Oldsubscription.Price = subscriptionSent.Price;
                Oldsubscription.Type = subscriptionSent.Type;
                Oldsubscription.IsActive = subscriptionSent.IsActive;

				_subscriptionRepository.Update(Oldsubscription);
                _subscriptionRepository.Save();
                return RedirectToAction(nameof(ShowAll));
            }
            return View(subscriptionSent);
        }


        public IActionResult Delete(Guid id)
        {
            Subscription subscriptionDelete = _subscriptionRepository.ShowDetails(id);
            return View("Delete", subscriptionDelete);
        }
        public IActionResult SaveDelete(Guid id)
        {
            Subscription subscriptionDelete = _subscriptionRepository.ShowDetails(id);
            if (subscriptionDelete != null)
            {
                _subscriptionRepository.DeleteFromDB(subscriptionDelete);
                _subscriptionRepository.Save();
                return RedirectToAction(nameof(ShowAll));
            }
            return NotFound();
        }
    
    
    }

}
