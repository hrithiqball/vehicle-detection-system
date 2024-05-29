import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router, TitleStrategy } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, debounce, interval, map, Observable, Subject } from 'rxjs';
import { Token } from '../models/token';
import { AppConfigService } from './app-config.service';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class AppService {

  currentTheme: string;
  currentMode: string;
  currentLanguage: string;
  isSidebarOn: boolean;
  avatar:string = '';

  URL_SIGN_IN = '/api/account/sign-in';
  URL_SIGN_UP = '/api/account/sign-up';
  URL_SEND_RESET_PASSWORD_EMAIL = '/api/account/send-reset-password-email';
  URL_UPDATE_THEME_COLOR = '/api/account/update-theme-color';
  URL_UPDATE_THEME_MODE = '/api/account/update-theme-mode';
  URL_UPDATE_LANGUAGE = '/api/account/update-language';
  URL_GET_AVATAR = '/api/account/get-avatar';

  DEFAULT_THEME = 'theme-blue';
  APP_NAME = 'cc';

  private themeModeSubject = new Subject<string>();
  private themeColorSubject = new Subject<string>();
  private languageSubject = new Subject<string>();

  constructor(
    public appConfig: AppConfigService,
    public http: HttpClient,
    public router: Router, 
    public tokenService: TokenService,
    public translate: TranslateService) {

    this.currentTheme = tokenService.getThemeColor();
    if (this.currentTheme !== this.DEFAULT_THEME) {
      this.lazyLoadTheme(this.currentTheme);
    }

    this.currentMode = tokenService.getThemeMode();
    this.currentLanguage = tokenService.getLanguage();

    console.log('current theme: ' + this.currentTheme);
    console.log('current mode: ' + this.currentMode);
    console.log('current language: ' + this.currentLanguage);

    translate.use(this.currentLanguage).subscribe();

    this.isSidebarOn = true;

    this.themeModeSubject.pipe(
      debounce(()=>interval(1000))
    ).subscribe((themeMode:string)=>this.saveThemeMode(themeMode));

    this.themeColorSubject.pipe(
      debounce(()=>interval(1000))
    ).subscribe((themeColor:string)=>this.saveThemeColor(themeColor));

    this.languageSubject.pipe(
      debounce(()=>interval(1000))
    ).subscribe((language)=>this.saveLanguage(language));

    this.tokenService.onAccessTokenUpdated().subscribe((status)=>{
      if(status === 'first-load'){
        this.currentLanguage = tokenService.getLanguage();
        this.currentMode = tokenService.getThemeMode();
        this.currentTheme = tokenService.getThemeColor();

      }else if(status === 'cleared'){
        // reset theme and language to default
        this.currentLanguage = tokenService.getLanguage();
        this.currentMode = tokenService.getThemeMode();
        this.currentTheme = tokenService.getThemeColor();
      }
         
      console.log('current theme: ' + this.currentTheme);
      console.log('current mode: ' + this.currentMode);
      console.log('current language: ' + this.currentLanguage);
     
    })
  }

  signIn(email:string, password:string): Observable<Token>{
    const url = this.appConfig.apiEndpoint + this.URL_SIGN_IN;

    const params = new HttpParams()
    .append('app', this.APP_NAME)
    .append('email', email)
    .append('password', password);

    return this.http.post<Token>(url, null, {params: params});
  }

  signUp(name:string, email:string, password:string){
    const url = this.appConfig.apiEndpoint + this.URL_SIGN_UP;
    const params = new HttpParams()
    .append('app', this.APP_NAME)
    .append('name', name)
    .append('email',email)
    .append('password',password);

    return this.http.post(url, null, {params: params});
  }

  sendResetPasswordEmail(email:string){
    const url = this.appConfig.apiEndpoint + this.URL_SEND_RESET_PASSWORD_EMAIL;
    const params = new HttpParams()
    .append('app', this.APP_NAME)
    .append('email', email);

    return this.http.post(url, null, {params: params});
  }

  signOut(){
    this.tokenService.clearAccessToken();
    this.tokenService.clearRefreshToken();
    this.router.navigateByUrl("login"); 
  }


  onThemeColorChanged(){
    return this.themeColorSubject;
  }

  onThemeModeChanged(){
    return this.themeModeSubject;
  }

  updateThemeColor(themeColor: string) {
    console.log(`update theme color: ${themeColor}`);
    this.currentTheme = themeColor;
    if (this.currentTheme !== this.DEFAULT_THEME) {
      this.lazyLoadTheme(this.currentTheme);
    }
    this.themeColorSubject.next(this.currentTheme);
  }

  saveThemeColor(themeColor: string){
    console.log(`save theme color: ${themeColor}`);
    const url = this.appConfig.apiEndpoint + this.URL_UPDATE_THEME_COLOR;
    const params = new HttpParams()
      .append('themeColor', themeColor);

    this.http.post(url, null, { params: params }).subscribe(() => {
      this.tokenService.renewAccessToken().subscribe(); //renew access token to update the theme color inside.
    });
  }

  updateThemeMode(themeMode: string) {
    console.log(`update theme mode: ${themeMode}`);
    this.currentMode = themeMode;
    this.themeModeSubject.next(themeMode);
  }

  saveThemeMode(themeMode: string){
    console.log(`save theme mode: ${themeMode}`);
    const url = this.appConfig.apiEndpoint + this.URL_UPDATE_THEME_MODE;
    const params = new HttpParams()
      .append('themeMode', themeMode);

    this.http.post(url, null, { params: params }).subscribe(() => {
      this.tokenService.renewAccessToken().subscribe(); //renew access token to update the theme mode inside.
    });
  }

  updateLanguage(language: string) {
    console.log(`update language: ${language}`);
    this.currentLanguage = language;
    this.languageSubject.next(language);
  }

  saveLanguage(language:string){
    console.log(`save language: ${language}`);
    const url = this.appConfig.apiEndpoint + this.URL_UPDATE_LANGUAGE;
    const params = new HttpParams()
      .append('language', language);

    this.http.post(url, null, { params: params }).subscribe(() => {    
      this.tokenService.renewAccessToken().subscribe(); //renew access token to update the language inside.
    });
  }

  getAvatar(){
    if(this.appConfig.apiEndpoint){
      console.log('get avatar');
      const url = this.appConfig.apiEndpoint + this.URL_GET_AVATAR;
      this.http.get<string>(url).subscribe((base64)=>{
        if(base64!==''){
          this.avatar = base64; //'data:image/png;base64,' + base64;
        }else {
          this.avatar = '';
        }
      })
    }
  }

  lazyLoadTheme(theme: string) {
    const id = 'lazy_load_theme';
    const link = document.getElementById(id);

    if (!link) {
      const linkEl = document.createElement('link');
      linkEl.setAttribute('rel', 'stylesheet');
      linkEl.setAttribute('type', 'text/css');
      linkEl.setAttribute('id', id);
      linkEl.setAttribute('href', `/${theme}.css`);
      document.head.appendChild(linkEl);
    } else {
      (link as HTMLLinkElement).href = `${theme}.css`;
    }
  }
}
