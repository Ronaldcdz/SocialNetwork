using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Dtos.Accounnts
{
    public class ForgotPasswordResponse
    {
        public bool HasError{ get; set; }
        public string? Error { get; set; }
    }
}
