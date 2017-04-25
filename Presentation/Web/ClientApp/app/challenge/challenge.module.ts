import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { MdlSelectModule } from "@angular-mdl/select";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { TranslationModule } from "angular-l10n";
import { AceEditorDirective, AceEditorComponent } from 'ng2-ace-editor';
import { TagInputModule } from 'ng2-tag-input';

import { ChallengeDetailsComponent } from "./challenge-details/challenge-details.component";
import {ChallengeDescriptionComponent} from "./challenge-details/challenge-description/challenge-description.component";
import {ChallengeActionsComponent} from "./challenge-details/challenge-actions/challenge-actions.component";
import {EditChallengeComponent} from "./edit-challenge/edit-challenge.component";
import {MdlNonRootModule} from "@angular-mdl/core";
import {MdlTextFieldValidatedModule} from "../shared/components/mdl-textfield-validated.component/mdl-textfield-validated.module";
import {LoadingSpinnerModule} from "../shared/components/loading-spinner.component/loading-spinner.module";
import {CommonModule} from "@angular/common";
import {EnumPipeModule} from "../shared/pipes/enum.pipe";
import {MarkdownModule} from "../shared/components/markdown.component";
import {TestCasesComponent} from "./edit-challenge/test-cases/test-cases.component";
import {AnswersComponent} from "./edit-challenge/answers/answers.component";

@NgModule({
    declarations: [
        AceEditorDirective,
        AceEditorComponent,
        ChallengeDetailsComponent,
        ChallengeDescriptionComponent,
        ChallengeActionsComponent,
        TestCasesComponent,
        AnswersComponent,
        EditChallengeComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        EnumPipeModule,
        MarkdownModule,
        MdlNonRootModule,
        LoadingSpinnerModule,
        MdlSelectModule,
        MdlTextFieldValidatedModule,
        TagInputModule,
        RouterModule.forChild([
            { path: "new", component: EditChallengeComponent },
            { path: "edit/:id", component: EditChallengeComponent },
            { path: ":id", component: ChallengeDetailsComponent },
            { path: "**", redirectTo: "home" }
        ]),
        TranslationModule.forChild()
    ]
})
export class ChallengeModule { }