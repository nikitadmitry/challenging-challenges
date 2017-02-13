import { Component, Input } from "@angular/core";

@Component({
    selector: "loading-spinner",
    template: `<mdl-spinner class="spinner" single-color *ngIf="active" [active]="active"></mdl-spinner>`,
    styles: [`
        .spinner {
                position: absolute;
                left: calc(50% - 14px);
                z-index: 100050;
        }`]
})
export class LoadingSpinnerComponent {
    @Input()
    active: boolean;
}