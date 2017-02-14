import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";

import { SharedModule } from "../shared/shared.module";
import { ChallengesComponent } from "./challenges.component";

@NgModule({
    declarations: [
        ChallengesComponent
    ],
    imports: [
        SharedModule,
        RouterModule.forChild([
            { path: "", component: ChallengesComponent }
            //{ path: "challenges", component: ChallengesComponent }
        ])
    ]
})
export class ChallengesModule { }