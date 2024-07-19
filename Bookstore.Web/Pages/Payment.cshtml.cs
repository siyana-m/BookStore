using Bookstore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace Bookstore.Web.Pages
{
    public class PaymentModel : PageModel
    {
        private readonly OrdersService _os;
        public PaymentModel(OrdersService os)
        {
            _os = os;
        }
        [BindProperty]
        public string? StripeToken { get; set; }
        public async Task OnGetAsync()
        {
            var order = await _os.GetLatestOrderByUser(User!.Identity!.Name!);
            if (order.IsPaid)
            {
                // добавете показване на грешка, че поръчката вече е била платена
                RedirectToPage("/");
            }
            // добавете ключа в appsettings.json, за да не стои забит в кода
            // вижте как го направихме в предното упражнение с Configuration
            StripeConfiguration.ApiKey = "sk_test_51OtqA2EShR09FPvo3stfdGTJwMi9PDojuIe8asp32EB2PleRQ368znZaWlqSqnh94Ss2b4xDQM9pY1V52RdDgo1500Hxngq7Ti";
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(order.OrderDetails!.Sum(x => x.Quantity *
               x.UnitPrice)) * 100, // тук е в стотинки, затова * 100
                Currency = "bgn",
                Description = "Поръчка на книги #" + order.Id,
                AutomaticPaymentMethods = new
           PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            };
            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);
            this.StripeToken = paymentIntent.ClientSecret;
            // тук може да е добра идея да запазим този тоукън в поръчката, за даможе после да я открием по него
        }

    }
}
