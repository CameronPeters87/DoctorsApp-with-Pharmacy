using Sprint33.PharmacyEntities;
using System.Collections.Generic;
using System.Web;

namespace Sprint33.Services.Interfaces
{
    public interface IShoppingCart
    {
        ShoppingCart GetCart(HttpContextBase httpContextBase);

        float GetTotalPrice();
        float GetTotalTax();
        int GetCount();
        IEnumerable<CustomerCart> GetCartItems();
        void AddToCart(Product product, int quantity, int patientId);
        void UpdateCart(Product product, int Quantity);
    }
}
