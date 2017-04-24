import {Component, Input, OnInit, Output} from '@angular/core';
import {FormArray, FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
    selector: 'test-cases',
    template: require('./test-cases.component.html')
})
export class TestCasesComponent implements OnInit {
    @Input() testCases: FormArray;

    constructor(private fb: FormBuilder) {
    }

    ngOnInit() {
        this.addTestCase();
    }

    addTestCase() {
        this.testCases.push(this.fb.group({
            input: ["", Validators.maxLength(200)],
            output: ["", Validators.compose([Validators.required, Validators.maxLength(200)])],
            isPublic: [true]
        }));
    }

    canAddTestCase(): boolean {
        return this.testCases.length < 5;
    }

    canDeleteTestCase(): boolean {
        return this.testCases.length > 1;
    }

    deleteTestCase(testCase: any) {
        this.testCases.removeAt(this.testCases.controls.indexOf(testCase));
    }
}