using System.Collections.Generic;

namespace ZooMag.DTOs.AdditionalServ
{
    public class CreateAdditionalServRequest
    {
        public string ServName { get; set; }
        public string ContentText { get; set; }
        public virtual IEnumerable<CreateServImageRequest> ServImages { get; set; }
    }
}