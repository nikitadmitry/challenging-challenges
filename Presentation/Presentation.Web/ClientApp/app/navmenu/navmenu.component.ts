import { Component, ViewEncapsulation } from '@angular/core';

@Component({
    selector: 'nav-menu',
    template: require('./navmenu.component.html'),
    styles: [require('./navmenu.component.css')],
    encapsulation: ViewEncapsulation.None
})
export class NavMenuComponent {
}
