namespace NET.Starter.Core.Services.Security.Inputs
{
    public class GoogleLoginInput
    {
        /// <summary>
        /// This token being passed here is generated from the client side when a request is made  to 
        /// i.e. react, angular, flutter etc. It is being returned as A jwt from google oauth server. 
        /// </summary>
        public required string IdToken { get; set; }
    }
}
