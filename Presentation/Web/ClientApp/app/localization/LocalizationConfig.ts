import { Injectable } from "@angular/core";
import { LocaleService, TranslationService } from "angular-l10n";

@Injectable()
export class LocalizationConfig {

    constructor(public locale: LocaleService, public translation: TranslationService) { }

    load(): Promise<any> {
        this.locale.AddConfiguration()
            .AddLanguages(["en", "ru"])
            .SetCookieExpiration(30)
            .DefineLanguage("en");
        this.locale.init();

        this.translation.AddConfiguration()
            .AddProvider("./assets/locale-");

        let promise: Promise<any> = new Promise((resolve: any) => {
            this.translation.translationChanged.subscribe(() => {
                resolve(true);
            });
        });

        this.translation.init();

        return promise;
    }

}

// AoT compilation requires a reference to an exported function.
export function initLocalization(localizationConfig: LocalizationConfig): Function {
    return () => localizationConfig.load();
}