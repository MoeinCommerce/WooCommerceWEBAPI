using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WooCommerceApi.Models.WooCommerceModels
{
    public class WooOrder
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }

        [JsonProperty("customer_note")]
        public string CustomerNote { get; set; }

        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }
        
        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty("payment_method_title")]
        public string PaymentMethodTitle { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("date_created_gmt")]
        public DateTime? DateCreated { get; set; }
        
        [JsonProperty("date_modified_gmt")]
        public DateTime? DateModified { get; set; }
        
        [JsonProperty("currency")]
        public string Currency { get; set; }
        
        [JsonProperty("shipping_total")]
        public double ShippingTotal { get; set; }
        
        [JsonProperty("total")]
        public double Total { get; set; }
        
        [JsonProperty("total_tax")]
        public double TotalTax { get; set; }
        
        [JsonProperty("billing")]
        public WooCustomer Billing { get; set; }
        
        [JsonProperty("shipping")]
        public WooCustomer Shipping { get; set; }
        
        [JsonProperty("line_items")]
        public List<WooOrderLineItem> LineItems { get; set; }
        
        [JsonProperty("shipping_lines")]
        public List<WooOrderShippingLine> ShippingLines { get; set; }
        
        [JsonProperty("fee_lines")]
        public List<WooOrderFeeLine> FeeLines { get; set; }
        
        [JsonProperty("coupon_lines")]
        public List<WooOrderCouponLine> CouponLines { get; set; }
        
        [JsonProperty("tax_lines")] 
        public List<WooOrderTaxLine> TaxLines { get; set; }

    }
    public class WooOrderLineItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("product_id")]
        public int ProductId { get; set; }
        
        [JsonProperty("variation_id")]
        public int VariationId { get; set; }
        
        [JsonProperty("quantity")]
        public double Quantity { get; set; }
        
        [JsonProperty("tax_class")]
        public string TaxClass { get; set; }
        
        [JsonProperty("subtotal")]
        public double Subtotal { get; set; }
        
        [JsonProperty("subtotal_tax")]
        public double SubtotalTax { get; set; }
        
        [JsonProperty("total")]
        public double Total { get; set; }
        
        [JsonProperty("total_tax")]
        public double TotalTax { get; set; }
        
        [JsonProperty("price")]
        public double Price { get; set; }
        
        [JsonProperty("taxes")]
        public List<WooOrderTax> Taxes { get; set; }
    }

    public class WooOrderTax
    {
        public int Id { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }
        public string Subtotal { get; set; }
    }

    public class WooOrderShippingLine
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("method_title")]
        public string MethodTitle { get; set; }

        [JsonProperty("method_id")]
        public string MethodId { get; set; }

        [JsonProperty("total")]
        public string Total { get; set; }

        [JsonProperty("total_tax")]
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

        [JsonProperty("rate_percent")]
        public int RatePercent { get; set; }
    }
}
