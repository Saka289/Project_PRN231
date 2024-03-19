using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Enums.SD;

namespace Shared.Dtos
{
    public class SendRequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;

        public string Url { get; set; }

        public object Data { get; set; }

        public ContentType ContentType { get; set; } = ContentType.Json;
    }
}
