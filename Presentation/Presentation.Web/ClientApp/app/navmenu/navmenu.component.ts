import { Component, ViewEncapsulation, Output, EventEmitter } from "@angular/core";

@Component({
    selector: "nav-menu",
    template: require("./navmenu.component.html"),
    styles: [require("./navmenu.component.css")],
    encapsulation: ViewEncapsulation.None
})
export class NavMenuComponent {
    @Output("onNavigated")
    onNavigatedEmitter: EventEmitter<void> = new EventEmitter<void>();

    onNavigated(): void {
        this.onNavigatedEmitter.emit();
    }
}
