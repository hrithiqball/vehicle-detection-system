import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { finalize, catchError, throwError } from 'rxjs';
import { User } from '../../models/user';
import { AppConfigService } from '../../services/app-config.service';
import { ConfirmPasswordValidator } from '../../validators/confirm-password.validators';
import { PasswordStrengthValidator } from '../../validators/password-strenght.validators';

@Component({
  selector: 'app-update-user-password-dialog',
  templateUrl: './update-user-password-dialog.component.html',
  styleUrls: ['./update-user-password-dialog.component.scss']
})
export class UpdateUserPasswordDialogComponent implements OnInit {

  URL_UPDATE_PASSWORD = '/api/user/update-password';

  form: FormGroup = new FormGroup({});
  isSaving: boolean = false;

  constructor(
    @Inject(MAT_DIALOG_DATA) public user: User,
    public appConfig: AppConfigService,
    public fb: FormBuilder,
    public http: HttpClient,
    public snackBar: MatSnackBar,
    public translate: TranslateService,
    public dialogRef: MatDialogRef<UpdateUserPasswordDialogComponent>
  ) {
    this.form = fb.group({
      password: ['', { validators: [Validators.required, PasswordStrengthValidator] }],
      confirmPassword: ['', { validators: [Validators.required] }]
    }, {
      validators: [ConfirmPasswordValidator('password', 'confirmPassword')]
    });
  }

  ngOnInit(): void {
  }

  submitForm() {
    const url = this.appConfig.apiEndpoint + this.URL_UPDATE_PASSWORD;
    const params = new HttpParams()
      .append('email', this.user.email)
      .append('password', this.form.value.password);

    this.isSaving = true;
    this.http.post(url, null, { params: params }).pipe(
      catchError((error) => {
        this.snackBar.open(this.translate.instant('UPDATE_USER_PASS_DIALOG.UPDATE_PASSWORD_ERROR'), '', {
          duration: 3000
        });
        return throwError(() => new Error(error.message));
      }),
      finalize(() => {
        this.isSaving = false;
      })
    ).subscribe(() => {
      this.snackBar.open(this.translate.instant('UPDATE_USER_PASS_DIALOG.UPDATE_PASSWORD_SUCCESS'), '', {
        duration: 3000
      });
      this.dialogRef.close();
    });
  }

}
