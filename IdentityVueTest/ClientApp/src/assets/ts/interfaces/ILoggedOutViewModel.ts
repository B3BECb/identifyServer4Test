interface ILoggedOutViewModel
{
	PostLogoutRedirectUri: string;

	ClientName: string;

	SignOutIframeUrl: string;

	AutomaticRedirectAfterSignOut: boolean;

	LogoutId: string;

	TriggerExternalSignout: boolean;

	ExternalAuthenticationScheme: string;
}
