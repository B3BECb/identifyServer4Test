import { upperFirst } from "lodash";
export default function CapitalizeKeys(obj) {
    if (Array.isArray(obj)) {
        return obj.map((v) => CapitalizeKeys(v));
    }
    else {
        if (obj !== null && obj.constructor === Object) {
            return Object.keys(obj).reduce((result, key) => ({
                ...result,
                [upperFirst(key)]: CapitalizeKeys(obj[key]),
            }), {});
        }
    }
    return obj;
}
//# sourceMappingURL=KeysCapitalizer.js.map