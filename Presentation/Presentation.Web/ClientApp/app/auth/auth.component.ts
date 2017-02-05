import { Component, Output, EventEmitter } from '@angular/core';
import { AuthService } from './auth.service';

@Component({
    selector: 'auth',
    template: require('./auth.component.html'),
    providers: [AuthService]
})
export class AuthComponent {
    @Output()
    onItemClicked: EventEmitter<void> = new EventEmitter<void>();
    
    constructor(private authService: AuthService) { }
}