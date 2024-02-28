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

        public const string TokenCookie = "JWTToken";

        public const string RoleAdmin = "ADMIN";

        public const string RoleCustomer = "CUSTOMER";
    }
}
