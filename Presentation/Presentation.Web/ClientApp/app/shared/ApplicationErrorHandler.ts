import { ErrorHandler, Injectable } from "@angular/core";
import { NotificationsService } from "angular2-notifications";

@Injectable()
export class ApplicationErrorHandler implements ErrorHandler {
  constructor(private notificationsService: NotificationsService) { }

  handleError(zoneError: any): void {
    if(zoneError.rejection && Array.isArray(zoneError.rejection)) {
      var errors: any[] = zoneError.rejection;

      errors.forEach(error => {
        setTimeout(() => this.notificationsService.error("Ошибка", error)); // we use setTimeout for dom rerender.
      });

      return;
    }

    console.group( "ErrorHandler" );
    console.error( zoneError.message );
    console.error( zoneError.stack );
    console.groupEnd();
  }
}