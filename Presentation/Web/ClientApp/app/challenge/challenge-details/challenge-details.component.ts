import {Component, OnInit, ViewChild} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import { AceEditorComponent } from 'ng2-ace-editor';
import {Translation, TranslationService} from "angular-l10n";
import "brace";
import "brace/mode/csharp";
import "brace/mode/java";
import "brace/mode/ruby";
import "brace/mode/python";
import "brace/mode/text";
import "brace/theme/eclipse";

import {ChallengeDetailsModel} from "../models/challenge.model";
import {ChallengesService} from "../../challenges/challenges.service";
import {EditorModeResolver} from "../services/editor-mode-resolver.service";

@Component({
    selector: "challenge-details",
    template: require("./challenge-details.component.html"),
    styles: [require("./challenge-details.component.css")],
    providers: [ChallengesService, EditorModeResolver]
})
export class ChallengeDetailsComponent extends Translation implements OnInit {
    challenge: ChallengeDetailsModel;
    @ViewChild(AceEditorComponent) editor: AceEditorComponent;
    answer: string;

    editorOptions: any = {

    };

    constructor(private route: ActivatedRoute,
                private challengesService: ChallengesService,
                translationService: TranslationService,
                private editorModeResolver: EditorModeResolver) {
        super(translationService);

        this.translation.AddConfiguration()
            .AddProvider("./assets/locale-challenges-");
        this.translation.init();
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            let challengeId = params['id'];

            this.challengesService.getChallenge(challengeId).subscribe((challenge) => {
                this.challenge = challenge;
                this.prepareView();
            })
        });

        this.editor.getEditor().commands.addCommand({
            name: "submit",
            bindKey: "Alt-Enter",
            exec: () => this.submit()
        });

        this.editor.getEditor().$blockScrolling = Infinity;
        this.editor.setTheme("eclipse");
    }

    private prepareView() {
        this.editor.setReadOnly(this.challenge.isAuthor);
        if (!this.challenge.isAuthor) {
            this.editor.getEditor().setValue(this.challenge.answerTemplate, 1);
        }
        this.editor.setMode(this.editorModeResolver.resolve(this.challenge.section));
    }

    submit() {
        this.challengesService.solve(this.challenge.id, this.answer).subscribe((response) => {
            debugger;
            console.log(response);
        })
    }
}