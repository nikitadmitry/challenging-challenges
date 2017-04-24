import {Component, ViewChild, ElementRef, OnInit} from "@angular/core";
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
import {FormGroup, Validators, FormBuilder, FormControl, FormArray} from "@angular/forms";
import {EnumSelectService} from "../../shared/services/enum-select.service";
import {Section} from "../models/Section";
import {Difficulty} from "../models/Difficulty";
import {Language} from "../models/Language";
import {MdlSelectComponent} from "@angular2-mdl-ext/select";
import {FormControlValidationMessagesBuilder} from "../../shared/validation/FormControlValidationMessagesBuilder";
import {EditorModeResolver} from "../services/editor-mode-resolver.service";

@Component({
    selector: "edit-challenge",
    template: require("./edit-challenge.component.html"),
    styles: [require("./edit-challenge.component.css")],
    providers: [
        ChallengesService,EnumSelectService,
        FormControlValidationMessagesBuilder,
        EditorModeResolver
    ]
})
export class EditChallengeComponent extends Translation implements OnInit {
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
    codeAnswer: string;
    testCases = [];

    constructor(private challengesService: ChallengesService,
                translationService: TranslationService,
                private fb: FormBuilder,
                private enumSelectService: EnumSelectService,
                private errorBuilder: FormControlValidationMessagesBuilder,
                private editorModeResolver: EditorModeResolver
    ) {
        super(translationService);

        this.challengeForm = this.fb.group({
            title: [null, Validators.compose([Validators.required, Validators.minLength(4), Validators.maxLength(80)])],
            section: [null, Validators.compose([Validators.required])],
            difficulty: [null, Validators.compose([Validators.required])],
            language: [null, Validators.compose([Validators.required])],
            previewText: [null, Validators.compose([Validators.required, Validators.minLength(50), Validators.maxLength(600)])],
            condition: [null, Validators.compose([Validators.required, Validators.minLength(50), Validators.maxLength(3000)])],
            codeAnswered: [true, Validators.required],
            sourceCode: [null, Validators.required],
            tags: [[]],
            answers: this.fb.array([], Validators.compose([Validators.minLength(1), Validators.maxLength(5), (fa: FormArray) => {
                let numberOfControls = fa.controls.length;

                if (numberOfControls < 1 || numberOfControls > 5) {
                    return { numberOfAnswersIsInvalid: true };
                }

                return null;
            }]))
        });
        (Window as any).form = this.challengeForm;

        this.translation.AddConfiguration()
            .AddProvider("./assets/locale-challenges-");
        this.translation.init();
    }

    getErrorForControl(fc: FormControl): string {
        let errors = this.errorBuilder.build(fc);
        return errors.length > 0 ? errors[0] : undefined
    }

    ngAfterViewInit() {
        this.previewEditor = new SimpleMDE({
            element: this.previewEditorElement.nativeElement,
            placeholder: "Preview Text",
            toolbar: ["bold", "italic", "|", "preview"],
            forceSync: true
        });
        this.previewEditor.codemirror.on("change", () => {
            this.challengeForm.get('previewText').setValue(this.previewEditor.value());
        });

        this.conditionEditor = new SimpleMDE({
            element: this.conditionEditorElement.nativeElement,
            placeholder: "Condition",
            toolbar: ["bold", "italic", "heading", "|", "code", "quote", "|", "preview"],
            forceSync: true
        });
        this.conditionEditor.codemirror.on("change", () => {
            this.challengeForm.get('condition').setValue(this.conditionEditor.value())
        });
    }

    ngOnInit(): void {
        this.initCodeEditor();
        this.initTemplateLoading();
        this.initLocalizations();
    }

    private initTemplateLoading() {
        this.challengeForm.get('codeAnswered').valueChanges.subscribe((value) => {
            if (value && this.challengeForm.get('section').value === Section.Other) {
                this.challengeForm.get('section').setValue(Section.CSharp);
            }
        });

        this.challengeForm.get('section').valueChanges.subscribe((value) => {
            if (value === Section.Other && this.challengeForm.get('codeAnswered').value) {
                this.challengeForm.get('codeAnswered').setValue(false);
            }
            this.editor.setMode(this.editorModeResolver.resolve(value));
            this.challengesService.getSourceCodeTemplate(value).subscribe((codeTemplate) => {
                this.editor.getEditor().setValue(codeTemplate, 1);
            });
        });
    }

    private initCodeEditor() {
        this.editor.getEditor().$blockScrolling = Infinity;
        this.editor.setTheme("eclipse");
        this.editor.setText("");
        this.editor.textChange.subscribe(() => {
            this.challengeForm.get('sourceCode').setValue(this.codeAnswer);
        });
    }

    private initLocalizations() {
        this.translation.translationChanged.subscribe(() => {
            this.sections = this.enumSelectService.convertToSelectValues(Section, "section");
            this.difficulties = this.enumSelectService.convertToSelectValues(Difficulty, "difficulty");
            this.languages = this.enumSelectService.convertToSelectValues(Language, "language");

            this.challengeForm.get('section').setValue(Section.CSharp);
            this.challengeForm.get('difficulty').setValue(Difficulty.Intermediate);
            this.challengeForm.get('language').setValue(Language.English);
        });
    }

    submit() {
        let saveModel = this.challengeForm.value;
        if (this.challengeForm.get("codeAnswered").value) {
            saveModel.testCases = this.testCases;
        } else {
            //aveModel.answers = this.answers;
        }

        debugger;
    }


}