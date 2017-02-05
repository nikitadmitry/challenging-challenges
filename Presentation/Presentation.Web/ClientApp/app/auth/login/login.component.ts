import { Component } from '@angular/core';
import { MdDialog,Overlay,OverlayContainer } from '@angular/material';
import {LoginDialogComponent} from './login-dialog.component';

@Component({
    selector: 'login',
    template: require('./login.component.html')
})
export class LoginComponent {

    constructor(public dialog: MdDialog) {}

    openDialog() {
        let dialogRef = this.dialog.open(LoginDialogComponent, {
            height: '400px',
            width: '600px',
        });
        dialogRef.afterClosed().subscribe(result => {
            
        });
    }
}