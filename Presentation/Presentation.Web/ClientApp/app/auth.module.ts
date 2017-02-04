import { NgModule } from '@angular/core';
import { Http, RequestOptions } from '@angular/http';
import { AuthHttp, AuthConfig } from 'angular2-jwt';

function authHttpServiceFactory(authConfig: AuthConfig, http: Http, options: RequestOptions) {
  return new AuthHttp(authConfig, http, options);
}

@NgModule({
  providers: [
    {
      provide: AuthConfig,
      useFactory: () => new AuthConfig()
    },
    {
      provide: AuthHttp,
      useFactory: authHttpServiceFactory,
      deps: [AuthConfig, Http, RequestOptions]
    }
  ]
})
export class AuthModule { }
