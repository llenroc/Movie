using System.Runtime.Serialization;

namespace Infrastructure
{
    public class WebResult:Result
    {
        public WebResult(bool status, string message): base(status, message)
        {
        }

        public string returnUrl { get; set; }
    }
}
