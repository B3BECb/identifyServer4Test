import { upperFirst } from "lodash";

export default function CapitalizeKeys(obj: any): any
{
	if(Array.isArray(obj))
	{
		return obj.map((v: any) => CapitalizeKeys(v));
	}
	else
	{
		if(obj !== null && obj.constructor === Object)
		{
			return Object.keys(obj).reduce(
				(result, key) => ({
					...result,
					[upperFirst(key)]: CapitalizeKeys(obj[key]),
				}),
				{},
			);
		}
	}
	return obj;
}
