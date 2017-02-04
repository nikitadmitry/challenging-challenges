import { NgModule } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { AuthHttp, AuthConfig } from 'angular2-jwt';
import { LoginComponent } from './login/login.component';
import { RouterModule, Routes } from '@angular/router';

function authHttpServiceFactory(authConfig: AuthConfig, http: Http, options: RequestOptions) {
  return new AuthHttp(authConfig, http, options);
}

@NgModule({
  // declarations: [
  //   LoginComponent
  // ],
  // imports: [
  //   RouterModule.forChild([
  //     { path: 'login', component: LoginComponent }
  //   ])
  // ],
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
  ]
})
export class AuthModule { }
