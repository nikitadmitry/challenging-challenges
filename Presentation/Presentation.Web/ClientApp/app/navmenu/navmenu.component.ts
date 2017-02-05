import { Component, ViewEncapsulation, Input } from '@angular/core';
import { MdlLayoutComponent } from 'angular2-mdl';

@Component({
    selector: 'nav-menu',
    template: require('./navmenu.component.html'),
    styles: [require('./navmenu.component.css')],
    encapsulation: ViewEncapsulation.None
})
export class NavMenuComponent {
    @Input()
    navLayout: MdlLayoutComponent;
    
}
