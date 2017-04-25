import { NgModule, ErrorHandler, APP_INITIALIZER } from "@angular/core";
import {DISABLE_NATIVE_VALIDITY_CHECKING, MdlModule} from "@angular-mdl/core";
import { SimpleNotificationsModule, NotificationsService } from "angular2-notifications";
import {TranslationModule, TranslationService} from "angular-l10n";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./navmenu/navmenu.component";
import { AuthModule } from "./auth/auth.module";
import { AppRoutingModule } from "./routing/app-routing.module";
import { ApplicationErrorHandler } from "./shared/ApplicationErrorHandler";
import { LocalizationConfig, initLocalization } from "./localization/LocalizationConfig";
import {HomeModule} from "./home/home.module";
import {LoadingSpinnerModule} from "./shared/components/loading-spinner.component/loading-spinner.module";
import {FormsModule} from "@angular/forms";
import {BrowserModule} from "@angular/platform-browser";
import {HttpModule, JsonpModule} from "@angular/http";

@NgModule({
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
        NavMenuComponent
    ],
    imports: [
        BrowserModule,
        HttpModule,
        JsonpModule,
        BrowserAnimationsModule,
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
