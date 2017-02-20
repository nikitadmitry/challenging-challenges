import { ErrorHandler, Injectable } from "@angular/core";
import { NotificationsService } from "angular2-notifications";
import {TranslationService} from "angular-l10n";

@Injectable()
export class ApplicationErrorHandler extends ErrorHandler {
    constructor(private notificationsService: NotificationsService,
                private translationService: TranslationService) {
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
            var serverError = this.translationService.translate("Common.ServerError");
            this.displayError(serverError);
            setTimeout(() => location.reload(), 5000);
            return;
        }

        super.handleError(zoneError);
    }

    private displayError(error: string): void {
        setTimeout(() => this.notificationsService.error("Ошибка", error)); // we use setTimeout for dom rerender.
    }
}