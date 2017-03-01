import { Component, EventEmitter, Output, ViewEncapsulation } from "@angular/core";
import { TranslationService, Translation } from "angular-l10n";

import { AuthService } from "./auth.service";
import {AuthDialogsService} from "./auth-dialogs.service";

@Component({
    selector: "auth",
    template: require("./auth.component.html"),
    styles: [require("./auth.component.css")],
    providers: [AuthService, AuthDialogsService],
    encapsulation: ViewEncapsulation.None
})
export class AuthComponent extends Translation {
    @Output("onNavigated")
    onNavigatedEmitter: EventEmitter<void> = new EventEmitter<void>();

    constructor(private authService: AuthService, translationService: TranslationService,
                private authDialogsService: AuthDialogsService) {
            super(translationService);
        }

    private onNavigated(): void {
        this.onNavigatedEmitter.emit();
    }

    isLoggedOn(): boolean {
        return this.authService.isLoggedOn();
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
    }
}