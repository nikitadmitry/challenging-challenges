import {Component, OnInit, ViewChild} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
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
import {MdlDialogService} from "angular2-mdl";

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
                private editorModeResolver: EditorModeResolver,
                private dialogService: MdlDialogService,
                private router: Router) {
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
        this.editor.setReadOnly(this.challenge.isAuthor || this.challenge.isSolved);
        let answerText: string;
        if (this.challenge.isAuthor) {
            answerText = "You are an author.";
        } else if (this.challenge.isSolved) {
            answerText = "You have solved this challenge.";
        }
        else {
            answerText = this.challenge.answerTemplate;
        }
        this.editor.getEditor().setValue(answerText, 1);

        this.editor.setMode(this.editorModeResolver.resolve(this.challenge.section));
    }

    submit() {
        this.challengesService.solve(this.challenge.id, this.answer).subscribe((response) => {
            if (response.isSolved) {
                this.dialogService.alert(response.ratingObtained + " Rating obtained.", "Close challenge", "Challenge Solved").subscribe(() => {
                    this.router.navigate(["challenges"]);
                });
            } else {
                this.dialogService.alert(response.errorMessage, "Return to challenge", "Wrong!");
            }
        })
    }
}