import { Injectable } from "@angular/core";

import { AuthService } from "./../auth.service";

interface ValidationResult {
    [key:string]: boolean;
}

@Injectable()
export class RegistrationValidator {
    private usernameTimeout;
    private emailTimeout;
    private requestDebounce = 1500;

    constructor(private authService: AuthService) { }

    usernameTaken(control: any): Promise<ValidationResult> {
        if (this.usernameTimeout) {
            clearTimeout(this.usernameTimeout);
        }
        return new Promise((resolve, reject) => {
            this.usernameTimeout = setTimeout(() => {
                this.authService.isUsernameTaken(control.value)
                    .then((isTaken: boolean) => {
                    if (isTaken) {
                        resolve({"usernameTaken": true});
                    }
                    resolve(null);
                });
            }, this.requestDebounce);
        });
    }

    emailRegistered(control: any): Promise<ValidationResult> {
        if (this.emailTimeout) {
            clearTimeout(this.emailTimeout);
        }
        return new Promise((resolve, reject) => {
            this.emailTimeout = setTimeout(() => {
                this.authService.isEmailRegistered(control.value)
                    .then((isTaken: boolean) => {
                    if (isTaken) {
                        resolve({"emailRegistered": true});
                    }
                    resolve(null);
                });
            }, this.requestDebounce);
        });
    }

    passwordComplexity(control: any): ValidationResult {
        var re: RegExp = new RegExp("(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[a-zA-Z0-9]");
        if (!re.test(control.value)) {
            return { passwordComplexity: false };
        }
        return null;
    }

    email(control: any): ValidationResult {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (!re.test(control.value)) {
            return { email: false };
        }
        return null;
    }

    equalToPassword(control: any): ValidationResult {
        let v = control.value;

        let e = control.root.get("password");

        if (e && e.value !== "" && v !== e.value) {
            return { equalToPassword: false }
        }

        return null;
    }
}