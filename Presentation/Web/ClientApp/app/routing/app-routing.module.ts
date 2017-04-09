import { NgModule }             from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import {HomeComponent} from "../home/home.component";
import {AuthGuard} from "../shared/services/auth-guard.service";
import {AuthService} from "../auth/auth.service";
import {AuthDialogsService} from "../auth/auth-dialogs.service";

const routes: Routes = [
    { path: "", redirectTo: "home", pathMatch: "full" },
    { path: "home", component: HomeComponent },
    { path: "challenges", loadChildren: "../challenges/challenges.module#ChallengesModule" },
    { path: "challenge", loadChildren: "../challenge/challenge.module#ChallengeModule", canActivate: [AuthGuard] },
    { path: "user", loadChildren: "../user/user.module#UserModule", canActivate: [AuthGuard] },
    { path: "**", redirectTo: "home" }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: [AuthGuard,AuthService,AuthDialogsService]
})
export class AppRoutingModule {}