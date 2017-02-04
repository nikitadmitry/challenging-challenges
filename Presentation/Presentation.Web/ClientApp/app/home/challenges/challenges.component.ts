import { Component, Input } from '@angular/core';

@Component({
    selector: 'challenges',
    template: require('./challenges.component.html')
})
export class ChallengesComponent {
    @Input()
    public sorting: number;
    
}
