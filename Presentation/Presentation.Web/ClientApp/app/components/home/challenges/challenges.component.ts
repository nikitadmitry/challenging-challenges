import { Component, Input } from '@angular/core';
//import { PaginatorModule } from 'primeng/primeng';

@Component({
    selector: 'challenges',
    template: require('./challenges.component.html')
})
export class ChallengesComponent {
    @Input()
    public sorting: number;
    
}
