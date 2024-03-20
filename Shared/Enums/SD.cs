using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Enums
{
    public class SD
    {
        public enum ApiType
        {
            GET, POST, PUT, DELETE
        }

        public enum ContentType
        {
            Json,
            MultipartFormData,
        }

        public enum PaymentStatus
        {
            NOT_STARTED,
            IN_PROGRESS,
            COMPLETED,
            REFUND,
        }

        public const string TokenCookie = "JWTToken";

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";

        public const string AuthAPIBase = "https://localhost:6001";
        public const string CouponAPIBase = "https://localhost:6002";
        public const string EmailAPIBase = "https://localhost:6003";
        public const string InventoryAPIBase = "https://localhost:6004";
        public const string OrderAPIBase = "https://webservicesorderapi.azurewebsites.net";
        public const string PaymentAPIBase = "https://webservicespaymentapi.azurewebsites.net";
        public const string ProductAPIBase = "https://localhost:6007";
        public const string ShoppingCartAPIBase = "https://localhost:6008";

        public const string BaseUrlGateWay = "https://localhost:6666";
    }
}
