import { Component, Input, Output, EventEmitter } from "@angular/core";

@Component({
    selector: "paging-buttons",
    template: require("./paging-buttons.component.html")
})
export class PagingButtonsComponent {
    @Input() nextPageEnabled: boolean;
    @Input() previousPageEnabled: boolean;
    @Output() nextPage: EventEmitter<void> = new EventEmitter<void>();
    @Output() previousPage: EventEmitter<void> = new EventEmitter<void>();
}