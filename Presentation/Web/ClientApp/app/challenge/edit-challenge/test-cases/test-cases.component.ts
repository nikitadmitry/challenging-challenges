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
        let testCase = this.fb.group({
            inputParameters: this.fb.array([]),
            outputParameters: this.fb.array([]),
            isPublic: [true]
        });

        this.testCases.push(testCase);
    }

    addInputParameter(testCase: FormGroup) {
        this.addParameter(testCase, "inputParameters");
    }

    addOutputParameter(testCase: FormGroup) {
        this.addParameter(testCase, "outputParameters");
    }

    private addParameter(testCase: FormGroup, collectionName: string) {
        let parameterControl = this.fb.control("", Validators.compose([Validators.required,
            Validators.maxLength(200)]));

        (testCase.get(collectionName) as FormArray).push(parameterControl);
    }

    canAddInputParameter(testCase: FormGroup) {
        return this.canAddParameter(testCase, "inputParameters");
    }

    canAddOutputParameter(testCase: FormGroup) {
        return this.canAddParameter(testCase, "outputParameters");
    }

    private canAddParameter(testCase: FormGroup, collectionName: string): boolean {
        return (testCase.get(collectionName) as FormArray).length < 5;
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