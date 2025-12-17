using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Models.Users;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.Payment
{
	public class PaymentByPaymob
	{
		private readonly IHttpClientFactory _httpFactory;
		private readonly IConfiguration _config;
		public PaymentByPaymob(IHttpClientFactory httpFactory, IConfiguration config)
		{
			_httpFactory = httpFactory;
			_config = config;
		}
		public async Task<string> StartPaymentAsync(decimal Price, User user) 
		{
 			var paymobApiKey = _config["PaymobSettings:ApiKey"];          
			var iframeId = _config["PaymobSettings:IframeId"];             
			var integrationId = _config["PaymobSettings:IntegrationId"];   
			var baseUrl = _config["PaymobSettings:BaseUrl"];   

			var client = _httpFactory.CreateClient();
			client.BaseAddress =new Uri(baseUrl??"https://staging.paymob.com/api");

			// 1) Auth -> get token
			var authBody = new { api_key = paymobApiKey };
			var authResp = await client.PostAsync("api/auth/tokens",
				new StringContent(JsonSerializer.Serialize(authBody), Encoding.UTF8, "application/json"));


			if (!authResp.IsSuccessStatusCode)
				return "Paymob auth failed";


			var authJson = JsonSerializer.Deserialize<JsonElement>(await authResp.Content.ReadAsStringAsync());
			var token = authJson.GetProperty("token").GetString();
			var merchantId = authJson.GetProperty("profile").GetProperty("id").GetInt32();

			// 2) Register order (amount in cents/piasters, e.g. EGP -> multiply by 100)
			var amountCents = (int)(Price * 100); // ensure decimals -> int
			var orderBody = new
			{
				auth_token = token,
				delivery_needed = false,
				amount_cents = amountCents,
				currency = "EGP",   // أو USD حسب حسابك
				merchant_order_id = Guid.NewGuid().ToString(), // حفظ هذا في DB لو احتجت
				items = new object[] { } // أو أضف عناصر إذا أردت
			};


			var orderResp = await client.PostAsync("api/ecommerce/orders",
				new StringContent(JsonSerializer.Serialize(orderBody), Encoding.UTF8, "application/json"));

			if (!orderResp.IsSuccessStatusCode)
				return "Paymob order registration failed";


			var orderJson = JsonSerializer.Deserialize<JsonElement>(await orderResp.Content.ReadAsStringAsync());
			var paymobOrderId = orderJson.GetProperty("id").GetInt32();

			// احفظ paymobOrderId مع الطلب في DB هنا (مهم)

			// 3) Request payment_key
			var billingData = new
			{
				first_name = user.FullName?.Split(" ")?.FirstOrDefault() ?? "NA",
				last_name = user.FullName?.Split(" ")?.Skip(1).FirstOrDefault() ?? "NA",
				 
				email = user.Email ?? "no-reply@example.com",
				phone_number = user.PhoneNumber ?? "0000000000",
				apartment = "NA",
				floor = "NA",
				street = "NA",
				building = "NA",

				city = "Cairo",
				country = "EG",
				state = "Cairo",
				postal_code = "00000"
			};
 
			var paymentKeyBody = new
			{
				auth_token = token,
				amount_cents = amountCents,
				expiration = 3600,
				order_id = paymobOrderId,
				billing_data = billingData,
				integration_id = int.Parse(integrationId),
				currency = "EGP"   
			};

			var pkResp = await client.PostAsync("api/acceptance/payment_keys",
				new StringContent(JsonSerializer.Serialize(paymentKeyBody), Encoding.UTF8, "application/json"));
			var respContent = await pkResp.Content.ReadAsStringAsync();
			 
			if (!pkResp.IsSuccessStatusCode)
				return "Paymob payment_key request failed";

			var pkJson = JsonSerializer.Deserialize<JsonElement>(await pkResp.Content.ReadAsStringAsync());
			var paymentKey = pkJson.GetProperty("token").GetString();

			// 4) Redirect to iframe page (مثال: نعرض View يحتوي iframe)
			var iframeUrl = $"https://accept.paymob.com/api/acceptance/iframes/{iframeId}?payment_token={paymentKey}";

			// Option A: Redirect user directly
			return iframeUrl;

			// Option B: Or return a view that contains an <iframe src="iframeUrl"></iframe>
			// return View("PaymobIframe", model: iframeUrl);
		}
	}
}
 