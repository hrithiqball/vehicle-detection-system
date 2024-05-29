import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { catchError, throwError, finalize } from 'rxjs';
import { ConfirmPasswordValidator } from 'src/app/validators/confirm-password.validators';
import { PasswordStrengthValidator } from 'src/app/validators/password-strenght.validators';
import { UniqueEmailValidator } from 'src/app/validators/unique-email.validators';
import { Role } from '../../models/role';
import { AppConfigService } from '../../services/app-config.service';

@Component({
  selector: 'app-create-user-dialog',
  templateUrl: './create-user-dialog.component.html',
  styleUrls: ['./create-user-dialog.component.scss']
})
export class CreateUserDialogComponent implements OnInit {

  URL_CREATE_USER = '/api/user/create-user';
  URL_ROLE_LIST = '/api/role/cc/role-list';

  form: FormGroup = new FormGroup({});
  isSaving: boolean = false;
  roleList: Role[] = [];
  
  constructor(
    public appConfig: AppConfigService,
    public http: HttpClient,
    public fb: FormBuilder,
    public uniqueCCEmailValidator: UniqueEmailValidator,
    public snackBar: MatSnackBar,
    public translate: TranslateService,
    public dialogRef: MatDialogRef<CreateUserDialogComponent>
  ) { 
    this.form = fb.group({
      name: ['', { validators: [Validators.required] }],
      email: ['', { validators: [Validators.required, Validators.email], asyncValidators: [uniqueCCEmailValidator], updateOn: 'blur' }], //asyn validator need to be injected and put after syn validator
      password: ['', { validators: [Validators.required, PasswordStrengthValidator] }],
      confirmPassword: ['', { validators: [Validators.required] }],
      role: ['', { validators: [Validators.required] }],
      phoneNumber: ['']
    }, { 
      validators: [ConfirmPasswordValidator('password', 'confirmPassword')]
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
      if (this.roleList.length > 0) {
        this.form.controls['role'].setValue(this.roleList[0].roleName);
      }
    });
  }

  submitForm() {
    const url = this.appConfig.apiEndpoint + this.URL_CREATE_USER;
  
    const user = {
      email: this.form.value.email,
      name: this.form.value.name,
      role: this.form.value.role,
      password: this.form.value.password,
      phoneNumber: this.form.value.phoneNumber
    }

    this.isSaving = true;
    this.http.post(url, user).pipe(
      catchError((error) => {
        this.snackBar.open(this.translate.instant('CREATE_USER_DIALOG.SUBMIT_ERROR'), '', {
          duration: 3000
        });
        return throwError(() => new Error(error.message));
      }),
      finalize(() => {
        this.isSaving = false;
      })
    ).subscribe(() => {
      this.snackBar.open(this.translate.instant('CREATE_USER_DIALOG.SUBMIT_SUCCESS'), '', {
        duration: 3000
      });
      this.dialogRef.close('user-created');
    });
  }
}
