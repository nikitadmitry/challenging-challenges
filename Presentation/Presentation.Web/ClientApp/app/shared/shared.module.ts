import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MdlModule } from "angular2-mdl";
import { MdlSelectModule } from "@angular2-mdl-ext/select";

import { LoadingSpinnerComponent } from "./shared-components/loading-spinner.component";
import { MdlTextFieldValidatedComponent } from "./shared-components/mdl-textfield-validated.component";

@NgModule({
    declarations: [
        LoadingSpinnerComponent,
        MdlTextFieldValidatedComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        MdlModule,
        MdlSelectModule
    ],
    exports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        LoadingSpinnerComponent,
        MdlModule,
        MdlTextFieldValidatedComponent,
        MdlSelectModule
    ]
})
export class SharedModule { }