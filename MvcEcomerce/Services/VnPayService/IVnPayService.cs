using MvcEcomerce.Application.VnPay;

namespace MvcEcomerce.Services.VnPayService
{
    public interface IVnPayService
    {
        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        public VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
