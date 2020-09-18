using Sprint33.Areas.Store.Models;
using Sprint33.PharmacyEntities;
using System.Web;
using System.Web.Mvc;

namespace Sprint33.Services.Interfaces
{
    public interface ICustomerOrderRepository
    {
        void InitializeOrder(int patientId, Controller controller);
        CustomerOrder GetOrder(int orderId);
        int GetOrderId(HttpContextBase controller);
        void ApplyCoupon(Coupon coupon, CustomerOrder order);
        string GetCheckoutUrl(int orderId, int patientId);
        string PaymentApproved(HttpContextBase controller);
        string PaymentDeclined(HttpContextBase controller);
        void UpdateBillingInfo(BillingForm model);

    }
}