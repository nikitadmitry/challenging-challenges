import {NgModule} from "@angular/core";
import {MdlTextFieldValidatedComponent} from "./mdl-textfield-validated.component";
import {FormsModule} from "@angular/forms";
import {CommonModule} from "@angular/common";
import {MdlTextFieldModule} from "@angular-mdl/core";
@NgModule({
    declarations: [
        MdlTextFieldValidatedComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        MdlTextFieldModule
    ],
    exports: [
        MdlTextFieldValidatedComponent
    ]
})
export class MdlTextFieldValidatedModule { }