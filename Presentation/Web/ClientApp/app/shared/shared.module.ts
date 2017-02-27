import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MdlModule } from "angular2-mdl";

import { LoadingSpinnerComponent } from "./shared-components/loading-spinner.component";
import { MarkdownComponent } from "./shared-components/markdown.component";
import {EnumPipe} from "./pipes/enum.pipe";

const sharedDeclarations: any[] = [
    LoadingSpinnerComponent,
    MarkdownComponent,
    EnumPipe
];

const sharedImports: any[] = [
    CommonModule,
    MdlModule
];

@NgModule({
    declarations: [
        sharedDeclarations
    ],
    imports: [
        sharedImports
    ],
    exports: [
        sharedDeclarations,
        sharedImports
    ]
})
export class SharedModule { }