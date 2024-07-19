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
                // �������� ��������� �� ������, �� ��������� ���� � ���� �������
                RedirectToPage("/");
            }
            // �������� ����� � appsettings.json, �� �� �� ���� ����� � ����
            // ����� ��� �� ���������� � �������� ���������� � Configuration
            StripeConfiguration.ApiKey = "sk_test_51OtqA2EShR09FPvo3stfdGTJwMi9PDojuIe8asp32EB2PleRQ368znZaWlqSqnh94Ss2b4xDQM9pY1V52RdDgo1500Hxngq7Ti";
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(order.OrderDetails!.Sum(x => x.Quantity *
               x.UnitPrice)) * 100, // ��� � � ��������, ������ * 100
                Currency = "bgn",
                Description = "������� �� ����� #" + order.Id,
                AutomaticPaymentMethods = new
           PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            };
            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);
            this.StripeToken = paymentIntent.ClientSecret;
            // ��� ���� �� � ����� ���� �� ������� ���� ������ � ���������, �� ������ ����� �� � ������� �� ����
        }

    }
}
