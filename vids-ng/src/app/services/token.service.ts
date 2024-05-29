import { Injectable } from '@angular/core';
import jwt_decode from 'jwt-decode';
import * as dayjs from 'dayjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AppConfigService } from './app-config.service';
import { map, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  URL_RENEW_ACCESS_TOKEN = '/api/account/renew-access-token';
  URL_RENEW_REFRESH_TOKEN = '/api/account/renew-refresh-token';

  accessToken: string;
  decodedAccessToken: { [key: string]: string };

  refreshToken: string;
  decodedRefreshToken: { [key: string]: string };

  accessTokenSubject = new Subject<string>();
  refreshTokenSubject = new Subject<string>();

  isFirstLoaded: boolean = true;

  constructor(private appConfig: AppConfigService, private http: HttpClient) {
    this.accessToken = '';
    this.decodedAccessToken = {};
    this.refreshToken = '';
    this.decodedRefreshToken = {};
  }

  onAccessTokenUpdated(){
    return this.accessTokenSubject;
  }

  onRefreshTokenUpdated(){
    return this.refreshTokenSubject;
  }

  loadAccessToken() {
    let token = localStorage.getItem('access-token');

    console.log(`## load access token from local storage: ${token}`);

    if (token) {
      this.accessToken = token;
      this.decodedAccessToken = jwt_decode(this.accessToken);
      console.log(`access token expiry time: ${this.getAccessTokenExpiryTime()}`);
    }
  }

  loadRefreshToken() {
    let token = localStorage.getItem('refresh-token');

    console.log(`## load refresh token from local storage: ${token}`);

    if (token) {
      this.refreshToken = token;
      this.decodedRefreshToken = jwt_decode(this.refreshToken);
      console.log(`refresh token expiry time: ${this.getRefreshTokenExpiryTime()}`);
    }
  }

  setAccessToken(token: string) {
    if (token) {
      console.log('update access token');
      if(!this.accessToken){
        this.isFirstLoaded = true;
      }else {
        this.isFirstLoaded = false;
      }
      this.accessToken = token;
      this.decodedAccessToken = jwt_decode(this.accessToken);
      console.log(this.decodedAccessToken);
      localStorage.setItem('access-token', token);

      if(this.isFirstLoaded)
        this.accessTokenSubject.next('first-load');
    }
  }

  clearAccessToken() {
    this.accessToken = '';
    this.decodedAccessToken = {};
    localStorage.removeItem('access-token');
    this.accessTokenSubject.next('cleared');
  }

  setRefreshToken(token: string) {
    if (token) {
      console.log('update refresh token');
      this.refreshToken = token;
      this.decodedRefreshToken = jwt_decode(this.refreshToken);
      console.log(this.decodedRefreshToken);
      localStorage.setItem('refresh-token', token);
      this.refreshTokenSubject.next('renewed');
    }
  }

  clearRefreshToken() {
    this.refreshToken = '';
    this.decodedRefreshToken = {};
    localStorage.removeItem('refresh-token');
    this.refreshTokenSubject.next('cleared');
  }

  removeAccessToken() {
    this.accessToken = '';
    this.decodedAccessToken = {};
  }

  removeRefreshToken() {
    this.refreshToken = '';
    this.decodedRefreshToken = {};
  }

  getEmail() {
    const email = this.decodedAccessToken['email'];
    if (email)
      return email;
    else
      return '';
  }

  getThemeColor() {
    const themeColor = this.decodedAccessToken['theme-color'];
    if (themeColor) {
      return themeColor;
    } else {
      return 'theme-blue';
    }
  }

  getThemeMode() {
    const themeMode = this.decodedAccessToken['theme-mode'];
    if (themeMode)
      return themeMode;
    else
      return 'light-mode';
  }

  getLanguage() {
    const lang = this.decodedAccessToken['language'];
    if (lang)
      return lang;
    else
      return 'en';
  }

  getName() {
    const name = this.decodedAccessToken['name'];
    if (name)
      return name;
    else
      return '';
  }

  getPhoneNumber() {
    const phoneNumber = this.decodedAccessToken['phone-number'];
    if (phoneNumber)
      return phoneNumber;
    else
      return '';
  }

  getAccessTokenExpiryTime(raw: boolean = false) {
    if (raw) {
      return this.decodedAccessToken ? this.decodedAccessToken['exp'] : null; //the raw time is number of seconds sinc jan 1 1970
    } else {
      return this.decodedAccessToken ? dayjs(parseInt(this.decodedAccessToken['exp']) * 1000).format('DD/MM/YYYY HH:mm:ss') : null;
    }
  }

  getRefreshTokenExpiryTime(raw: boolean = false) {
    if (raw) {
      return this.decodedRefreshToken ? this.decodedRefreshToken['exp'] : null; //the raw time is number of seconds sinc jan 1 1970
    } else {
      return this.decodedRefreshToken ? dayjs(parseInt(this.decodedRefreshToken['exp']) * 1000).format('DD/MM/YYYY HH:mm:ss') : null;
    }
  }


  checkIsAccessTokenExpired() {
    const expiryTime = dayjs(parseInt(this.decodedAccessToken['exp']) * 1000);
    const currentTime = dayjs();
    if (expiryTime.diff(currentTime) > 0) {
      return false;
    } else {
      return true;
    }
  }

  checkIsRefreshTokenExpired() {
    const expiryTime = dayjs(parseInt(this.decodedRefreshToken['exp']) * 1000);
    const currentTime = dayjs();
    if (expiryTime.diff(currentTime) > 0) {
      return false;
    } else {
      return true;
    }
  }

  checkIsAuthenticated(): boolean {
    if (this.accessToken) {
      return true; //return true as long as has token. the actual authentication will be done when got api call. for ui just return true if has access token
    } else {
      return false;
    }
  }

  hasClaim(claim: string): boolean {
    if (this.accessToken) {
      let hasPermission = this.decodedAccessToken[claim] === 'true';
      return hasPermission;
    } else {
      return false;
    }
  }

  renewAccessToken() {
    console.log('renew access token');
    const url = this.appConfig.apiEndpoint + this.URL_RENEW_ACCESS_TOKEN;

    const params = new HttpParams()
      .append('refreshToken', this.refreshToken);

    return this.http.post(url, null, { params: params }).pipe(
      map((token: any) => {
        this.setAccessToken(token);
      })
    );
  }

  renewRefreshToken() {
    console.log('renew refresh token');
    const url = this.appConfig.apiEndpoint + this.URL_RENEW_REFRESH_TOKEN;

    const params = new HttpParams()
      .append('refreshToken', this.refreshToken);

    return this.http.post(url, null, { params: params });
  }


}
