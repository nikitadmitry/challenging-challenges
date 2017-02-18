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

    languageChanged: boolean = this.locale.getCurrentLanguage() !== "en";

    public notificationOptions = {
        timeOut: 5000,
        theClass: "mdl-shadow--8dp",
    };

    constructor(public locale: LocaleService, public translation: TranslationService) {
        super(translation);
    }

    toggleLanguage(): void {
        let language: string = this.languageChanged ? "ru" : "en";

        this.locale.setCurrentLanguage(language);
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
