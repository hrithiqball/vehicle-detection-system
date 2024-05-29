import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { BehaviorSubject, catchError, Observable, of, throwError, filter, switchMap, take, map } from 'rxjs';
import { Router } from '@angular/router';
import { TokenService } from '../services/token.service';
import { AppService } from '../services/app.service';

@Injectable()
export class CustomHttpInterceptor implements HttpInterceptor {

  private renewTokenInProgress = false;
  private renewTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  constructor(private router: Router, private tokenService: TokenService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    if (this.tokenService.accessToken) {
      request = request.clone({ setHeaders: { Authorization: `Bearer ${this.tokenService.accessToken}` } });
    }

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {

        if (error.error instanceof ErrorEvent) {
          console.error(`Error: ${error.error.message}`);

        } else {
          console.error(`Error Code: ${error.status},  Message: ${error.message}`);

          switch (error.status) {
            case 401:
              if (request.url.includes('api/account/sign-in')){
                return throwError(()=> error);
              }
              else if (
                request.url.includes('api/account/renew-access-token') ||
                request.url.includes('api/account/renew-refresh-token')) {

                // we do not want to refresh token if error 401 receive in these api calls. Instead
                // sign out the user and redirect to login page.

                this.signOut();
                return throwError(() => error);
              }
              else {
                // for other api calls, try to get new access token by using refresh token.
                if (this.renewTokenInProgress) {
                  // if renewTokenProgres is true, wai until refreshTokenSubject has a non null value, which
                  // mean the new toke is ready.

                  return this.renewTokenSubject.pipe(
                    filter(result => result !== null),
                    take(1),
                    switchMap(() => next.handle(this.addAccessToken2Header(request)))
                  );

                } else {
                  return this.tokenService.renewAccessToken().pipe(
                    switchMap((token: any) => {
                      this.renewTokenInProgress = false;
                      this.renewTokenSubject.next(token);             
                      this.tokenService.setAccessToken(token);
                      return next.handle(this.addAccessToken2Header(request));
                    }),
                    catchError((error: any) => {
                      this.renewTokenInProgress = false;
                      this.signOut();
                      return throwError(() => new Error(error.message));
                    }));
                }
              }
            case 403:
              this.router.navigateByUrl("/error/unauthorized");
              break;
            case 404:
              if(!request.url.includes('assets')){
                this.router.navigateByUrl("/error/not-found");
              }
              break;
          }
        }

        return throwError(() => new Error(error.message));
      })
    )
  }

  addAccessToken2Header(request: any) {
    try {
      if (this.tokenService.accessToken) {
        return request.clone({ setHeaders: { Authorization: `Bearer ${this.tokenService.accessToken}` } });
      } else {
        return request;
      }

    } catch (error) {
      console.error(error);
    }
  }

  signOut(){
    this.tokenService.clearAccessToken();
    this.tokenService.clearRefreshToken();
    this.router.navigateByUrl("login");
  }

}
