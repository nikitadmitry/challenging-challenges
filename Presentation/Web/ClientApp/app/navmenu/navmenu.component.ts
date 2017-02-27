import { Component, ViewEncapsulation, Output, EventEmitter } from "@angular/core";
import { Translation, TranslationService } from "angular-l10n";

@Component({
    selector: "nav-menu",
    template: require("./navmenu.component.html"),
    styles: [require("./navmenu.component.css")],
    encapsulation: ViewEncapsulation.None
})
export class NavMenuComponent extends Translation {
    @Output("onNavigated")
    onNavigatedEmitter: EventEmitter<void> = new EventEmitter<void>();
    flag: boolean = true;
    constructor(translationService: TranslationService) {
        super(translationService);
    }

    toggleFlag(): void {
        this.flag = !this.flag;
    }

    onNavigated(): void {
        this.onNavigatedEmitter.emit();
    }
}
