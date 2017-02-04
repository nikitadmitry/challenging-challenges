import { Component, OnDestroy } from '@angular/core';
import { Messages } from './shared/messages';

@Component({
    selector: 'app',
    template: require('./app.component.html'),
    styles: [require('./app.component.css')]
})
export class AppComponent implements OnDestroy {
    messages = Messages;

    ngOnDestroy(): void {
        document.body.appendChild(document.createElement("app"));
    }
}
