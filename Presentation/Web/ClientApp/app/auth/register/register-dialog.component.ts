import { Component, OnInit } from "@angular/core";
import { MdlDialogReference } from "angular2-mdl";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";

import { AuthService } from "../auth.service";
import { RegistrationValidator } from "./RegistrationValidator";
import {Translation, TranslationService} from "angular-l10n";

@Component({
    selector: "register",
    template: require("./register-dialog.component.html"),
    styles: [require("./register-dialog.component.css")],
    providers: [AuthService, RegistrationValidator]
})
export class RegisterDialogComponent extends Translation implements OnInit {
    registerForm: FormGroup;
    userName: string;

    constructor(private authService: AuthService, private fb: FormBuilder,
        private registerDialog: MdlDialogReference,
        private registrationValidator: RegistrationValidator,
        translationService: TranslationService) {
        super(translationService);
    }

    ngOnInit(): void {
        this.registerForm = this.fb.group({
            userName: ["", Validators.compose([Validators.required, Validators.minLength(4), Validators.maxLength(24)]),
                (control) => this.registrationValidator.usernameTaken(control)],
            email: ["", Validators.compose([Validators.required, (control) => this.registrationValidator.email(control)]),
                (control) => this.registrationValidator.emailRegistered(control)],
            password: ["", Validators.compose([Validators.required, Validators.minLength(6), Validators.maxLength(100),
                (control) => this.registrationValidator.passwordComplexity(control)])],
            confirmPassword: ["", Validators.compose([Validators.required,
                (control) => this.registrationValidator.equalToPassword(control)])]
        });
    }

    register(): void {
        if (this.registerForm.valid) {
            this.authService.register(this.registerForm.value).then(() => this.closeDialog());
        }
    }

    cancel(): void {
        this.closeDialog();
    }

    private closeDialog(): void {
        this.registerDialog.hide();
    }
}