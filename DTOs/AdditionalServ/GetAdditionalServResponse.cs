using System;
using System.Collections.Generic;

namespace ZooMag.DTOs.AdditionalServ
{
    public class GetAdditionalServResponse
    {
        public int Id { get; set; }
        public string ServName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string ContentText { get; set; }
        public List<GetServImageResponse> ServImages { get; set; }
    }
}