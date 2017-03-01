import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { MdlSelectModule } from "@angular2-mdl-ext/select";
import { FormsModule } from "@angular/forms";
import { TranslationModule } from "angular-l10n";
import { AceEditorDirective, AceEditorComponent } from 'ng2-ace-editor';

import { ChallengeDetailsComponent } from "./challenge-details/challenge-details.component";
import {ChallengeDescriptionComponent} from "./challenge-details/challenge-description/challenge-description.component";
import {ChallengeActionsComponent} from "./challenge-details/challenge-actions/challenge-actions.component";
import {NewChallengeComponent} from "./new-challenge/new-challenge.component";
import {MdlNonRootModule} from "angular2-mdl";
import {MdlTextFieldValidatedModule} from "../shared/components/mdl-textfield-validated.component/mdl-textfield-validated.module";
import {LoadingSpinnerModule} from "../shared/components/loading-spinner.component/loading-spinner.module";
import {CommonModule} from "@angular/common";
import {EnumPipeModule} from "../shared/pipes/enum.pipe";
import {MarkdownModule} from "../shared/components/markdown.component";

@NgModule({
    declarations: [
        AceEditorDirective,
        AceEditorComponent,
        ChallengeDetailsComponent,
        ChallengeDescriptionComponent,
        ChallengeActionsComponent,
        NewChallengeComponent
    ],
    imports: [
        CommonModule,
        EnumPipeModule,
        MarkdownModule,
        MdlNonRootModule,
        LoadingSpinnerModule,
        MdlSelectModule,
        MdlTextFieldValidatedModule,
        FormsModule,
        RouterModule.forChild([
            { path: "new", component: NewChallengeComponent },
            { path: ":id", component: ChallengeDetailsComponent }
        ]),
        TranslationModule.forChild()
    ]
})
export class ChallengeModule { }