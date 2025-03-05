using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebApi.Models;
using WooCommerceApi.Models.WooCommerceModels;

namespace WooCommerceApi.Utilities
{
    public class Constants
    {
        public static string CONSUMER_KEY = "ck_0b65361a66a4784f7fa6c1f7078e50f9e6521516";
        public static string CONSUMER_SECRET = "cs_8e1d1d6c0439c5a0d0452c886b1485b5bb505280";
        public static Dictionary<string, int> PaymentMethods = new Dictionary<string, int>
        {
            {"cod", 1},
            {"bacs", 2},
            {"cheque", 3},
            {"paypal", 4},
            {"stripe", 5},
            {"bankmellat", 6},
            {"WC_ZPal", 7},
            {"WC_IDPay", 8}
        };
        public static Dictionary<ProductTypes, string> ProductTypes = new Dictionary<ProductTypes, string>
        {
            {WebApi.Models.ProductTypes.Simple, "simple" },
            {WebApi.Models.ProductTypes.Variable, "variable" },
            {WebApi.Models.ProductTypes.Variation, "variation" },
        };
        public static Dictionary<OrderStatus, string> OrderStatuses = new Dictionary<OrderStatus, string>
        {
            {WebApi.Models.OrderStatus.Pending, "pending" },
            {WebApi.Models.OrderStatus.Processing, "processing" },
            {WebApi.Models.OrderStatus.OnHold, "on-hold" },
            {WebApi.Models.OrderStatus.Completed, "completed" },
            {WebApi.Models.OrderStatus.Cancelled, "cancelled" },
            {WebApi.Models.OrderStatus.Refunded, "refunded" },
            {WebApi.Models.OrderStatus.Failed, "failed" },
            {WebApi.Models.OrderStatus.Other, "others" }
        };
        public static string OrderPath => "wp-admin/admin.php?page=wc-orders&action=edit&id={0}";

        public static Dictionary<string, string> StateDictionary = new Dictionary<string, string>
        {
            { "KHZ", "Khuzestan (خوزستان)" },
            { "THR", "Tehran (تهران)" },
            { "ILM", "Ilam (ایلام)" },
            { "BHR", "Bushehr (بوشهر)" },
            { "ADL", "Ardabil (اردبیل)" },
            { "ESF", "Isfahan (اصفهان)" },
            { "YZD", "Yazd (یزد)" },
            { "KRH", "Kermanshah (کرمانشاه)" },
            { "KRN", "Kerman (کرمان)" },
            { "HDN", "Hamadan (همدان)" },
            { "GZN", "Ghazvin (قزوین)" },
            { "ZJN", "Zanjan (زنجان)" },
            { "LRS", "Luristan (لرستان)" },
            { "ABZ", "Alborz (البرز)" },
            { "EAZ", "East Azarbaijan (آذربایجان شرقی)" },
            { "WAZ", "West Azarbaijan (آذربایجان غربی)" },
            { "CHB", "Chaharmahal and Bakhtiari (چهارمحال و بختیاری)" },
            { "SKH", "South Khorasan (خراسان جنوبی)" },
            { "RKH", "Razavi Khorasan (خراسان رضوی)" },
            { "NKH", "North Khorasan (خراسان شمالی)" },
            { "SMN", "Semnan (سمنان)" },
            { "FRS", "Fars (فارس)" },
            { "QHM", "Qom (قم)" },
            { "KRD", "Kurdistan (کردستان)" },
            { "KBD", "Kohgiluyeh and Boyer-Ahmad (کهگیلویه و بویراحمد)" },
            { "GLS", "Golestan (گلستان)" },
            { "GIL", "Gilan (گیلان)" },
            { "MZN", "Mazandaran (مازندران)" },
            { "MKZ", "Markazi (مرکزی)" },
            { "HRZ", "Hormozgan (هرمزگان)" },
            { "SBN", "Sistan and Baluchestan (سیستان و بلوچستان)" }
        };
        public static Dictionary<string, string> CountryDictionary = new Dictionary<string, string>
        {
            { "AF", "Afghanistan" }, { "AX", "Åland Islands" }, { "AL", "Albania" },
            { "DZ", "Algeria" }, { "AS", "American Samoa" }, { "AD", "Andorra" },
            { "AO", "Angola" }, { "AI", "Anguilla" }, { "AQ", "Antarctica" },
            { "AG", "Antigua and Barbuda" }, { "AR", "Argentina" }, { "AM", "Armenia" },
            { "AW", "Aruba" }, { "AU", "Australia" }, { "AT", "Austria" },
            { "AZ", "Azerbaijan" }, { "BS", "Bahamas" }, { "BH", "Bahrain" },
            { "BD", "Bangladesh" }, { "BB", "Barbados" }, { "BY", "Belarus" },
            { "BE", "Belgium" }, { "PW", "Belau" }, { "BZ", "Belize" },
            { "BJ", "Benin" }, { "BM", "Bermuda" }, { "BT", "Bhutan" },
            { "BO", "Bolivia" }, { "BQ", "Bonaire, Saint Eustatius and Saba" }, { "BA", "Bosnia and Herzegovina" },
            { "BW", "Botswana" }, { "BV", "Bouvet Island" }, { "BR", "Brazil" },
            { "IO", "British Indian Ocean Territory" }, { "BN", "Brunei" }, { "BG", "Bulgaria" },
            { "BF", "Burkina Faso" }, { "BI", "Burundi" }, { "KH", "Cambodia" },
            { "CM", "Cameroon" }, { "CA", "Canada" }, { "CV", "Cape Verde" },
            { "KY", "Cayman Islands" }, { "CF", "Central African Republic" }, { "TD", "Chad" },
            { "CL", "Chile" }, { "CN", "China" }, { "CX", "Christmas Island" },
            { "CC", "Cocos (Keeling) Islands" }, { "CO", "Colombia" }, { "KM", "Comoros" },
            { "CG", "Congo (Brazzaville)" }, { "CD", "Congo (Kinshasa)" }, { "CK", "Cook Islands" },
            { "CR", "Costa Rica" }, { "HR", "Croatia" }, { "CU", "Cuba" },
            { "CW", "Curaçao" }, { "CY", "Cyprus" }, { "CZ", "Czech Republic" },
            { "DK", "Denmark" }, { "DJ", "Djibouti" }, { "DM", "Dominica" },
            { "DO", "Dominican Republic" }, { "EC", "Ecuador" }, { "EG", "Egypt" },
            { "SV", "El Salvador" }, { "GQ", "Equatorial Guinea" }, { "ER", "Eritrea" },
            { "EE", "Estonia" }, { "ET", "Ethiopia" }, { "FK", "Falkland Islands" },
            { "FO", "Faroe Islands" }, { "FJ", "Fiji" }, { "FI", "Finland" },
            { "FR", "France" }, { "GF", "French Guiana" }, { "PF", "French Polynesia" },
            { "TF", "French Southern Territories" }, { "GA", "Gabon" }, { "GM", "Gambia" },
            { "GE", "Georgia" }, { "DE", "Germany" }, { "GH", "Ghana" },
            { "GI", "Gibraltar" }, { "GR", "Greece" }, { "GL", "Greenland" },
            { "GD", "Grenada" }, { "GP", "Guadeloupe" }, { "GU", "Guam" },
            { "GT", "Guatemala" }, { "GG", "Guernsey" }, { "GN", "Guinea" },
            { "GW", "Guinea-Bissau" }, { "GY", "Guyana" }, { "HT", "Haiti" },
            { "HM", "Heard Island and McDonald Islands" }, { "HN", "Honduras" }, { "HK", "Hong Kong" },
            { "HU", "Hungary" }, { "IS", "Iceland" }, { "IN", "India" },
            { "ID", "Indonesia" }, { "IR", "Iran" }, { "IQ", "Iraq" },
            { "IE", "Ireland" }, { "IM", "Isle of Man" }, { "IL", "Israel" },
            { "IT", "Italy" }, { "CI", "Ivory Coast" }, { "JM", "Jamaica" },
            { "JP", "Japan" }, { "JE", "Jersey" }, { "JO", "Jordan" },
            { "KZ", "Kazakhstan" }, { "KE", "Kenya" }, { "KI", "Kiribati" },
            { "KW", "Kuwait" }, { "KG", "Kyrgyzstan" }, { "LA", "Laos" },
            { "LV", "Latvia" }, { "LB", "Lebanon" }, { "LS", "Lesotho" },
            { "LR", "Liberia" }, { "LY", "Libya" }, { "LI", "Liechtenstein" },
            { "LT", "Lithuania" }, { "LU", "Luxembourg" }, { "MO", "Macao" },
            { "MK", "North Macedonia" }, { "MG", "Madagascar" }, { "MW", "Malawi" },
            { "MY", "Malaysia" }, { "MV", "Maldives" }, { "ML", "Mali" },
            { "MT", "Malta" }, { "MH", "Marshall Islands" }, { "MQ", "Martinique" },
            { "MR", "Mauritania" }, { "MU", "Mauritius" }, { "YT", "Mayotte" },
            { "MX", "Mexico" }, { "FM", "Micronesia" }, { "MD", "Moldova" },
            { "MC", "Monaco" }, { "MN", "Mongolia" }, { "ME", "Montenegro" },
            { "MS", "Montserrat" }, { "MA", "Morocco" }, { "MZ", "Mozambique" },
            { "MM", "Myanmar" }, { "NA", "Namibia" }, { "NR", "Nauru" },
            { "NP", "Nepal" }, { "NL", "Netherlands" }, { "NC", "New Caledonia" },
            { "NZ", "New Zealand" }, { "NI", "Nicaragua" }, { "NE", "Niger" },
            { "NG", "Nigeria" }, { "NU", "Niue" }, { "NF", "Norfolk Island" },
            { "KP", "North Korea" }, { "NO", "Norway" }, { "OM", "Oman" },
            { "PS", "Palestinian Territory" }, { "PK", "Pakistan" }, { "PA", "Panama" },
            { "PG", "Papua New Guinea" }, { "PY", "Paraguay" }, { "PE", "Peru" },
            { "PH", "Philippines" }, { "PL", "Poland" }, { "PT", "Portugal" },
            { "PR", "Puerto Rico" }, { "QA", "Qatar" }, { "RO", "Romania" },
            { "RU", "Russia" }, { "RW", "Rwanda" }, { "SA", "Saudi Arabia" },
            { "ZA", "South Africa" }, { "ES", "Spain" }, { "LK", "Sri Lanka" },
            { "SE", "Sweden" }, { "CH", "Switzerland" }, { "TH", "Thailand" },
            { "TR", "Turkey" }, { "UA", "Ukraine" }, { "AE", "United Arab Emirates" },
            { "GB", "United Kingdom" }, { "US", "United States" }
        };
    }
}
