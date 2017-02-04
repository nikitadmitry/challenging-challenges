import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { tokenNotExpired, JwtHelper, AuthConfig } from 'angular2-jwt';
import { LoginViewModel } from './login/login.model';
import { RegisterViewModel } from './register/register.model';
import { Actions } from '../shared/actions'
import { Observable } from 'rxjs/Observable'
import { Messages } from '../shared/messages';

@Injectable()
export class AuthService {
    private tokenName: string = this.authConfig.getConfig().tokenName;
    private jwtHelper: JwtHelper = new JwtHelper();

    constructor(private http: Http, private authConfig: AuthConfig) { }

    public isLoggedOn(): boolean {
         try {
             return tokenNotExpired();
         } catch (error) {
             console.log(error.message);
             this.logout();
         }
    };

    public login(loginViewModel: LoginViewModel) {
        this.subscribeToRequest(this.http.post(Actions.account.login, loginViewModel));
    }

    public register(registerViewModel: RegisterViewModel) {
        this.subscribeToRequest(this.http.post(Actions.account.register, registerViewModel));
    }

    private subscribeToRequest(observable: Observable<Response>) {
        observable.subscribe(response => {
            localStorage.setItem(this.tokenName, (response.json() as TokenResponse).token);
        }, error => this.handleError(error.json()));
    }

    private handleError(errors: any[]): void {
        errors.forEach(error => {
            Messages.push({severity: "error", detail: error, summary: "Error"});
        });
    }

    public logout() {
        localStorage.removeItem(this.tokenName);
    }
}

interface TokenResponse {
    token: string;
}