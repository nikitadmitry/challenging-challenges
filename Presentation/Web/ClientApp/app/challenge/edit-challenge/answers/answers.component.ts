import {Component, Input, OnInit} from '@angular/core';

@Component({
    selector: 'answers',
    template: require('./answers.component.html')
})
export class AnswersComponent implements OnInit {
    @Input() answers: Array<any>;
    constructor() {

    }

    ngOnInit() {
        this.addAnswer();
    }

    addAnswer() {
        this.answers.push({value: ""});
    }

    canAddAnswer(): boolean {
        return this.answers.length < 5;
    }

    deleteAnswer(answer: any) {
        this.answers.splice(this.answers.indexOf(answer), 1);
    }
}