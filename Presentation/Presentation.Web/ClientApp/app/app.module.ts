import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UniversalModule } from 'angular2-universal';
import { AppComponent } from './app.component'
import { NavMenuComponent } from './navmenu/navmenu.component';
import { HomeComponent } from './home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { ChallengesComponent } from './home/challenges/challenges.component';
import { AuthModule } from './auth/auth.module';
import { GrowlModule } from 'primeng/primeng';
import { MaterialModule } from '@angular/material';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule }   from '@angular/forms';

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
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        AppRoutingModule,
        GrowlModule,
        MaterialModule.forRoot(),
        AuthModule,
        FormsModule
    ]
})
export class AppModule {
}