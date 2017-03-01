import {
    Component, Renderer, ElementRef, Optional, Inject, forwardRef, Input, ViewEncapsulation, OnChanges, OnInit
} from "@angular/core";
import {
    NG_VALUE_ACCESSOR, FormControl, ControlValueAccessor
} from "@angular/forms";
import { MdlTextFieldComponent, DISABLE_NATIVE_VALIDITY_CHECKING } from "angular2-mdl";

import { FormControlValidationMessagesBuilder } from "../../validation/FormControlValidationMessagesBuilder";

@Component({
    selector: "mdl-textfield-validated",
    host: {
        "[class.mdl-textfield]": "true",
        "[class.is-upgraded]": "true",
        "[class.mdl-textfield--expandable]": "icon",
        "[class.mdl-textfield--floating-label]": "isFloatingLabel",
        "[class.has-placeholder]": "placeholder"
    },
    template: require("./mdl-textfield-validated.component.html"),
    styles: [require("./mdl-textfield-validated.component.css")],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => MdlTextFieldValidatedComponent),
            multi: true
        },
        FormControlValidationMessagesBuilder
    ],
    encapsulation: ViewEncapsulation.None
})
export class MdlTextFieldValidatedComponent extends MdlTextFieldComponent {
    private error: string;
    @Input() formControl: FormControl;

    constructor(renderer: Renderer, elmRef: ElementRef,
        @Optional() @Inject(DISABLE_NATIVE_VALIDITY_CHECKING) nativeCheckGlobalDisabled: Boolean,
        private formControlValidationMessagesBuilder: FormControlValidationMessagesBuilder) {
        super(renderer, elmRef, nativeCheckGlobalDisabled);
    }

    public registerOnChange(fn: any): void {
        super.registerOnChange((value, e) => {
            fn(value, e);
            this.formControl.setValue(value);
            this.updateError();
        });
    }

    private updateError(): void {
        var errorMessages: Array<string> = this.formControlValidationMessagesBuilder.build(this.formControl);

        this.error = errorMessages && errorMessages.length > 0 ? errorMessages[0] : undefined;
    }
}