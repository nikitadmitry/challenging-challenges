import "angular2-universal-polyfills/browser";
import { enableProdMode } from "@angular/core";
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from "./app/app.module";
import "rxjs/add/operator/map";
import './assets/locale-en.json';
import './assets/locale-ru.json';

// enable either Hot Module Reloading or production mode
if (module["hot"]) {
    module["hot"].accept();
} else {
    enableProdMode();
}

// boot the application, either now or when the DOM content is loaded
const platform = platformBrowserDynamic();
const bootApplication = () => { platform.bootstrapModule(AppModule); };
if (document.readyState === "complete") {
    bootApplication();
} else {
    document.addEventListener("DOMContentLoaded", bootApplication);
}
