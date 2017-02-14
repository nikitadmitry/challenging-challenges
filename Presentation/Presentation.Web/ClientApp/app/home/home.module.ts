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
import { MarkdownComponent } from "./challenges/markdown.component";
import { TopUsersComponent } from "./top-users/top-users.component";
import { SharedModule } from "../shared/shared.module";

@NgModule({
    declarations: [
        ChallengeCardComponent,
        LatestChallengesComponent,
        PopularChallengesComponent,
        UnsolvedChallengesComponent,
        HomeComponent,
        MarkdownComponent,
        TopUsersComponent
    ],
    imports: [
        CommonModule,
        Ng2PaginationModule,
        RouterModule.forChild([{ path: "", component: HomeComponent }]),
        SharedModule,
        TranslationModule.forChild()
    ],
    exports: [HomeComponent]
})
export class HomeModule { }