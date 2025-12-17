using System.Text.Json;
using Core.Enums;
using Core.Interfaces;
using Core.Models.Subscriptions;
using Core.Models.Users;
using Infrastructure.Services.Payment;
using Infrastructure.Services.Subscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skillup_Academy.ViewModels.PaymentViewModels;
using X.Paymob.CashIn;
using X.Paymob.CashIn.Models.Callback;

namespace Skillup_Academy.Controllers.Subscriptions
{		
	[Authorize]
	public class PaymentController : Controller
	{
		private readonly IRepository<Subscription> _repoSubscription;
		private readonly IRepository<SubscriptionPlan> _repoSubscriptionPlan;
		private readonly PaymentByPaymobCustom _paymentByPaymobCustom;
		private readonly PaymentByPaymob _paymentByPaymob;
		private readonly UserManager<User> _userManager;
		private readonly IPaymobCashInBroker _broker;

		public PaymentController(IRepository<Subscription> repoSubscription,
			PaymentByPaymobCustom paymentByPaymobCustom, PaymentByPaymob paymentByPaymob,
			UserManager<User> userManager, IPaymobCashInBroker broker, IRepository<SubscriptionPlan> repoSubscriptionPlan)
		{
			_repoSubscription = repoSubscription;
			_paymentByPaymobCustom = paymentByPaymobCustom;
			_paymentByPaymob = paymentByPaymob;
			_userManager = userManager;
			_broker = broker;
			_repoSubscriptionPlan = repoSubscriptionPlan;
		}

		[HttpGet]
 		public async Task<IActionResult> Checkout(Guid planId)
		{
			var plan =await _repoSubscription.GetByIdAsync(planId);
			if (plan == null) return NotFound();


			var vm = new CheckoutViewModel
			{
				PlanId = plan.Id,
				Name = plan.Name,
				Description = plan.Description,
				Price = plan.Price,
				DurationDays = plan.DurationDays,
				MaxCourses = plan.MaxCourses
			};


			return View(vm);
		}
		 

		[HttpPost]
		public async Task<IActionResult> Processing(CheckoutViewModel checkoutVM) 
		{
			var user = _userManager.GetUserAsync(User).Result;
			if (user == null)
				return RedirectToAction("Login","Account");

			var plan =await _repoSubscription.GetByIdAsync(checkoutVM.PlanId);

			if (checkoutVM.TypeMethod == PaymentMethod.Paymob.ToString())
			{
 				var subscriptionPlan = new SubscriptionPlan
				{ 
					StartDate = DateTime.Now,
					EndDate = DateTime.Now.AddDays(plan.DurationDays),
 					PaidAmount =plan.Price,
					MaxCourses = plan.MaxCourses,
					TransactionId = checkoutVM.PlanId.ToString(),
					IsActive = true,
					SubscriptionId = checkoutVM.PlanId,
					UserId = user.Id
				};
				await _repoSubscriptionPlan.AddAsync(subscriptionPlan);
				//var paymentUrl = await _paymentByPaymob.StartPaymentAsync(checkoutVM.Price,user);
				var paymentUrl = await _paymentByPaymobCustom.StartPaymentAsync(checkoutVM.Price,user,subscriptionPlan.Id.ToString());

				TempData["SubscriptionId"] = plan.Id.ToString();

				await _repoSubscriptionPlan.SaveChangesAsync();
				return Redirect(paymentUrl);	
			} 

			return RedirectToAction("Checkout",checkoutVM);
		}

		[AllowAnonymous]
		[HttpPost]
 		public async Task<IActionResult> PaymobCallback([FromQuery] string hmac, [FromBody] CashInCallback callbackData)
		{
			if (callbackData.Type == CashInCallbackTypes.Transaction)
			{
				// استخرج بيانات المعاملة من جسم الطلب (JSON)
				var rawJson = ((JsonElement)callbackData.Obj).GetRawText();
				var transaction = JsonSerializer.Deserialize<CashInCallbackTransaction>(rawJson);

				// كده ترجع المعرف اللي انت بعته
				string subscriptionPlanId = transaction.Order.MerchantOrderId?.ToString();
				Guid subscriptionPlanIdGuid = Guid.Parse(subscriptionPlanId);
				 

				bool valid = _broker.Validate(transaction, hmac);
				if(!valid)
					return BadRequest();

				 
				var subscriptionPlan = await _repoSubscriptionPlan.GetByIdAsync(subscriptionPlanIdGuid);

				if (subscriptionPlan != null)
				{
					if (transaction.Success)
					{
						var user = await _userManager.FindByIdAsync(subscriptionPlan.UserId.ToString());

						user.CanViewPaidCourses = true;
						await _userManager.UpdateAsync(user);
						await _repoSubscriptionPlan.SaveChangesAsync();
 				 
						return Ok(); 
					}
					else
					{
						_repoSubscriptionPlan.Delete(subscriptionPlan);
						await _repoSubscriptionPlan.SaveChangesAsync();

						return BadRequest();
					}

				}

				return BadRequest();
			}
			return BadRequest();
		}


		[AllowAnonymous]
		public IActionResult PaymobResult()
		{
 			var successParam = Request.Query["success"].ToString();
			var orderId = Request.Query["order"].ToString();  
			var message = Request.Query["data.message"].ToString();

 			bool isSuccess = successParam == "true";

			var subscriptionId = TempData["SubscriptionId"];
			
			if (isSuccess)
			{
				return RedirectToAction("Success");
			}

 			return RedirectToAction("Failure", new { id = subscriptionId });
		}


		[AllowAnonymous]
		public IActionResult Success()
		{
			return View(); 
		}
		[AllowAnonymous]
		public IActionResult Failure(Guid id)
		{
			return View(id);  
		}








		//[HttpPost]
		//public async Task<IActionResult> Processing(CheckoutViewModel checkoutVM) 
		//{
		//	var user = _userManager.GetUserAsync(User).Result;
		//	if (user == null)
		//		return RedirectToAction("Login","Account");
		//	var plan = _repoSubscription.GetByIdAsync(checkoutVM.PlanId);

		//	if (checkoutVM.TypeMethod == PaymentMethod.Paymob.ToString())
		//	{
		//		var paymentUrl = await _paymentByPaymob.StartPaymentAsync(checkoutVM.Price,user);
		//		return Redirect(paymentUrl);	
		//	}

		//	return RedirectToAction("Checkout",checkoutVM);
		//}


		//	[AllowAnonymous]
		//[HttpPost]
		//public async Task<IActionResult> PaymobCallback()
		//{
		//		try
		//	{
		//		Request.EnableBuffering();
		//	}
		//	catch
		//	{
		//		}

		//	using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
		//	var body = await reader.ReadToEndAsync();
		//		try { Request.Body.Position = 0; } catch { }

		//	try
		//	{
		//		using var doc = System.Text.Json.JsonDocument.Parse(body);
		//		var root = doc.RootElement;

		//			string merchantOrderId = "";
		//		if (root.TryGetProperty("merchant_order_id", out var moid))
		//			merchantOrderId = moid.GetString();

		//			string txnId = "";
		//		string status = "";
		//		if (root.TryGetProperty("transaction", out var tx))
		//		{
		//			if (tx.TryGetProperty("id", out var idProp)) txnId = idProp.GetRawText().Trim('"');
		//			if (tx.TryGetProperty("status", out var stProp)) status = stProp.GetRawText().Trim('"');
		//		}

		//			if (!string.IsNullOrEmpty(merchantOrderId))
		//		{
		//			var user = await _userManager.GetUserAsync(User);
		//			user.CanViewPaidCourses = true;

		//			await _userManager.UpdateAsync(user);

		//		}

		//		return Ok();
		//	}
		//	catch (System.Text.Json.JsonException)
		//	{
		//			return BadRequest("invalid json");
		//	}
		//	catch (Exception ex)
		//	{
		//				return StatusCode(500);
		//	}
		//}


		//[AllowAnonymous]
		//[HttpGet]
		//public IActionResult PaymentResult()
		//{ 
		//	return View();
		//}




	}
}
