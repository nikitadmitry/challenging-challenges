import {Component, ViewChild, ElementRef, OnChanges} from "@angular/core";
import { AceEditorComponent } from 'ng2-ace-editor';
import {Translation, TranslationService} from "angular-l10n";
import "brace";
import "brace/mode/csharp";
import "brace/mode/java";
import "brace/mode/ruby";
import "brace/mode/python";
import "brace/mode/text";
import "brace/theme/eclipse";
import * as SimpleMDE from "simplemde";

import {ChallengesService} from "../../challenges/challenges.service";
import {FormGroup, Validators, FormBuilder} from "@angular/forms";
import {EnumSelectService} from "../../shared/services/enum-select.service";
import {Section} from "../models/Section";
import {Difficulty} from "../models/Difficulty";
import {Language} from "../models/Language";
import {MdlSelectComponent} from "@angular2-mdl-ext/select";

@Component({
    selector: "new-challenge",
    template: require("./new-challenge.component.html"),
    styles: [require("./new-challenge.component.css")],
    providers: [ChallengesService,EnumSelectService]
})
export class NewChallengeComponent extends Translation {
    challengeForm: FormGroup;
    @ViewChild(AceEditorComponent) editor: AceEditorComponent;
    sections: any[];
    difficulties: any[];
    languages: any[];
    @ViewChild("sectionSelect") sectionSelect: MdlSelectComponent;
    @ViewChild('previewEditor') previewEditorElement : ElementRef;
    @ViewChild('conditionEditor') conditionEditorElement : ElementRef;
    previewEditor: any;
    conditionEditor: any;
    answer: string;

    constructor(private challengesService: ChallengesService,
                translationService: TranslationService,
                private fb: FormBuilder,
                private enumSelectService: EnumSelectService
    ) {
        super(translationService);

        this.challengeForm = this.fb.group({
            title: [null, Validators.compose([Validators.required, Validators.minLength(4), Validators.maxLength(30)])],
            section: [null, Validators.compose([Validators.required])],
            difficulty: [null, Validators.compose([Validators.required])],
            language: [null, Validators.compose([Validators.required])],
            codeAnswered: [false, Validators.compose([Validators.required])],
            previewText: [null, Validators.compose([Validators.required])],
            condition: [null, Validators.required],
            answers: [[]],
            sourceCode: [null, Validators.required],
            tags: [[]]
        });

        this.translation.AddConfiguration()
            .AddProvider("./assets/locale-challenges-");
        this.translation.init();

        this.translation.translationChanged.subscribe(() => {
            this.sections = this.enumSelectService.convertToSelectValues(Section, "section");
            this.difficulties = this.enumSelectService.convertToSelectValues(Difficulty, "difficulty");
            this.languages = this.enumSelectService.convertToSelectValues(Language, "language");

            this.challengeForm.get('section').setValue(Section.CSharp);
            this.challengeForm.get('difficulty').setValue(Difficulty.Intermediate);
            this.challengeForm.get('language').setValue(Language.English);
        });
    }

    ngOnInit(): void {
        this.challengeForm.get('section').valueChanges.subscribe(() => {
           this.answer = "Updated to: " + this.challengeForm.get('section').value;
        });

        this.conditionEditor = new SimpleMDE({
            element: this.conditionEditorElement.nativeElement.value,
            placeholder: "Condition",
            toolbar: ["bold", "italic", "heading", "|", "code", "quote", "|", "preview"]
        });
        this.conditionEditor.codemirror.on("change", () => {
            this.challengeForm.get('condition').setValue(this.conditionEditor.value())
        });

        this.previewEditor = new SimpleMDE({
            element: this.previewEditorElement.nativeElement.value,
            placeholder: "Preview Text",
            toolbar: ["bold", "italic", "heading", "|", "preview"]
        });
        this.previewEditor.codemirror.on("change", () => {
            this.challengeForm.get('previewText').setValue(this.previewEditor.value());
        });

        this.editor.getEditor().$blockScrolling = Infinity;
        this.editor.setTheme("eclipse");
        this.editor.textChanged.subscribe(() => {
            this.challengeForm.get('sourceCode').setValue(this.answer);
        });
    }

    submit() {

    }
}