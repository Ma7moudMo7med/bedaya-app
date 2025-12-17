using Core.Interfaces;
using Core.Interfaces.Subscriptions;
using Core.Models.Subscriptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Skillup_Academy.ViewModels.SubscriptionsViewModels;

namespace Skillup_Academy.Controllers.Subscriptions
{
    public class SubscriptionPlanController : Controller
    {
        public readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
		private readonly IRepository<Subscription> _repoSubscription;

		public SubscriptionPlanController(ISubscriptionPlanRepository subscriptionPlanRepository,IRepository<Subscription> repoSubscription)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
			_repoSubscription = repoSubscription;
		}

        // /SubscriptionPlan/ShowAll
        public IActionResult ShowAll()
        {
            List<SubscriptionPlan> SubscriptionPlanList = _subscriptionPlanRepository.ShowAll();
            return View("ShowAll", SubscriptionPlanList);
        }
        public IActionResult ShowDetails(Guid id)
        {
            SubscriptionPlan SubscriptionPlanDetails = _subscriptionPlanRepository.ShowDetails(id);
            return View("ShowDetails", SubscriptionPlanDetails);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Subscriptions = new SelectList(await _repoSubscription.GetAllAsync(), "Id", "Name");
            return View();
        }

        public async Task<IActionResult> SaveCreate(SubscriptionPlanViewModel subscriptionplan)
        {
            if (ModelState.IsValid)
            {
                SubscriptionPlan subscriptionPlan = new SubscriptionPlan();
                subscriptionPlan.StartDate = subscriptionplan.StartDate;
                subscriptionPlan.EndDate = subscriptionplan.EndDate;
                subscriptionPlan.PaidAmount = subscriptionplan.PaidAmount;
                subscriptionPlan.TransactionId = subscriptionplan.TransactionId ?? " ";
                subscriptionPlan.IsActive = subscriptionplan.IsActive; 
                subscriptionPlan.SubscriptionId = subscriptionplan.SubscriptionId;
                _subscriptionPlanRepository.SubscriptionPlanAdd(subscriptionPlan);
                _subscriptionPlanRepository.Save();
                return RedirectToAction("ShowAll");
            }

			ViewBag.Subscriptions = new SelectList(await _repoSubscription.GetAllAsync(), "Id", "Name");
			return View(nameof(Create), subscriptionplan);
        }

        public IActionResult Edit(Guid id)
        {
            SubscriptionPlan subscriptionplanedit = _subscriptionPlanRepository.ShowDetails(id);
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(ShowAll));
            }
            return View("Edit", subscriptionplanedit);
        }
        public IActionResult SaveEdit(SubscriptionPlan subscriptionplansent, Guid id)
        {
            SubscriptionPlan Oldsubscriptionplan = _subscriptionPlanRepository.ShowDetails(id);
            if (ModelState.IsValid)
            {
                Oldsubscriptionplan.StartDate = subscriptionplansent.StartDate;
                Oldsubscriptionplan.EndDate = subscriptionplansent.EndDate;
                Oldsubscriptionplan.PaidAmount = subscriptionplansent.PaidAmount;
                Oldsubscriptionplan.TransactionId = subscriptionplansent.TransactionId;
                Oldsubscriptionplan.IsActive = subscriptionplansent.IsActive;
                _subscriptionPlanRepository.Update(Oldsubscriptionplan);
                _subscriptionPlanRepository.Save();
                return RedirectToAction(nameof(ShowAll));
            }
            return View("Edit", subscriptionplansent);
        }
        public IActionResult Delete(Guid id)
        {
            SubscriptionPlan subscriptionplandelete = _subscriptionPlanRepository.ShowDetails(id);
            return View("Delete", subscriptionplandelete);
        }
        public IActionResult SaveDelete(Guid id)
        {
            SubscriptionPlan subscriptionplandelete = _subscriptionPlanRepository.ShowDetails(id);
            if (subscriptionplandelete != null)
            {
                _subscriptionPlanRepository.DeleteFromDB(subscriptionplandelete);
                _subscriptionPlanRepository.Save();
                return RedirectToAction(nameof(ShowAll));
            }
            return NotFound();
        }
    }
}
