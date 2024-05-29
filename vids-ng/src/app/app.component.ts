import { OverlayContainer } from '@angular/cdk/overlay';
import { Component } from '@angular/core';
import { AppService } from './services/app.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent {
  constructor(
    public appService: AppService,
    public overlayContainer: OverlayContainer) {

    appService.onThemeColorChanged().subscribe(() => {
      this.updateOverlayTheme();
    });

    appService.onThemeModeChanged().subscribe(() => {
      this.updateOverlayTheme();
    });

    this.updateOverlayTheme();

  }

  getClass() {
    return 'app-root' + " " + this.appService.currentTheme + " " + this.appService.currentMode;
  }

  updateOverlayTheme() {
    const overlayContainerElement = this.overlayContainer.getContainerElement();
    overlayContainerElement.className = 'cdk-overlay-container';
    overlayContainerElement.classList.add(this.appService.currentTheme, this.appService.currentMode);
  }
}
