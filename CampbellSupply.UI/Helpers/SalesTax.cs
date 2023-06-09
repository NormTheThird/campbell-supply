using CampbellSupply.DataLayer.DataContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CampbellSupply.Helpers
{
    /// <summary>
    /// From Tami in an email
    /// The tax laws are based on “where possession is taken.”
    /// ** Orders shipped to a SD or IA address… must charge sales tax because we have a physical presence in those states.
    /// ** Rules should be based on “shipping” not billing.
    /// -	Sales Tax is charged on merchandise and shipping – on SD or IA shipments.
    /// -	Gift Cards – Do not charge sales tax on the Gift Card Purchase – at any time.
    /// -	Shipping on Gift Cards – Must be taxed… if Gift Card is delivered to a SD or IA address.
    /// </summary>
    public static class SalesTax
    {
        public static decimal Calculate(List<ShoppingCartModel> cart, string shippingState)
        {
            try
            {
                if (string.IsNullOrEmpty(shippingState)) return 0.0m;
                var totalNonGiftCardAmount = cart.Where(c => !c.Name.Contains("Gift")).Select(c => c.Price).DefaultIfEmpty().Sum();
                var totalNonGiftCardShippingAmount = cart.Where(c => !c.Name.Contains("Gift")).Select(c => c.ShippingPrice).DefaultIfEmpty().Sum();
                var totalGiftCardAmount = cart.Where(c => c.Name.Contains("Gift")).Select(c => c.Price).DefaultIfEmpty().Sum();
                var TotalGiftCardShippingAmount = cart.Where(c => c.Name.Contains("Gift")).Select(c => c.ShippingPrice).DefaultIfEmpty().Sum();
                var totalTaxableAmount = TotalGiftCardShippingAmount + totalNonGiftCardAmount + totalNonGiftCardShippingAmount;

                // If biiling address is in IA or SD then charge tax on item and not on shipping
                if (shippingState.ToUpper().Trim().Equals("IOWA")) return Math.Round((totalTaxableAmount * 0.07m), 2, MidpointRounding.AwayFromZero);
                if (shippingState.ToUpper().Trim().Equals("SOUTH DAKOTA")) return Math.Round((totalTaxableAmount * 0.065m), 2, MidpointRounding.AwayFromZero);
                return 0.0m;
            }
            catch (Exception)
            {
                return 0.0m;
            }
        }

        public static decimal Calculate(List<OrderItemModel> orderItems, string shippingState)
        {
            try
            {
                if (string.IsNullOrEmpty(shippingState)) return 0.0m;
                var totalNonGiftCardAmount = orderItems.Where(o => !o.Name.Contains("Gift")).Select(o => o.Price).DefaultIfEmpty().Sum();
                var totalNonGiftCardShippingAmount = orderItems.Where(o => !o.Name.Contains("Gift")).Select(o => o.ShippingPrice).DefaultIfEmpty().Sum();
                var totalGiftCardAmount = orderItems.Where(o => o.Name.Contains("Gift")).Select(o => o.Price).DefaultIfEmpty().Sum();
                var TotalGiftCardShippingAmount = orderItems.Where(o => o.Name.Contains("Gift")).Select(o => o.ShippingPrice).DefaultIfEmpty().Sum();
                var totalTaxableAmount = TotalGiftCardShippingAmount + totalNonGiftCardAmount + totalNonGiftCardShippingAmount;

                // If shipping address is in IA or SD then charge tax on item and not on shipping
                if (shippingState.ToUpper().Trim().Equals("IOWA")) return Math.Round((totalTaxableAmount * 0.07m), 2, MidpointRounding.AwayFromZero);
                if (shippingState.ToUpper().Trim().Equals("SOUTH DAKOTA")) return Math.Round((totalTaxableAmount * 0.065m), 2, MidpointRounding.AwayFromZero);
                return 0.0m;
            }
            catch (Exception)
            {
                return 0.0m;
            }
        }
    }
}