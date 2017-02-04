import { Component } from '@angular/core';
import { Http } from '@angular/http';
import { AuthHttp, tokenNotExpired } from 'angular2-jwt';

@Component({
    selector: 'counter',
    template: require('./counter.component.html')
})
export class CounterComponent {
    public currentCount = 0;

    constructor(private http: Http, private authHttp: AuthHttp) { }

    public incrementCounter() {
        this.currentCount++;
    }

     public isLoggedOn() {
        return tokenNotExpired();
    };

    public login() {
            this.http.post('/Account/Login', {
                userName: 'TestUser1',
                password: 'qWaszx-12'
            }).subscribe(response => {
                localStorage.setItem('id_token', response.json() as string);
            });
        }

    public logout() {
        localStorage.removeItem('id_token');
    }
}
