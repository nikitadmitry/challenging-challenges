import { Component, OnDestroy, ViewEncapsulation, ViewChild } from "@angular/core";
import { MdlLayoutComponent } from "angular2-mdl";

@Component({
    selector: "app",
    template: require("./app.component.html"),
    styles: [require("./app.component.css")],
    encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnDestroy {
    @ViewChild(MdlLayoutComponent)
    layout: MdlLayoutComponent;

    ngOnDestroy(): void {
        document.body.appendChild(document.createElement("app"));
    }

    onNavigated(): void {
        this.layout.closeDrawerOnSmallScreens();
    }
}
