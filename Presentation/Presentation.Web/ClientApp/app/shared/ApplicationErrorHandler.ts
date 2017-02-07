import { ErrorHandler } from "@angular/core";
import { NotificationsService } from "angular2-notifications";

export class ApplicationErrorHandler implements ErrorHandler {
  constructor(private notificationsService: NotificationsService) { }

  handleError(zoneError: any): void {
    if(zoneError.rejection) {
      var errors: any[] = zoneError.rejection;

      errors.forEach(error => {
        this.notificationsService.error("Ошибка", error);
      });

      // return;
    }

    // console.group( "ErrorHandler" );
    // console.error( zoneError.message );
    // console.error( zoneError.stack );
    // console.groupEnd();
  }
}