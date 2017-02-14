import { Component, OnDestroy, ViewEncapsulation, ViewChild } from "@angular/core";
import { MdlLayoutComponent } from "angular2-mdl";
import { Translation, LocaleService, TranslationService } from "angular-l10n";

@Component({
    selector: "app",
    template: require("./app.component.html"),
    styles: [require("./app.component.css")],
    encapsulation: ViewEncapsulation.None
})
export class AppComponent extends Translation implements OnDestroy {
    @ViewChild(MdlLayoutComponent)
    layout: MdlLayoutComponent;

    public notificationOptions = {
        timeOut: 5000,
        theClass: "mdl-shadow--8dp",
    };

    constructor(public locale: LocaleService, public translation: TranslationService) {
        super(translation);

        this.locale.AddConfiguration()
            .AddLanguages(["en", "ru"])
            .SetCookieExpiration(30)
            .DefineLanguage("en");
        this.locale.init();

        this.translation.AddConfiguration()
            .AddProvider("./assets/locale-");
        this.translation.init();
    }

    selectLanguage(language: string): void {
        this.locale.setCurrentLanguage(language);
    }

    ngOnDestroy(): void {
        document.body.appendChild(document.createElement("app"));
    }

    onNavigated(): void {
        this.layout.closeDrawerOnSmallScreens();
    }
}
