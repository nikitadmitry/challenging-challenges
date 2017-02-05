import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoginViewModel } from './login.model';

@Component({
    selector: 'login-dialog',
    template: require('./login-dialog.component.html')
})
export class LoginDialogComponent {
    public model: LoginViewModel;

    constructor(private router: Router) { }

    public login(): void {
        this.router.navigate([{ outlets: { popup: null }}]);
    }

    public register(): void {

    }
}