using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_models.Cloudinary
{
    public class AddImageDto
    {
        
        public IFormFile Image { get; set; }

    }
}
