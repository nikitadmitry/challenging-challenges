import { NgModule } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { AuthHttp, AuthConfig } from 'angular2-jwt';
import { RouterModule, Routes } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
//import { MdlNonRootModule } from 'angular2-mdl';

import { AuthComponent } from './auth.component';
import { LoginComponent } from './login/login.component';
import { LoginDialogComponent } from './login/login-dialog.component';

function authHttpServiceFactory(authConfig: AuthConfig, http: Http, options: RequestOptions) {
  return new AuthHttp(authConfig, http, options);
}

@NgModule({
  declarations: [
    AuthComponent,
    LoginComponent,
    LoginDialogComponent
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
  imports: [ 
    BrowserModule,
    //MdlNonRootModule.forRoot()
  ],
  exports: [
    AuthComponent
  ]
})
export class AuthModule { }
