interface IConsentInputModel
{
	Button: string;

	ScopesConsented: string[];

	RememberConsent: boolean;

	ReturnUrl: string;
}

interface IConsentViewModel
	extends IConsentInputModel
{
	ClientName: string;

	ClientUrl: string;

	ClientLogoUrl: string;

	AllowRememberConsent: boolean;

	IdentityScopes: IScopeViewModel[];

	ResourceScopes: IScopeViewModel[];
}

interface IScopeViewModel
{
	Name: string;

	DisplayName: string;

	Description: string;

	Emphasize: boolean;

	Required: boolean;

	Checked: boolean;
}
