import {Pipe, PipeTransform, NgModule} from '@angular/core';
import {FormControlValidationMessagesBuilder} from "../validation/FormControlValidationMessagesBuilder";
import {AbstractControl} from "@angular/forms";

@Pipe({name: 'errorText'})
export class ErrorTextPipe implements PipeTransform {
    constructor(private errorBuilder: FormControlValidationMessagesBuilder) {
    }

    transform(errors: any): string {
        let errorStrings = this.errorBuilder.buildForErrors(errors);
        return errorStrings.length > 0 ? errorStrings[0] : undefined;
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