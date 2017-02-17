import { Injectable } from "@angular/core";
import { AbstractControl } from "@angular/forms";

@Injectable()
export class FormControlValidationMessagesBuilder {
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
        switch (errorName) {
            case "required":
                return `Поле обязательно для заполнения.`;
            case "minlength":
                return `Минимальная длина поля ${errorContext.requiredLength} символа.`;
            case "maxlength":
                return `Максимальная длина поля ${errorContext.requiredLength} символа.`;
            case "email":
                return "Введите корректный адрес.";
            case "usernameTaken":
                return "Введенное имя уже зарегистрировано.";
            case "emailRegistered":
                return "Адрес уже используется.";
            case "passwordComplexity":
                return "Пароль должен содержать прописную, заглавную буквы и цифру.";
            case "equalToPassword":
                return "Пароли должны совпадать.";
            default:
                return "";
        }
    }
}