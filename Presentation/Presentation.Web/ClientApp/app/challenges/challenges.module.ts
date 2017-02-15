import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { MdlSelectModule } from "@angular2-mdl-ext/select";
import { FormsModule } from "@angular/forms";
import { TranslationModule } from "angular-l10n";

import { SharedModule } from "../shared/shared.module";
import { ChallengesComponent } from "./challenges.component";

@NgModule({
    declarations: [
        ChallengesComponent
    ],
    imports: [
        SharedModule,
        MdlSelectModule,
        FormsModule,
        RouterModule.forChild([
            { path: "", component: ChallengesComponent }
        ]),
        TranslationModule.forChild()
    ]
})
export class ChallengesModule { }