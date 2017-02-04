import { Component } from '@angular/core';
import { MdDialog } from '@angular/material';
import {LoginDialogComponent} from './login-dialog.component';

@Component({
    selector: 'login',
    template: require('./login.component.html')
})
export class LoginComponent {

    constructor(public dialog: MdDialog) {}

    openDialog() {
        let dialogRef = this.dialog.open(LoginDialogComponent);
        dialogRef.afterClosed().subscribe(result => {
            
        });
    }
}