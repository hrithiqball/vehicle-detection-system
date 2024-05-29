import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { catchError, finalize, map, throwError } from 'rxjs';
import { AppConfigService } from 'src/app/services/app-config.service';
import { ConfirmPasswordValidator } from 'src/app/validators/confirm-password.validators';
import { PasswordStrengthValidator } from 'src/app/validators/password-strenght.validators';

@Component({
  selector: 'app-update-password-dialog',
  templateUrl: './update-password-dialog.component.html',
  styleUrls: ['./update-password-dialog.component.scss']
})
export class UpdatePasswordDialogComponent implements OnInit {

  form: FormGroup = new FormGroup({});

  URL_CHANGE_PASSWORD = '/api/account/update-password';

  isSaving: boolean = false

  constructor(
    public appConfig: AppConfigService,
    public fb: FormBuilder,
    public http: HttpClient,
    public snackBar: MatSnackBar,
    public translate: TranslateService,
    public dialogRef: MatDialogRef<UpdatePasswordDialogComponent>) {
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
    const url = this.appConfig.apiEndpoint + this.URL_CHANGE_PASSWORD;
    const params = new HttpParams()
      .append('password', this.form.value.password);

    this.isSaving = true;
    this.http.post(url, null, { params: params }).pipe(   
      catchError((error) => {
        this.snackBar.open(this.translate.instant('UPDATE_PASS_DIALOG.UPDATE_PASSWORD_ERROR'), '', {
          duration: 3000
        });
        return throwError(() => new Error(error.message));
      }),
      finalize(() => {
        this.isSaving = false;
      })
    ).subscribe(() => {
      this.snackBar.open(this.translate.instant('UPDATE_PASS_DIALOG.UPDATE_PASSWORD_SUCCESS'), '', {
        duration: 3000
      });
      this.dialogRef.close();
    });
  }
}
