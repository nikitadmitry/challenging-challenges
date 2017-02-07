import { Component, EventEmitter, Output } from "@angular/core";
import { AuthService } from "./auth.service";
import { LoginDialogComponent } from "./login/login-dialog.component";
import { MdlDialogService, MdlDialogReference } from "angular2-mdl";

@Component({
    selector: "auth",
    template: require("./auth.component.html"),
    styles: [require("./auth.component.css")],
    providers: [AuthService]
})
export class AuthComponent {
    @Output("onNavigated")
    onNavigatedEmitter: EventEmitter<void> = new EventEmitter<void>();

    constructor(private authService: AuthService, private dialogService: MdlDialogService) { }

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
}