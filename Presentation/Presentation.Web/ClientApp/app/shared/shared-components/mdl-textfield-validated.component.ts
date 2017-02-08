import { Component, Renderer, ElementRef, Optional, Inject, NgModule, forwardRef, AfterContentChecked, Input } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, NG_VALUE_ACCESSOR, ControlValueAccessor, FormControl } from "@angular/forms";
import { MdlTextFieldComponent, DISABLE_NATIVE_VALIDITY_CHECKING, MdlTextFieldModule } from "angular2-mdl";

import { FormControlValidationMessagesBuilder } from "../validation/FormControlValidationMessagesBuilder";

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
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => MdlTextFieldValidatedComponent),
    multi: true
  },
  FormControlValidationMessagesBuilder],
})
export class MdlTextFieldValidatedComponent extends MdlTextFieldComponent implements ControlValueAccessor, AfterContentChecked {
  private error: string;

  @Input()
  formControl: FormControl;

  constructor(renderer: Renderer, elmRef: ElementRef,
    @Optional() @Inject(DISABLE_NATIVE_VALIDITY_CHECKING) nativeCheckGlobalDisabled: Boolean,
    private formControlValidationMessagesBuilder: FormControlValidationMessagesBuilder) {
      super(renderer, elmRef, nativeCheckGlobalDisabled);
  }

  ngAfterContentChecked(): void {
    this.updateError();
  }

  private updateError(): void {
    var errorMessages: Array<string> = this.formControlValidationMessagesBuilder.build(this.formControl);

    this.error = errorMessages && errorMessages.length > 0 ? errorMessages[0] : undefined;
  }
}

@NgModule({
  imports: [CommonModule, FormsModule, MdlTextFieldModule],
  exports: [MdlTextFieldValidatedComponent],
  declarations: [MdlTextFieldValidatedComponent],
})
export class MdlTextFieldValidatedModule {

}
