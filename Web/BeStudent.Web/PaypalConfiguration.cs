namespace BeStudent.Web
{
    using System.Collections.Generic;

    using PayPal.Api;

    public static class PaypalConfiguration
    {
        public static readonly string ClientId;
        public static readonly string ClientSecret;

        static PaypalConfiguration()
        {
            //TODO::
            //var config = GetConfig();
            ClientId = "AewO0fN7c6GuMMO0fIQq_0qILiDOf4ASjy1cJTarVYmeNJPbePQ1mnV4g-HoTpvKMPShpl3q9x7hxILi";
            ClientSecret = "EGXduWBuSMzgSI5G7ikWf1fBUn42tOCPNzXMh1Li6yumI4AIQatBZ0NOCYrtqLD5fbm2Y8B18I-aveD7";
        }

        // getting properties from the web.config
        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            // getting accesstocken from paypal  
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }

        public static APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoke
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}
