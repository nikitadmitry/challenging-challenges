import { Component } from '@angular/core';
import { Actions } from '../../shared/actions'
import { AuthService } from '../../shared/auth/auth.service'

@Component({
    selector: 'counter',
    template: require('./counter.component.html'),
    providers: [AuthService]
})
export class CounterComponent {
    public currentCount = 0;


    constructor(private authService: AuthService) { }

    public incrementCounter() {
        this.currentCount++;
    }

     public isLoggedOn() {
         return this.authService.isLoggedOn();
    };

    public login() {
        this.authService.login({ userName: 'TestUser1', password: 'qWaszx12' });
        //this.authService.login({ userName: 'TestUser1', password: undefined });
    }

    public logout() {
        this.authService.logout();
    }
}
