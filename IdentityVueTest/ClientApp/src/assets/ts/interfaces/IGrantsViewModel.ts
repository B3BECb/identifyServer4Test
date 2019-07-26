interface IGrantsViewModel
{
	 Grants: IGrantViewModel[];
}

interface IGrantViewModel
{
	ClientId: string;
	ClientName: string;
	ClientUrl: string;
	ClientLogoUrl: string;
	Created: string;
	Expires: string | null;
	IdentityGrantNames: string[];
	ApiGrantNames: string[];
}
