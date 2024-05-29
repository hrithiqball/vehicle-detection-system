import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, Inject, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { catchError, throwError, finalize } from 'rxjs';
import { Role } from '../../models/role';
import { User } from '../../models/user';
import { AppConfigService } from '../../services/app-config.service';

@Component({
  selector: 'app-update-user-info-dialog',
  templateUrl: './update-user-info-dialog.component.html',
  styleUrls: ['./update-user-info-dialog.component.scss']
})
export class UpdateUserInfoDialogComponent implements OnInit {

  URL_UPDATE_USER = '/api/user/update-user';
  URL_ROLE_LIST = '/api/role/cc/role-list';

  form: FormGroup = new FormGroup({});
  isSaving: boolean = false;
  roleList: Role[] = [];

  constructor(
    @Inject(MAT_DIALOG_DATA) public user: User,
    public fb: FormBuilder,
    public appConfig: AppConfigService,
    public http: HttpClient,
    public snackBar: MatSnackBar,
    public translate: TranslateService,
    public dialogRef: MatDialogRef<UpdateUserInfoDialogComponent>,
  ) {
    this.form = fb.group({
      name: [this.user.name, { validators: [Validators.required] }],
      role: [this.user.role, {validators: [Validators.required]}],
      phoneNumber: [this.user.phoneNumber]
    });
  }

  ngOnInit(): void {
    this.getRoleList();
  }

  getRoleList() {
    const url = this.appConfig.apiEndpoint + this.URL_ROLE_LIST;
    this.http.get<Role[]>(url).pipe(
      catchError((error) => {
        return throwError(() => new Error(error.message));
      })
    ).subscribe((res) => {
      this.roleList = res;
    });
  }

  submitForm() {
    const url = this.appConfig.apiEndpoint + this.URL_UPDATE_USER;
    const params = new HttpParams()
      .append('email', this.user.email)
      .append('name', this.form.value.name)
      .append('role', this.form.value.role)
      .append('phoneNumber', this.form.value.phoneNumber);

    this.isSaving = true;
    this.http.post(url, null, { params: params }).pipe(
      catchError((error) => {
        this.snackBar.open(this.translate.instant('UPDATE_USER_INFO_DIALOG.UPDATE_INFO_ERROR'), '', {
          duration: 3000
        });
        return throwError(() => new Error(error.message));
      }),
      finalize(() => {
        this.isSaving = false;
      })
    ).subscribe(() => {
      this.snackBar.open(this.translate.instant('UPDATE_USER_INFO_DIALOG.UPDATE_INFO_SUCCESS'), '', {
        duration: 3000
      });
      this.dialogRef.close();
      this.user.name = this.form.value.name;
      this.user.phoneNumber = this.form.value.phoneNumber;
      this.user.role = this.form.value.role;
    });
  }

}
