import { Injectable, signal } from '@angular/core';

export interface Notification {
  title: string;
  message: string;
  type: 'success' | 'error' | 'warning';
  show: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  public notificationObj = signal<Notification>({
    title: '',
    message: '',
    type: 'success',
    show: false,
  });

  showErrorNotification(title: string, message: string) {
    this.notificationObj.set({
      title,
      message,
      type: 'error',
      show: true,
    });
  }

  showSuccessNotification(title: string, message: string) {
    this.notificationObj.set({
      title,
      message,
      type: 'success',
      show: true,
    });
  }

  // showWarningNotification(title: string, message: string) {
  //     this.notificationObj.set({
  //         title,
  //         message,
  //         type: 'warning',
  //         show: true
  //     });
  // }

  hideNotification() {
    this.notificationObj.update((notification) => ({ ...notification, show: false }));
  }
}
