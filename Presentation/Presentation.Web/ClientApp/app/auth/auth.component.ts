import { Component } from '@angular/core';
import { AuthService } from './auth.service';

@Component({
    selector: 'auth',
    template: require('./auth.component.html'),
    providers: [AuthService]
})
export class AuthComponent {
    constructor(private authService: AuthService) { }

    public isLoggedOn(): boolean {
        return this.authService.isLoggedOn();
    }
}