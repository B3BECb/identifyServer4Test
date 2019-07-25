interface IExternalProvider
{
	DisplayName: string;
	AuthenticationScheme: string;
}

interface ILoginInputModel
{
	Username: string;
	Password: string;
	RememberLogin: boolean;
	ReturnUrl: string;
}

interface ILoginViewModel
	extends ILoginInputModel
{
	AllowRememberLogin: boolean;
	EnableLocalLogin: boolean;

	ExternalProviders: IExternalProvider[];
	VisibleExternalProviders: IExternalProvider[];

	IsExternalLoginOnly: boolean;
	ExternalLoginScheme: string;
}
