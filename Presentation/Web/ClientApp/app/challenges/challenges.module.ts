import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { MdlSelectModule } from "@angular-mdl/select";
import { FormsModule } from "@angular/forms";
import { TranslationModule } from "angular-l10n";

import { ChallengesComponent } from "./challenges.component";
import { ChallengeComponent } from "./challenge/challenge.component";
import { FiltersComponent } from "./filters/filters.component";
import { PagingButtonsComponent } from "./paging-buttons/paging-buttons.component";
import {MdlNonRootModule} from "@angular-mdl/core";
import {LoadingSpinnerModule} from "../shared/components/loading-spinner.component/loading-spinner.module";
import {CommonModule} from "@angular/common";
import {MarkdownModule} from "../shared/components/markdown.component";
import {EnumPipeModule} from "../shared/pipes/enum.pipe";

@NgModule({
    declarations: [
        ChallengeComponent,
        ChallengesComponent,
        FiltersComponent,
        PagingButtonsComponent
    ],
    imports: [
        CommonModule,
        MarkdownModule,
        EnumPipeModule,
        MdlNonRootModule,
        LoadingSpinnerModule,
        FormsModule,
        MdlSelectModule,
        RouterModule.forChild([
            { path: "", component: ChallengesComponent },
            { path: ":searchText", component: ChallengesComponent },
            { path: "**", redirectTo: "" }
        ]),
        TranslationModule.forChild()
    ]
})
export class ChallengesModule { }