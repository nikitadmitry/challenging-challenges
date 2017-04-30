import {Component, Input, OnInit, Output} from '@angular/core';
import {FormArray, FormBuilder, FormControl, FormGroup, ValidationErrors, Validators} from "@angular/forms";
import {FormControlValidationMessagesBuilder} from "../../../shared/validation/FormControlValidationMessagesBuilder";

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
        testCase.setValidators((group: FormGroup) => {
            if ((group.get("outputParameters") as any).controls.length === 0) {
                return {"hasNoOutputParameters": true};
            }
            return null;
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

    deleteInputParameter(testCase: FormGroup, parameter) {
        this.deleteParameter(testCase, parameter, "inputParameters");
    }

    deleteOutputParameter(testCase: FormGroup, parameter) {
        this.deleteParameter(testCase, parameter, "outputParameters");
    }


    private deleteParameter(testCase: FormGroup, parameter, collectionName: string) {
        let parameters = (testCase.get(collectionName) as FormArray);

        parameters.removeAt(parameters.controls.indexOf(parameter));
    }

    canAddInputParameter(testCase: FormGroup) {
        return this.canAddParameter(testCase, "inputParameters");
    }

    canAddOutputParameter(testCase: FormGroup) {
        return this.canAddParameter(testCase, "outputParameters");
    }

    private canAddParameter(testCase: FormGroup, collectionName: string): boolean {
        return (testCase.get(collectionName) as FormArray).length < 3;
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