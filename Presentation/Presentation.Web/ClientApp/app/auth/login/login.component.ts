import { Component, Output, EventEmitter } from '@angular/core';
import {LoginDialogComponent} from './login-dialog.component';

@Component({
    selector: 'login',
    template: require('./login.component.html')
})
export class LoginComponent {
    @Output()
    onDialogClosed: EventEmitter<void> = new EventEmitter<void>();

    openDialog() {
        
    }
}