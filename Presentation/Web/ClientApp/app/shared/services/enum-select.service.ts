import {Injectable} from "@angular/core";
import {TranslationService} from "angular-l10n";

@Injectable()
export class EnumSelectService {
    constructor(private translation: TranslationService) { }

    public convertToSelectValues(e: any, prefix?: string): any[] {
        return Object.keys(e).map(k => e[k]).filter(v => typeof v === "string")
            .map(_name => {
                return {
                    value: e[_name] as number,
                    name: this.translation.translate(prefix ? `${prefix}.${prefix}-${e[_name]}` : _name)
                };
            });
    }
}