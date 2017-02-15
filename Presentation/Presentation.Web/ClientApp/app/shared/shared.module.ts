import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MdlModule } from "angular2-mdl";

import { LoadingSpinnerComponent } from "./shared-components/loading-spinner.component";

const sharedImports: any[] = [
    LoadingSpinnerComponent
];

const sharedDeclarations: any[] = [
    CommonModule,
    MdlModule
];

@NgModule({
    declarations: [
        sharedImports
    ],
    imports: [
        sharedDeclarations
    ],
    exports: [
        sharedDeclarations,
        sharedImports
    ]
})
export class SharedModule { }