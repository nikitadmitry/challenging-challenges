import { Component, EventEmitter, Output, ViewEncapsulation } from "@angular/core";
import { MdlDialogService, MdlDialogReference } from "angular2-mdl";
import { TranslationService, Translation } from "angular-l10n";

import { AuthService } from "./auth.service";
import { LoginDialogComponent } from "./login/login-dialog.component";
import { RegisterDialogComponent } from "./register/register-dialog.component";

@Component({
    selector: "auth",
    template: require("./auth.component.html"),
    styles: [require("./auth.component.css")],
    providers: [AuthService],
    encapsulation: ViewEncapsulation.None
})
export class AuthComponent extends Translation {
    @Output("onNavigated")
    onNavigatedEmitter: EventEmitter<void> = new EventEmitter<void>();

    constructor(private authService: AuthService, private dialogService: MdlDialogService,
        translationService: TranslationService) {
            super(translationService);
        }

    private onNavigated(): void {
        this.onNavigatedEmitter.emit();
    }

    isLoggedOn(): boolean {
        return this.authService.isLoggedOn();
    }

    openLoginDialog(): void {
        this.dialogService.showCustomDialog({
            component: LoginDialogComponent,
            isModal: true,
            styles: {"width": "350px"},
            clickOutsideToClose: true,
            enterTransitionDuration: 400,
            leaveTransitionDuration: 400
        }).subscribe((dialogReference: MdlDialogReference) => {
             this.onNavigated();
        });
    }

    openRegistrationDialog(): void {
        this.dialogService.showCustomDialog({
            component: RegisterDialogComponent,
            isModal: true,
            styles: {"width": "350px"},
            clickOutsideToClose: true,
            enterTransitionDuration: 400,
            leaveTransitionDuration: 400
        }).subscribe((dialogReference: MdlDialogReference) => {
             this.onNavigated();
        });
    }

    logout(): void {
        this.authService.logout();
    }
}