import { Component } from '@angular/core';
import { MdDialogRef } from '@angular/material';
import { LoginViewModel } from './login.model';

@Component({
    template: require('./login-dialog.component.html'),
})
export class LoginDialogComponent {
    public model: LoginViewModel;

    constructor(public dialogRef: MdDialogRef<LoginDialogComponent>) {}
}