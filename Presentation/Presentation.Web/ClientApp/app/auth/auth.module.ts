import { NgModule } from "@angular/core";
import { Http, RequestOptions } from "@angular/http";
import { AuthHttp, AuthConfig } from "angular2-jwt";
import { BrowserModule } from "@angular/platform-browser";
import { MdlNonRootModule } from "angular2-mdl";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { AuthComponent } from "./auth.component";
import { LoginDialogComponent } from "./login/login-dialog.component";

function authHttpServiceFactory(authConfig: AuthConfig, http: Http, options: RequestOptions) {
  return new AuthHttp(authConfig, http, options);
}

@NgModule({
  declarations: [
    AuthComponent,
    LoginDialogComponent
  ],
  providers: [
    { provide: AuthConfig, useFactory: () => new AuthConfig ({ noJwtError: true }) },
    { provide: AuthHttp, useFactory: authHttpServiceFactory, deps: [AuthConfig, Http, RequestOptions] }
  ],
  imports: [
    BrowserModule,
    MdlNonRootModule,
    FormsModule, ReactiveFormsModule
  ],
  exports: [
    AuthComponent
  ],
  entryComponents: [LoginDialogComponent]
})
export class AuthModule { }
