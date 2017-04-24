import { Injectable } from "@angular/core";
import { AbstractControl } from "@angular/forms";
import { TranslationService } from "angular-l10n";

@Injectable()
export class FormControlValidationMessagesBuilder {
    constructor(private translationService: TranslationService) {
        // this.translationService.AddConfiguration()
        //     .AddProvider("./assets/locale-");
        // this.translationService.init();
    }

    public build(control: AbstractControl): Array<string> {
        if (!control.errors) {
            return new Array<string>();
        }

        var errors: any = control.errors;
        var errorNames: Array<string> = Object.keys(errors);
        var errorMessages: Array<string> = new Array<string>();

        errorNames.forEach(errorName => {
            errorMessages.push(this.getErrorMessage(errorName, errors[errorName]));
        });

        return errorMessages;
    }

    private getErrorMessage(errorName: string, errorContext: any): string {
        let errorParameters: any[];

        switch (errorName) {
            case "minlength":
            case "maxlength":
                errorParameters = [errorContext.requiredLength];
                break;
            default:
                errorParameters = [];
        }

        return this.translationService.translate("Validation." + errorName, errorParameters);
    }
}