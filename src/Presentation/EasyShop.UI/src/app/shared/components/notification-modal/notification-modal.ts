import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-notification-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notification-modal.html',
  styleUrl: './notification-modal.css',
})
export class NotificationModal {
  public notificationService = inject(NotificationService);

  public notificationServiceObj = this.notificationService.notificationObj;

  closeModal(): void {
    this.notificationService.hideNotification();
  }
}
