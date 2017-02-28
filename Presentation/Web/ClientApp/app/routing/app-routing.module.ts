import { NgModule }             from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import {HomeComponent} from "../home/home.component";
import {AuthGuard} from "../shared/services/auth-guard.service";

const routes: Routes = [
    { path: "", redirectTo: "home", pathMatch: "full" },
    { path: "home", component: HomeComponent },
    { path: "challenges", loadChildren: "../challenges/challenges.module#ChallengesModule" },
    { path: "challenge", loadChildren: "../challenge/challenge.module#ChallengeModule", canActivate: [AuthGuard] },
    { path: "**", redirectTo: "home" }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}