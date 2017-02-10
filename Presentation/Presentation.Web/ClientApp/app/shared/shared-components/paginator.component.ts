import { Component, Output, ChangeDetectionStrategy } from "@angular/core";

import { PaginationInstance } from "ng2-pagination";

@Component({
    selector: "paginator",
    template: require("./paginator.component.html"),
    styles: [require("./paginator.component.css")],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class PaginatorComponent {
    @Output()
    pageChange: () => void;

    public config: PaginationInstance = {
        itemsPerPage: 10,
        currentPage: 1
    };
}