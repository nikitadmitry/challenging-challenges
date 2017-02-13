import { ErrorHandler, Injectable } from "@angular/core";
import { NotificationsService } from "angular2-notifications";

@Injectable()
export class ApplicationErrorHandler extends ErrorHandler {
    constructor(private notificationsService: NotificationsService) {
        super();
    }

    handleError(zoneError: any): void {
        if(zoneError.rejection && Array.isArray(zoneError.rejection)) {
            var errors: any[] = zoneError.rejection;

            errors.forEach(error => {
                this.displayError(error);
            });

            return;
        }

        if (zoneError.status === 500) {
            this.displayError("Произошла ошибка на сервере. Попробуйте перезагрузить страницу.");
            return;
        }

        super.handleError(zoneError);
    }

    private displayError(error: string): void {
        setTimeout(() => this.notificationsService.error("Ошибка", error)); // we use setTimeout for dom rerender.
    }
}