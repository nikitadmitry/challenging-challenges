import {Pipe, PipeTransform, NgModule} from '@angular/core';
import {FormControlValidationMessagesBuilder} from "../validation/FormControlValidationMessagesBuilder";
import {AbstractControl} from "@angular/forms";

@Pipe({name: 'errorText'})
export class ErrorTextPipe implements PipeTransform {
    constructor(private errorBuilder: FormControlValidationMessagesBuilder) {
    }

    transform(error: any): string {
        console.log("detect");
        return "err";
        // let errors = this.errorBuilder.build(value);
        // return errors.length > 0 ? errors[0] : undefined;
    }
}

@NgModule({
    declarations: [
        ErrorTextPipe
    ],
    exports: [
        ErrorTextPipe
    ]
})
export class ErrorTextPipeModule { }