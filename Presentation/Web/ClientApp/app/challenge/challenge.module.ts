import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { MdlSelectModule } from "@angular2-mdl-ext/select";
import { FormsModule } from "@angular/forms";
import { TranslationModule } from "angular-l10n";
import { AceEditorDirective, AceEditorComponent } from 'ng2-ace-editor';

import { SharedModule } from "../shared/shared.module";
import { ChallengeDetailsComponent } from "./challenge-details/challenge-details.component";
import {ChallengeDescriptionComponent} from "./challenge-details/challenge-description/challenge-description.component";
import {ChallengeActionsComponent} from "./challenge-details/challenge-actions/challenge-actions.component";

@NgModule({
    declarations: [
        AceEditorDirective,
        AceEditorComponent,
        ChallengeDetailsComponent,
        ChallengeDescriptionComponent,
        ChallengeActionsComponent
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