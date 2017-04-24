import {Component, Input, OnInit} from '@angular/core';
import {FormArray, FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
    selector: 'answers',
    template: require('./answers.component.html')
})
export class AnswersComponent implements OnInit {
    @Input() answers: FormArray;
    constructor() {
    }

    ngOnInit() {
        this.addAnswer();
    }

    addAnswer() {
        this.answers.push(new FormControl("", Validators.required));
    }

    canAddAnswer(): boolean {
        return this.answers.controls.length < 5;
    }

    canDeleteAnswer(): boolean {
        return this.answers.controls.length > 1;
    }

    deleteAnswer(answer: any) {
        this.answers.removeAt(this.answers.controls.indexOf(answer));
    }
}