import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { MdlSelectModule } from "@angular2-mdl-ext/select";
import { FormsModule } from "@angular/forms";
import { TranslationModule } from "angular-l10n";

import { SharedModule } from "../shared/shared.module";
import { ChallengeDetailsComponent } from "./challenge-details/challenge-details.component";

@NgModule({
    declarations: [
        ChallengeDetailsComponent
    ],
    imports: [
        SharedModule,
        MdlSelectModule,
        FormsModule,
        RouterModule.forChild([
            { path: ":id", component: ChallengeDetailsComponent }
        ]),
        TranslationModule.forChild()
    ]
})
export class ChallengeModule { }