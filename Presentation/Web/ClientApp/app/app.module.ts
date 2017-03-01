import { NgModule, ErrorHandler, APP_INITIALIZER } from "@angular/core";
import { UniversalModule } from "angular2-universal";
import {DISABLE_NATIVE_VALIDITY_CHECKING, MdlModule} from "angular2-mdl";
import { SimpleNotificationsModule, NotificationsService } from "angular2-notifications";
import {TranslationModule, TranslationService} from "angular-l10n";

import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./navmenu/navmenu.component";
import { AuthModule } from "./auth/auth.module";
import { AppRoutingModule } from "./routing/app-routing.module";
import { ApplicationErrorHandler } from "./shared/ApplicationErrorHandler";
import { LocalizationConfig, initLocalization } from "./localization/LocalizationConfig";
import {HomeModule} from "./home/home.module";
import {LoadingSpinnerModule} from "./shared/components/loading-spinner.component/loading-spinner.module";
import {FormsModule} from "@angular/forms";

@NgModule({
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
        NavMenuComponent
    ],
    imports: [
        UniversalModule, // must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        SimpleNotificationsModule.forRoot(),
        TranslationModule.forRoot(),
        FormsModule,
        MdlModule,
        LoadingSpinnerModule,
        AppRoutingModule,
        HomeModule,
        AuthModule
    ],
    providers: [
        LocalizationConfig,
        {
            provide: APP_INITIALIZER,
            useFactory: initLocalization,
            deps: [LocalizationConfig],
            multi: true
        },
        {
            provide: DISABLE_NATIVE_VALIDITY_CHECKING,
            useValue: true
        },
        {
            provide: ErrorHandler,
            useFactory: (notificationsService, translationService) =>
                new ApplicationErrorHandler(notificationsService, translationService),
            deps: [NotificationsService, TranslationService]
        }
    ]
})
export class AppModule {
}
