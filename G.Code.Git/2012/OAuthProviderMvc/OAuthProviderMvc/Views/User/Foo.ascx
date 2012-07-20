<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Register Assembly="DotNetOpenAuth.OpenId.Provider.UI" Namespace="DotNetOpenAuth.OpenId.Provider"
	TagPrefix="op" %>

	<%=Html.Encode(ViewData["username"] ?? string.Empty)%>
	Identity page

	<op:IdentityEndpoint ID="IdentityEndpoint11" runat="server" ProviderEndpointUrl="~/OpenId/Provider"
		ProviderVersion="V11" />
	<op:IdentityEndpoint ID="IdentityEndpoint20" runat="server" ProviderEndpointUrl="~/OpenId/Provider"
		XrdsUrl="~/User/all/xrds" XrdsAutoAnswer="false" XrdsAdvertisement="Both" />


	<h2>
		<% if (!string.IsNullOrEmpty(ViewData["username"] as string)) { %>
		This is
		<%=Html.Encode(ViewData["username"])%>'s
		<% } %>
		OpenID identity page
	</h2>
	<% if (string.Equals(Context.User.Identity.Name, ViewData["username"])) { %>
	<p>
		This is <b>your</b> identity page.
	</p>
	<% } %>
