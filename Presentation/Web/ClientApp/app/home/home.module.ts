import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { Ng2PaginationModule } from "ng2-pagination";
import { TranslationModule } from "angular-l10n";

import { HomeComponent } from "./home.component";
import { ChallengeCardComponent } from "./challenges/challenge-card.component";
import { LatestChallengesComponent } from "./challenges/latest-challenges.component";
import { PopularChallengesComponent } from "./challenges/popular-challenges.component";
import { UnsolvedChallengesComponent } from "./challenges/unsolved-challenges.component";
import { TopUsersComponent } from "./top-users/top-users.component";
import {MdlNonRootModule} from "angular2-mdl";
import {MarkdownModule} from "../shared/components/markdown.component";
import {LoadingSpinnerModule} from "../shared/components/loading-spinner.component/loading-spinner.module";

@NgModule({
    declarations: [
        ChallengeCardComponent,
        LatestChallengesComponent,
        PopularChallengesComponent,
        UnsolvedChallengesComponent,
        HomeComponent,
        TopUsersComponent
    ],
    imports: [
        CommonModule,
        LoadingSpinnerModule,
        Ng2PaginationModule,
        RouterModule.forChild([{ path: "", component: HomeComponent }]),
        MarkdownModule,
        MdlNonRootModule,
        TranslationModule
    ],
    exports: [HomeComponent]
})
export class HomeModule { }