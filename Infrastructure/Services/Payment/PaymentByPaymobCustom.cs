using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Users;
using Microsoft.Extensions.Configuration;
using X.Paymob.CashIn;
using X.Paymob.CashIn.Models.Orders;
using X.Paymob.CashIn.Models.Payment;

namespace Infrastructure.Services.Payment
{
	public class PaymentByPaymobCustom
	{
		private readonly IPaymobCashInBroker _broker;
 		private readonly IConfiguration _config;
		public PaymentByPaymobCustom(IConfiguration config,IPaymobCashInBroker broker)
		{
 			_config = config;
			_broker = broker;
		}

		public async Task<string> StartPaymentAsync(decimal Price, User user , string subPlan)
		{
			var amountCents = (int)(Price * 100);
 			var orderRequest = CashInCreateOrderRequest.CreateOrder(amountCents,"EGP", subPlan);
 
			var orderResponse = await _broker.CreateOrderAsync(orderRequest);

			// داخل نفس الإجراء بعد الحصول على orderResponse
			var billingData = new CashInBillingData(
				firstName: user.FullName?.Split(" ")?.FirstOrDefault() ?? "NA",
				lastName: user.FullName?.Split(" ")?.Skip(1).FirstOrDefault() ?? "NA",
				phoneNumber: user.PhoneNumber ?? "0000000000",
				email: user.Email ?? "no-reply@example.com"
			);

 			int integrationId = _config.GetValue<int>("PaymobSettings:IntegrationId");
			int iframeId = _config.GetValue<int>("PaymobSettings:IframeId");

 			// إعداد طلب مفتاح الدفع
			var paymentKeyRequest = new CashInPaymentKeyRequest(
				integrationId: integrationId,
				orderId: orderResponse.Id,
				billingData: billingData,
				amountCents: amountCents
			);

 			// تنفيذ الطلب للحصول على مفتاح الدفع
			var paymentKeyResponse = await _broker.RequestPaymentKeyAsync(paymentKeyRequest);

			// بناء رابط iframe باستخدام معرف الـIframe (من إعدادات Paymob) والمفتاح المستلم
			string iframeSrc = _broker.CreateIframeSrc(
				iframeId: iframeId.ToString(),
				token: paymentKeyResponse.PaymentKey
			);


			// نحول المستخدم إلى صفحة تعرض هذا الـiframe (أو تمرير الرابط للـView)
 			return   iframeSrc;
 		}


		 

	}
}
