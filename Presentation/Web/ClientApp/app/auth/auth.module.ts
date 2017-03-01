import { NgModule } from "@angular/core";
import { Http, RequestOptions } from "@angular/http";
import { AuthHttp, AuthConfig } from "angular2-jwt";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { TranslationModule } from "angular-l10n";

import { AuthComponent } from "./auth.component";
import { LoginDialogComponent } from "./login/login-dialog.component";
import { RegisterDialogComponent } from "./register/register-dialog.component";
import {MdlNonRootModule} from "angular2-mdl";
import {MdlTextFieldValidatedModule} from "../shared/components/mdl-textfield-validated.component/mdl-textfield-validated.module";

function authHttpServiceFactory(authConfig: AuthConfig, http: Http, options: RequestOptions) {
  return new AuthHttp(authConfig, http, options);
}

@NgModule({
  declarations: [
    AuthComponent,
    LoginDialogComponent,
    RegisterDialogComponent
  ],
  providers: [
    { provide: AuthConfig, useFactory: () => new AuthConfig ({ noJwtError: true }) },
    { provide: AuthHttp, useFactory: authHttpServiceFactory, deps: [AuthConfig, Http, RequestOptions] }
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    TranslationModule,
    MdlTextFieldValidatedModule,
    MdlNonRootModule
  ],
  exports: [
    AuthComponent,
    LoginDialogComponent,
    RegisterDialogComponent
  ],
  entryComponents: [LoginDialogComponent, RegisterDialogComponent]
})
export class AuthModule { }
