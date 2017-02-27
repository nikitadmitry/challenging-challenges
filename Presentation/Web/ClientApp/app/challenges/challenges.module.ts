import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { MdlSelectModule } from "@angular2-mdl-ext/select";
import { FormsModule } from "@angular/forms";
import { TranslationModule } from "angular-l10n";

import { SharedModule } from "../shared/shared.module";
import { ChallengesComponent } from "./challenges.component";
import { ChallengeComponent } from "./challenge/challenge.component";
import { FiltersComponent } from "./filters/filters.component";
import { PagingButtonsComponent } from "./paging-buttons/paging-buttons.component";
import {EnumPipe} from "../shared/pipes/enum.pipe";

@NgModule({
    declarations: [
        ChallengeComponent,
        ChallengesComponent,
        FiltersComponent,
        PagingButtonsComponent,
        EnumPipe
    ],
    imports: [
        SharedModule,
        MdlSelectModule,
        FormsModule,
        RouterModule.forChild([
            { path: "", component: ChallengesComponent },
            { path: "**", redirectTo: "" }
        ]),
        TranslationModule.forChild()
    ]
})
export class ChallengesModule { }