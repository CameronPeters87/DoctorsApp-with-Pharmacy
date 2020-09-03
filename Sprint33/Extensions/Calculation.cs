using System;

namespace Sprint33.Areas.Pharmacist.HelperMethods
{
    public static class Calculation
    {
        public static float CalculateVatOnProduct(float sellingPrice, float quantity, float taxRate)
        {
            float val = sellingPrice * quantity * taxRate;
            return (float) Math.Round(val, 2);
        }
    }
}