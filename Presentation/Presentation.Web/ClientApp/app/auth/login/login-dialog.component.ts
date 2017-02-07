import { Component, OnInit } from "@angular/core";
import { MdlDialogReference } from "angular2-mdl";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";

import { AuthService } from "../auth.service";

@Component({
    selector: "login",
    template: require("./login-dialog.component.html"),
    styles: [require("./login-dialog.component.css")],
    providers: [AuthService]
})
export class LoginDialogComponent implements OnInit {
    loginForm: FormGroup;

    validationMessages = {
        "userName": {
            "required":      "Name is required.",
            "minlength":     "Name must be at least 4 characters long.",
            "maxlength":     "Name cannot be more than 24 characters long.",
            "forbiddenName": "Someone named \"Bob\" cannot be a hero."
        },
        "password": {
            "required": "Power is required."
        }
    };

    constructor(private authService: AuthService, private fb: FormBuilder,
    private loginDialog: MdlDialogReference) { }

    ngOnInit(): void {
        this.loginForm = this.fb.group({
            userName: ["", [Validators.required, Validators.minLength(4), Validators.maxLength(24)]],
            password: ["", [Validators.required, Validators.minLength(6), Validators.maxLength(100)]]
        });
    }

    login(): void {
        this.authService.login(this.loginForm.value).then(() => this.closeDialog());
        // , (error) => {
        //     debugger;
        // });
    }

    cancel(): void {
        this.closeDialog();
    }

    private closeDialog(): void {
        this.loginDialog.hide();
    }
}