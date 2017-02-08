import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";
import { tokenNotExpired, AuthConfig } from "angular2-jwt";
import { LoginViewModel } from "./login/login.model";
import { RegisterViewModel } from "./register/register.model";
import { Actions } from "../shared/actions";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/toPromise";

@Injectable()
export class AuthService {
    private tokenName: string = this.authConfig.getConfig().tokenName;

    constructor(private http: Http, private authConfig: AuthConfig) { }

    public isLoggedOn(): boolean {
         try {
             return tokenNotExpired();
         } catch (error) {
             console.log(error.message);
             this.logout();
         }
    };

    public login(loginViewModel: LoginViewModel): Promise<any> {
        return this.subscribeToRequest(this.http.post(Actions.account.login, loginViewModel));
    }

    public register(registerViewModel: RegisterViewModel): Promise<any> {
        return this.subscribeToRequest(this.http.post(Actions.account.register, registerViewModel));
    }

    private subscribeToRequest(tokenResponse: Observable<Response>): Promise<any> {
        return tokenResponse.toPromise()
            .then(response => {
                localStorage.setItem(this.tokenName, (response.json() as TokenResponse).token);
            })
            .catch(error => this.handleError(error));
    }

    private handleError (error: any): any {
        return Promise.reject(error.json());
    }

    public logout(): void {
        localStorage.removeItem(this.tokenName);
    }

    public isUsernameTaken(userName: string): Promise<boolean> {
        return this.http.get(Actions.account.checkUsernameAvailability(userName))
            .toPromise()
            .then(x => !(x.json() as boolean));
    }

    public isEmailRegistered(email: string): Promise<boolean> {
        return this.http.get(Actions.account.checkEmailAvailability(email))
            .toPromise()
            .then(x => !(x.json() as boolean));
    }

    public isPasswordStrong(password: string): boolean {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(password);
    }
}

interface TokenResponse {
    token: string;
}