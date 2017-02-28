import {Component, ViewChild} from "@angular/core";
import { AceEditorComponent } from 'ng2-ace-editor';
import {Translation, TranslationService} from "angular-l10n";
import "brace";
import "brace/mode/csharp";
import "brace/mode/java";
import "brace/mode/ruby";
import "brace/mode/python";
import "brace/mode/text";
import "brace/theme/eclipse";

import {ChallengesService} from "../../challenges/challenges.service";
import {FormGroup, Validators, FormBuilder} from "@angular/forms";

@Component({
    selector: "new-challenge",
    template: require("./new-challenge.component.html"),
    styles: [require("./new-challenge.component.css")],
    providers: [ChallengesService]
})
export class NewChallengeComponent extends Translation {
    challengeForm: FormGroup;
    @ViewChild(AceEditorComponent) editor: AceEditorComponent;

    constructor(private challengesService: ChallengesService,
                translationService: TranslationService,
                private fb: FormBuilder) {
        super(translationService);

        this.translation.AddConfiguration()
            .AddProvider("./assets/locale-challenges-");
        this.translation.init();
    }

    ngOnInit(): void {
        this.challengeForm = this.fb.group({
            userName: ["", Validators.compose([Validators.required, Validators.minLength(4), Validators.maxLength(24)])],
            email: ["", Validators.compose([Validators.required])],
            password: ["", Validators.compose([Validators.required, Validators.minLength(6), Validators.maxLength(100)])],
            confirmPassword: ["", Validators.compose([Validators.required])]
        });
    }

    submit() {

    }
}