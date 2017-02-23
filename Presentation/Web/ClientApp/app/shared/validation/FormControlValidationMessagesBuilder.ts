import { Injectable } from "@angular/core";
import { AbstractControl } from "@angular/forms";
import { TranslationService } from "angular-l10n";

@Injectable()
export class FormControlValidationMessagesBuilder {
    constructor(private translationService: TranslationService) { }

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
        var errorTemplate = this.translationService.translate("Validation." + errorName);
        switch (errorName) {
            case "minlength":
                return `Минимальная длина поля ${errorContext.requiredLength}.`;
            case "maxlength":
                return `Максимальная длина поля ${errorContext.requiredLength}.`;
            default:
                return "";
        }
    }
}