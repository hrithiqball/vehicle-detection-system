import { HttpClient, HttpParams } from "@angular/common/http";
import { ElementRef, Inject, Injectable } from "@angular/core";
import { AbstractControl, AsyncValidator, ValidationErrors } from "@angular/forms";
import { catchError, map, Observable, of } from "rxjs";
import { AppConfigService } from "../services/app-config.service";

@Injectable({ providedIn: 'root' })
export class UniqueRoleValidator implements AsyncValidator {

  URL_IS_ROLE_NAME_TAKEN = '/api/role/is-role-name-taken';

  constructor(private appConfig: AppConfigService, private http: HttpClient) { }

  validate(control: AbstractControl): Observable<ValidationErrors | null> {
    let originalRoleName = '';
    if (control.parent)
      originalRoleName = control.parent.value.oriRoleName;

    if (control.value === originalRoleName) {
      return of(null);
    } else {
      return this.isRoleNameTaken(control.value).pipe(
        map(isTaken => (isTaken ? { roleTaken: true } : null)),
        catchError(() => of({ roleTaken: true }))
      );
    }
  }

  isRoleNameTaken(roleName: string) {
    const url = this.appConfig.apiEndpoint + this.URL_IS_ROLE_NAME_TAKEN;

    const params = new HttpParams()
     .append('roleName', roleName);

    return this.http.get(url, { params: params });
  }
}
