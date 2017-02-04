import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UniversalModule } from 'angular2-universal';
import { AppComponent } from './components/app/app.component'
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
//import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { ChallengesComponent } from './components/home/challenges/challenges.component';
import { PaginatorModule } from 'primeng/primeng';
import { AuthModule } from './auth.module';

@NgModule({
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        //FetchDataComponent,
        ChallengesComponent,
        HomeComponent,
        AuthModule
    ],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', loadChildren: './components/fetchdata/fetchdata.module#FetchDataModule' /*#FetchDataModule*/ },
            { path: '**', redirectTo: 'home' }
        ]),
        PaginatorModule
    ]
})
export class AppModule {
}
