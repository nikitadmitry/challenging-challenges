import { Component, EventEmitter, Output, ViewEncapsulation } from "@angular/core";
import { TranslationService, Translation } from "angular-l10n";

import { AuthService } from "./auth.service";
import {AuthDialogsService} from "./auth-dialogs.service";
import {Router} from "@angular/router";
import {UserService} from "../user/user.service";

@Component({
    selector: "auth",
    template: require("./auth.component.html"),
    styles: [require("./auth.component.css")],
    providers: [AuthService, AuthDialogsService, UserService],
    encapsulation: ViewEncapsulation.None
})
export class AuthComponent extends Translation {
    @Output("onNavigated")
    onNavigatedEmitter: EventEmitter<void> = new EventEmitter<void>();

    constructor(private authService: AuthService, translationService: TranslationService,
                private authDialogsService: AuthDialogsService, private router: Router,
                private UserService: UserService) {
            super(translationService);
        }

    private onNavigated(): void {
        this.onNavigatedEmitter.emit();
    }

    isLoggedOn(): boolean {
        return this.authService.isLoggedOn();
    }

    goToProfile() {
        this.UserService.getCurrentUserId()
            .subscribe((userId) => this.router.navigate(["user", userId]));
    }

    openLoginDialog(): void {
        this.authDialogsService.openLoginDialog().subscribe(() => {
            this.onNavigated();
        });
    }

    openRegistrationDialog(): void {
        this.authDialogsService.openRegistrationDialog().subscribe(() => {
            this.onNavigated();
        });
    }

    logout(): void {
        this.authService.logout();
        this.router.navigate(["home"]);
    }
}