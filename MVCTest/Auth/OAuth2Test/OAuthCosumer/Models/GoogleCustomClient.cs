using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using System.Collections.Generic;

namespace OAuthCosumer.Models
{
    public class GoogleCustomClient : OpenIdClient
    {
        public GoogleCustomClient()
            : base("Custom", "http://www.wangcl.com:8001/user/bob/")
        {
        }

        protected override Dictionary<string, string> GetExtraData(IAuthenticationResponse response)
        {
            var fetchResponse = response.GetExtension<FetchResponse>();
            if (fetchResponse != null)
            {
                var extraData = new Dictionary<string, string>
                    {
                        {"email", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.Email)},
                        {"country", fetchResponse.GetAttributeValue(WellKnownAttributes.Contact.HomeAddress.Country)},
                        {"firstName", fetchResponse.GetAttributeValue(WellKnownAttributes.Name.First)},
                        {"lastName", fetchResponse.GetAttributeValue(WellKnownAttributes.Name.Last)}
                    };

                return extraData;
            }

            return null;
        }

        protected override void OnBeforeSendingAuthenticationRequest(IAuthenticationRequest request)
        {
            var fetchRequest = new FetchRequest();
            fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
            fetchRequest.Attributes.AddRequired(WellKnownAttributes.Contact.HomeAddress.Country);
            fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.First);
            fetchRequest.Attributes.AddRequired(WellKnownAttributes.Name.Last);

            request.AddExtension(fetchRequest);
        }
    }
}