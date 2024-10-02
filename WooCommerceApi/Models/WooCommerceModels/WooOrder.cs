using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooCommerceApi.Models.WooCommerceModels
{
    public class WooOrder
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Currency { get; set; }
        public string Total { get; set; }
        public string TotalTax { get; set; }
        public WooCustomer Billing { get; set; }
        public WooCustomer Shipping { get; set; }
        public List<WooOrderLineItem> LineItems { get; set; }
        public List<WooOrderShippingLine> ShippingLines { get; set; }
        public List<WooOrderFeeLine> FeeLines { get; set; }
        public List<WooOrderCouponLine> CouponLines { get; set; }
        public List<WooOrderTaxLine> TaxLines { get; set; }
    }

    public class WooCustomer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class WooOrderLineItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public int VariationId { get; set; }
        public int Quantity { get; set; }
        public string TaxClass { get; set; }
        public string Subtotal { get; set; }
        public string SubtotalTax { get; set; }
        public string Total { get; set; }
        public string TotalTax { get; set; }
        public List<WooOrderTax> Taxes { get; set; }
    }

    public class WooOrderTax
    {
        public int Id { get; set; }
        public string Total { get; set; }
        public string Subtotal { get; set; }
    }

    public class WooOrderShippingLine
    {
        public int Id { get; set; }
        public string MethodTitle { get; set; }
        public string MethodId { get; set; }
        public string Total { get; set; }
        public string TotalTax { get; set; }
    }

    public class WooOrderFeeLine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TaxClass { get; set; }
        public string TaxStatus { get; set; }
        public string Total { get; set; }
        public string TotalTax { get; set; }
        public List<WooOrderTax> Taxes { get; set; }
    }

    public class WooOrderCouponLine
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Discount { get; set; }
        public string DiscountTax { get; set; }
    }

    public class WooOrderTaxLine
    {
        public int Id { get; set; }
        public string RateCode { get; set; }
        public string RateId { get; set; }
        public string Label { get; set; }
        public bool Compound { get; set; }
        public string TaxTotal { get; set; }
        public string ShippingTaxTotal { get; set; }
    }
}
