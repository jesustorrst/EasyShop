import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { NotificationService } from '../services/notification.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const notificationService = inject(NotificationService);

  let title = 'Error';
  let message = 'An unexpected error occurred. Please try again later.';
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      switch (error.status) {
        case 400:
          title = 'Datos inválidos | Bad Request';
          message =
            error.error?.message ||
            'The request was invalid. Please check your inputs and try again.';
          break;
        case 401:
          title = 'Sesión expirada| Unauthorized';
          message = 'You are not authorized to perform this action. Please log in and try again.';
          break;
        case 403:
          title = 'Prohibido | Forbidden';
          message = 'You do not have permission to access this resource.';
          break;
        case 404:
          title = 'No encontrado | Not Found';
          message = 'The requested resource was not found.';
          break;
        case 500:
          title = 'Error del servidor | Internal Server Error';
          message = 'An error occurred on the server. Please try again later.';
          break;
      }

      notificationService.showErrorNotification(title, message);
      return throwError(() => error);
    }),
  );
};
