import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AbstractControl, AsyncValidator, ValidationErrors } from "@angular/forms";
import { catchError, map, Observable, of } from "rxjs";
import { AppConfigService } from "../services/app-config.service";

@Injectable({ providedIn: 'root' })
export class UniqueEmailValidator implements AsyncValidator {

  URL_IS_EMAIL_TAKEN = '/api/account/is-email-taken';

  constructor(private appConfig: AppConfigService, private http: HttpClient) {}

  validate(control: AbstractControl): Observable<ValidationErrors | null> {
    return this.isEmailTaken(control.value).pipe(
      map(isTaken => (isTaken ? { emailTaken: true } : null)), 
      catchError(() => of({ emailTaken: true }))
    );
  }

  isEmailTaken(email: string) {
    const url = this.appConfig.apiEndpoint + this.URL_IS_EMAIL_TAKEN;

    const params = new HttpParams()
      .append('app', "recozense")
      .append('email', email);

    return this.http.get(url, { params: params });
  }
}
