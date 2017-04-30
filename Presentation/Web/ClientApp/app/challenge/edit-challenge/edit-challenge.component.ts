import {Component, ViewChild, ElementRef, OnInit, AfterViewInit, OnDestroy} from "@angular/core";
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
import {FormGroup, Validators, FormBuilder, ValidationErrors, FormArray} from "@angular/forms";
import {EnumSelectService} from "../../shared/services/enum-select.service";
import {Section} from "../models/Section";
import {Difficulty} from "../models/Difficulty";
import {Language} from "../models/Language";
import {MdlSelectComponent} from "@angular-mdl/select";
import {FormControlValidationMessagesBuilder} from "../../shared/validation/FormControlValidationMessagesBuilder";
import {EditorModeResolver} from "../services/editor-mode-resolver.service";
import {Subject} from "rxjs";
import {debug} from "util";

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
export class EditChallengeComponent extends Translation implements OnInit, AfterViewInit, OnDestroy {
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
    private ngUnsubscribe: Subject<void> = new Subject<void>();

    constructor(private challengesService: ChallengesService,
                translationService: TranslationService,
                private fb: FormBuilder,
                private enumSelectService: EnumSelectService,
                private errorBuilder: FormControlValidationMessagesBuilder,
                private editorModeResolver: EditorModeResolver
    ) {
        super(translationService);

        this.constructForm();

        this.translation.AddConfiguration()
            .AddProvider("./assets/locale-")
            .AddProvider("./assets/locale-challenges-");
        this.translation.init();
    }

    ngOnInit(): void {
        this.subscribeToAnswerTypeChange();
        this.initCodeEditor();
        this.initLocalizations();
        this.initTemplateLoading();
    }

    ngAfterViewInit() {
        this.createPreviewEditor();
        this.createConditionEditor();

        this.toggleConditionalField(this.challengeForm.get('codeAnswered').value);
        this.loadTemplate(this.challengeForm.get('section').value);
    }

    ngOnDestroy() {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();

        this.previewEditor.codemirror.off("change");
        this.conditionEditor.codemirror.off("change");
    }

    private constructForm() {
        this.challengeForm = this.fb.group({
            idNew: [true],
            id: [null],
            title: [null, Validators.compose([Validators.required, Validators.minLength(4), Validators.maxLength(80)])],
            section: [Section.CSharp, Validators.compose([Validators.required])],
            difficulty: [Difficulty.Intermediate, Validators.compose([Validators.required])],
            language: [Language.English, Validators.compose([Validators.required])],
            previewText: [null, Validators.compose([Validators.required, Validators.minLength(50), Validators.maxLength(600)])],
            condition: [null, Validators.compose([Validators.required, Validators.minLength(50), Validators.maxLength(3000)])],
            codeAnswered: [true, Validators.required],
            sourceCode: [null, Validators.required],
            tags: [[]],
            answers: this.fb.array([]),
            testCases: this.fb.array([], (array: FormArray) => {
                for (let control of array.controls) {
                    if (control.get("isPublic").value) {
                        return null;
                    }
                }
                return {"hasNoPublicTestCases": true};
            })
        });
    }

    private createConditionEditor() {
        this.conditionEditor = new SimpleMDE({
            element: this.conditionEditorElement.nativeElement,
            placeholder: "Condition",
            toolbar: ["bold", "italic", "heading", "|", "code", "quote", "|", "preview"],
            forceSync: true
        });
        this.conditionEditor.codemirror.on("change", () => {
            this.challengeForm.get('condition').setValue(this.conditionEditor.value());
            this.challengeForm.get('condition').markAsTouched();
        });
    }

    private createPreviewEditor() {
        this.previewEditor = new SimpleMDE({
            element: this.previewEditorElement.nativeElement,
            placeholder: "Preview Text",
            toolbar: ["bold", "italic", "|", "preview"],
            forceSync: true
        });
        this.previewEditor.codemirror.on("change", () => {
            this.challengeForm.get('previewText').setValue(this.previewEditor.value());
            this.challengeForm.get('previewText').markAsTouched();
        });
    }

    private initTemplateLoading() {
        this.challengeForm.get('section').valueChanges
            .takeUntil(this.ngUnsubscribe)
            .subscribe((section: Section) => {
                if (section === Section.Other && this.challengeForm.get('codeAnswered').value) {
                    this.challengeForm.get('codeAnswered').setValue(false);
                }

                this.loadTemplate(section);
            });
    }

    private loadTemplate(section: Section) {
        this.editor.setMode(this.editorModeResolver.resolve(section));

        this.challengesService.getSourceCodeTemplate(section)
            .takeUntil(this.ngUnsubscribe)
            .subscribe((codeTemplate) => this.editor.getEditor().setValue(codeTemplate, 1));
    }

    private subscribeToAnswerTypeChange() {
        this.challengeForm.get('codeAnswered').valueChanges
            .takeUntil(this.ngUnsubscribe)
            .subscribe((isCodeAnswered) => {
                if (isCodeAnswered && this.challengeForm.get('section').value === Section.Other) {
                    this.challengeForm.get('section').setValue(Section.CSharp);
                }

                this.toggleConditionalField(isCodeAnswered);
            });
    }

    private toggleConditionalField(isCodeAnswered) {
        this.challengeForm.get(isCodeAnswered ? "answers" : "testCases").disable();
        this.challengeForm.get(!isCodeAnswered ? "answers" : "testCases").enable();
    }

    private initCodeEditor() {
        this.editor.getEditor().$blockScrolling = Infinity;
        this.editor.setTheme("eclipse");
        this.editor.setText("");
        this.editor.textChange.takeUntil(this.ngUnsubscribe).subscribe(() => {
            this.challengeForm.get('sourceCode').setValue(this.editor.text);
        });
    }

    private initLocalizations() {
        this.translation.translationChanged
            .takeUntil(this.ngUnsubscribe)
            .subscribe(() => {
                this.sections = this.enumSelectService.convertToSelectValues(Section, "section");
                this.difficulties = this.enumSelectService.convertToSelectValues(Difficulty, "difficulty");
                this.languages = this.enumSelectService.convertToSelectValues(Language, "language");
            });
    }

    submit() {
        let saveModel = this.challengeForm.value;

        debugger;
    }


}