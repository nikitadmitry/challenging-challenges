import { NgModule, ErrorHandler } from "@angular/core";
import { UniversalModule } from "angular2-universal";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MdlModule, DISABLE_NATIVE_VALIDITY_CHECKING } from "angular2-mdl";
import { SimpleNotificationsModule, NotificationsService } from "angular2-notifications";
import { Ng2PaginationModule } from "ng2-pagination";

import { AppComponent } from "./app.component";
import { HomeModule } from "./home/home.module";
import { NavMenuComponent } from "./navmenu/navmenu.component";
import { AuthModule } from "./auth/auth.module";
import { AppRoutingModule } from "./routing/app-routing.module";
import { ApplicationErrorHandler } from "./shared/ApplicationErrorHandler";
import { SharedModule } from "./shared/shared.module";
import { ChallengesModule } from "./challenges/challenges.module";

@NgModule({
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
        NavMenuComponent
    ],
    imports: [
        UniversalModule, // must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        SimpleNotificationsModule.forRoot(),
        MdlModule,
        AuthModule,
        HomeModule,
        FormsModule,
        ReactiveFormsModule,
        Ng2PaginationModule,
        SharedModule,
        ChallengesModule,

        AppRoutingModule
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
