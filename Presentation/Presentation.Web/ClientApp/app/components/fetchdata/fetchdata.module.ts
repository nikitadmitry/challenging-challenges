import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FetchDataComponent }   from './fetchdata.component';
import { CommonModule } from '@angular/common'

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild([
            { path: '', component: FetchDataComponent }
        ])
    ],
    declarations: [FetchDataComponent]
})
export class FetchDataModule { }