import {Injectable} from "@angular/core";
import {MdlDialogService, MdlDialogReference} from "@angular-mdl/core";
import {Observable} from "rxjs";
import {LoginDialogComponent} from "./login/login-dialog.component";
import {RegisterDialogComponent} from "./register/register-dialog.component";

@Injectable()
export class AuthDialogsService {
    constructor(private dialogService: MdlDialogService) { }

    public openLoginDialog(): Observable<MdlDialogReference> {
        return this.dialogService.showCustomDialog({
            component: LoginDialogComponent,
            isModal: true,
            styles: {"width": "350px"},
            clickOutsideToClose: true,
            enterTransitionDuration: 400,
            leaveTransitionDuration: 400
        });
    }

    openRegistrationDialog(): Observable<MdlDialogReference> {
        return this.dialogService.showCustomDialog({
            component: RegisterDialogComponent,
            isModal: true,
            styles: {"width": "350px"},
            clickOutsideToClose: true,
            enterTransitionDuration: 400,
            leaveTransitionDuration: 400
        });
    }
}