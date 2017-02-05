import { Component, OnDestroy, ViewEncapsulation } from '@angular/core';
//import { Messages } from './shared/messages';

@Component({
    selector: 'app',
    template: require('./app.component.html'),
    styles: [require('./app.component.css')],
    encapsulation: ViewEncapsulation.None   
})
export class AppComponent implements OnDestroy {
    //messages = Messages;

    ngOnDestroy(): void {
        document.body.appendChild(document.createElement("app"));
    }
}
