import { NgModule } from '@angular/core';
import { ProfileComponent } from './profile/profile.component';
import {CommonModule} from "@angular/common";
import {TranslationModule} from "angular-l10n";
import {RouterModule} from "@angular/router";
import {LoadingSpinnerModule} from "../shared/components/loading-spinner.component/loading-spinner.module";
import {MdlNonRootModule} from "angular2-mdl";
import {FormsModule} from "@angular/forms";

@NgModule({
    imports: [
        CommonModule,
        MdlNonRootModule,
        FormsModule,
        LoadingSpinnerModule,
        RouterModule.forChild([
            { path: ":id", component: ProfileComponent },
            { path: "**", redirectTo: "home" }
        ]),
        TranslationModule.forChild()
    ],
    exports: [],
    declarations: [ProfileComponent],
    providers: [],
})
export class UserModule { }
