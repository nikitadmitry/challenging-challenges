import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { MdlModule } from "angular2-mdl";
import { MdlSelectModule } from "@angular2-mdl-ext/select";

import { LoadingSpinnerComponent } from "./shared-components/loading-spinner.component";
import { MdlTextFieldValidatedComponent } from "./shared-components/mdl-textfield-validated.component";
//import { PaginatorComponent } from "./shared-components/paginator.component";

@NgModule({
    declarations: [
        LoadingSpinnerComponent,
        //PaginatorComponent,
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
        LoadingSpinnerComponent,
        MdlModule,
        //PaginatorComponent,
        MdlTextFieldValidatedComponent,
        MdlSelectModule
    ]
})
export class SharedModule { }