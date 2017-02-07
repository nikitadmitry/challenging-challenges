interface ValidationResult {
    [key:string]: boolean;
}

export class RegistrationValidator {
    static usernameTaken(control: any): Promise<ValidationResult> {
        console.log("usernameTaken");
        let q = new Promise((resolve, reject) => {
            setTimeout(() => {
                if (control.value === "David") {
                    resolve({"usernameTaken": true});
                } else {
                    resolve(null);
                }
            }, 1000);
        });

        return q;
    }

    static emailRegistered(control: any): Promise<ValidationResult> {
        console.log("emailRegistered");
        let q = new Promise((resolve, reject) => {
            setTimeout(() => {
                if (control.value === "qwe@qwe.qwe") {
                    resolve({"emailRegistered": true});
                } else {
                    resolve(null);
                }
            }, 1000);
        });

        return q;
    }

    static equalToPassword(control: any): ValidationResult {
        console.log("equalToPassword");
        let v = control.value;

        let e = control.root.get("password");

        if (e && v !== e.value) {
            return { equalToPassword: false }
        }

        return null;
    }
}