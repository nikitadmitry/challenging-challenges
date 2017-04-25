import { Component, OnInit } from "@angular/core";
import { MdlDialogReference } from "@angular-mdl/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { TranslationService, Translation } from "angular-l10n";

import { AuthService } from "../auth.service";

@Component({
    selector: "login",
    template: require("./login-dialog.component.html"),
    styles: [require("./login-dialog.component.css")],
    providers: [AuthService]
})
export class LoginDialogComponent extends Translation implements OnInit {
    loginForm: FormGroup;
    userName: string;

    constructor(private authService: AuthService, private fb: FormBuilder,
    private loginDialog: MdlDialogReference, translationService: TranslationService ) {
        super(translationService);
    }

    ngOnInit(): void {
        this.loginForm = this.fb.group({
            userName: ["", Validators.compose([Validators.required, Validators.minLength(4), Validators.maxLength(24)])],
            password: ["", Validators.compose([Validators.required, Validators.minLength(6)])]
        });
    }

    login(): void {
        if (this.loginForm.valid) {
            this.authService.login(this.loginForm.value).then(() => this.closeDialog());
        }
    }

    cancel(): void {
        this.closeDialog();
    }

    private closeDialog(): void {
        this.loginDialog.hide();
    }
}