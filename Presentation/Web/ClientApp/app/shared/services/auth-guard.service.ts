import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AuthService } from "../../auth/auth.service";
import {AuthDialogsService} from "../../auth/auth-dialogs.service";

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private authService: AuthService, private authDialogsService: AuthDialogsService) {}

    canActivate() {
        if(this.authService.isLoggedOn()) {
            return true;
        } else {
            this.authDialogsService.openLoginDialog();
            return false;
        }
    }
}