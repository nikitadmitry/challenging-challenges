import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { LoginComponent } from './auth/login/login.component';

const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: HomeComponent },
    { path: 'counter', component: CounterComponent },
    { path: 'fetch-data', component: FetchDataComponent },//'./components/fetchdata/fetchdata.module#FetchDataModule' /*#FetchDataModule*/ },
    { path: 'login', component: LoginComponent, outlet: 'popup' },
    { path: '**', redirectTo: 'home' }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}