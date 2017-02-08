import { NgModule, ErrorHandler } from "@angular/core";
import { UniversalModule } from "angular2-universal";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MdlModule, DISABLE_NATIVE_VALIDITY_CHECKING } from "angular2-mdl";
import { SimpleNotificationsModule, NotificationsService } from "angular2-notifications";

import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./navmenu/navmenu.component";
import { HomeComponent } from "./home/home.component";
import { FetchDataComponent } from "./components/fetchdata/fetchdata.component";
import { CounterComponent } from "./components/counter/counter.component";
import { ChallengesComponent } from "./home/challenges/challenges.component";
import { AuthModule } from "./auth/auth.module";
import { AppRoutingModule } from "./app-routing.module";
import { ApplicationErrorHandler } from "./shared/ApplicationErrorHandler";
import { MdlTextFieldValidatedModule } from "./shared/shared-components/mdl-textfield-validated.component";

@NgModule({
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        ChallengesComponent,
        HomeComponent
    ],
    imports: [
        UniversalModule, // must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        AppRoutingModule,
        SimpleNotificationsModule.forRoot(),
        MdlModule,
        AuthModule,
        FormsModule, ReactiveFormsModule,
        MdlTextFieldValidatedModule
    ],
    providers: [
        {
            provide: DISABLE_NATIVE_VALIDITY_CHECKING,
            useValue: true
        },
        {
            provide: ErrorHandler,
            useFactory: (notificationsService) => new ApplicationErrorHandler(notificationsService),
            deps: [NotificationsService]
        }
    ]
})
export class AppModule {
}
