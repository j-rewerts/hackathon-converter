import { Component } from '@angular/core';
import { AppConfig } from './config/app.config';

@Component({
  selector: 'cv-root',
  // template: `
  //   <router-outlet></router-outlet>
  // `
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent {
  appConfig = AppConfig;
}
