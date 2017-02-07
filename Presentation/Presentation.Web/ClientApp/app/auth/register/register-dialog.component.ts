import { Component, OnInit } from "@angular/core";
import { MdlDialogReference } from "angular2-mdl";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";

import { AuthService } from "../auth.service";
import { RegistrationValidator } from "./RegistrationValidator";

@Component({
    selector: "login",
    template: require("./register-dialog.component.html"),
    styles: [require("./register-dialog.component.css")],
    providers: [AuthService, RegistrationValidator]
})
export class RegisterDialogComponent implements OnInit {
    registerForm: FormGroup;
    userName: string;

    usernamePattern: string = "(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[a-zA-Z0-9]{6,100}";

    constructor(private authService: AuthService, private fb: FormBuilder,
    private registerDialog: MdlDialogReference, private registrationValidator: RegistrationValidator) { }

    ngOnInit(): void {
        this.registerForm = this.fb.group({
            userName: ["", Validators.compose([Validators.required, Validators.minLength(4), Validators.maxLength(24)]),
                (control) => this.registrationValidator.usernameTaken(control)],
            email: ["", Validators.compose([Validators.required, this.registrationValidator.email]),
                this.registrationValidator.emailRegistered],
            password: ["", Validators.compose([Validators.required, Validators.pattern(this.usernamePattern)])],
            confirmPassword: ["", this.registrationValidator.equalToPassword]
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