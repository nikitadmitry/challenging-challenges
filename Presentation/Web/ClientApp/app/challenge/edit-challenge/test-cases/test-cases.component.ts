import {Component, Input, OnInit, Output} from '@angular/core';

@Component({
    selector: 'test-cases',
    template: require('./test-cases.component.html')
})
export class TestCasesComponent implements OnInit {
    @Input() testCases: Array<any>;
    constructor() {

    }

    ngOnInit() {
        this.addTestCase();
    }

    addTestCase() {
        this.testCases.push({
            input: "",
            output: "",
            isPublic: true
        });
    }

    canAddTestCase(): boolean {
        return this.testCases.length < 5;
    }

    deleteTestCase(testCase: any) {
        this.testCases.splice(this.testCases.indexOf(testCase), 1);
    }
}