import { Injectable } from "@angular/core";
import {Http, Response, URLSearchParams} from "@angular/http";
import { tokenNotExpired, AuthConfig, AuthHttp } from "angular2-jwt";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/toPromise";

import { LoginViewModel } from "./login/login.model";
import { RegisterViewModel } from "./register/register.model";
import { Actions } from "../shared/actions";

@Injectable()
export class AuthService {
    private tokenName: string = this.authConfig.getConfig().tokenName;

    constructor(private http: Http, private authConfig: AuthConfig, private authHttp: AuthHttp) { }

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

    public getUserName(): Promise<string> {
        return this.authHttp.get(Actions.account.getUserName)
            .toPromise()
            .then(x => x.text() as string);
    }

    public logout(): void {
        localStorage.removeItem(this.tokenName);
    }

    public isUsernameTaken(userName: string): Promise<boolean> {
        let params = new URLSearchParams();
        params.set('userName', userName);

        return this.http.get(Actions.account.checkUsernameAvailability, { search: params })
            .toPromise()
            .then(x => !(x.json() as boolean));
    }

    public isEmailRegistered(email: string): Promise<boolean> {
        let params = new URLSearchParams();
        params.set('email', email);

        return this.http.get(Actions.account.checkEmailAvailability, { search: params })
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