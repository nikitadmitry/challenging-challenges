import { NgModule } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { AuthHttp, AuthConfig } from 'angular2-jwt';
import { RouterModule, Routes } from '@angular/router';
import { MaterialModule } from '@angular/material';

import { LoginComponent } from './login/login.component';
import { LoginDialogComponent } from './login/login-dialog.component';
import { AuthComponent } from './auth.component';

function authHttpServiceFactory(authConfig: AuthConfig, http: Http, options: RequestOptions) {
  return new AuthHttp(authConfig, http, options);
}

@NgModule({
  declarations: [
    LoginComponent,
    LoginDialogComponent,
    AuthComponent
  ],
  providers: [
    {
      provide: AuthConfig,
      useFactory: () => new AuthConfig ({ noJwtError: true })
    },
    {
      provide: AuthHttp,
      useFactory: authHttpServiceFactory,
      deps: [AuthConfig, Http, RequestOptions]
    }
  ],
  entryComponents: [ LoginDialogComponent ],
  imports: [ MaterialModule ]
})
export class AuthModule { }
