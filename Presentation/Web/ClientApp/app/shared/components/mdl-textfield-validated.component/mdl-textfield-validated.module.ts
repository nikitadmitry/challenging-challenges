import {NgModule} from "@angular/core";
import {MdlTextFieldValidatedComponent} from "./mdl-textfield-validated.component";
import {FormsModule} from "@angular/forms";
import {CommonModule} from "@angular/common";
@NgModule({
    declarations: [
        MdlTextFieldValidatedComponent
    ],
    imports: [
        CommonModule,
        FormsModule
    ],
    exports: [
        MdlTextFieldValidatedComponent
    ]
})
export class MdlTextFieldValidatedModule { }