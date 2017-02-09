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
        setTimeout(() => this.notificationsService.error("Ошибка", error)); // we use setTimeout for dom rerender.
      });

      return;
    }

    super.handleError(zoneError);
  }
}