import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {

  private appConfig: any;
  private http: HttpClient;

  private appConfigLoadedSubject = new Subject<string>();

  constructor(http: HttpClient) {
    this.http = http;
  }

  loadAppConfig() {
    console.log('## load app config');
    this.http.get('/assets/config/app-setting.json').subscribe((config) => {
      this.appConfig = config; 
      this.appConfigLoadedSubject.next('loaded');
    })
  }

  onAppConfigLoaded(){
    return this.appConfigLoadedSubject;
  }

  get isLoaded(): boolean {
    if (this.appConfig)
      return true;
    else
      return false;
  }

  get apiEndpoint(): string {
    if(this.appConfig)
      return this.appConfig.apiEndpoint;
    else 
      return '';
  }

  get applicationName(): string {
    if (this.appConfig)
      return this.appConfig.applicationName;
    else
      return '';
  }
}
