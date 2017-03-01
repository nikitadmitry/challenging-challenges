import {Injectable} from "@angular/core";
import {TranslationService} from "angular-l10n";

@Injectable()
export class EnumSelectService {
    constructor(private translation: TranslationService) { }

    public convertToSelectValues(e: any, prefix?: string): any[] {
        return Object.keys(e).map(k => e[k]).filter(v => typeof v === "string")
            .map(_name => {
                return {
                    type: e[_name] as number,
                    name: this.translation.translate(prefix ? prefix + "." + _name : _name)
                };
            });
    }
}