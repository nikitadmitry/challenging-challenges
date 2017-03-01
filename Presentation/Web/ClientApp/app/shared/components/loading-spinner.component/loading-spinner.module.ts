import {NgModule} from "@angular/core";
import {LoadingSpinnerComponent} from "./loading-spinner.component";
import {MdlSpinnerModule} from "angular2-mdl";
import {CommonModule} from "@angular/common";

@NgModule({
    declarations: [
        LoadingSpinnerComponent
    ],
    imports: [
        CommonModule,
        MdlSpinnerModule
    ],
    exports: [
        LoadingSpinnerComponent
    ]
})
export class LoadingSpinnerModule { }