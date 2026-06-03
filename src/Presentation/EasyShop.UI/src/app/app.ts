import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NotificationModal } from './shared/components/notification-modal/notification-modal';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NotificationModal],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('EasyShop.UI');
}
