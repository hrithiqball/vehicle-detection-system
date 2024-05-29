import { HttpClient, HttpEvent, HttpEventType, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { catchError, map, Subscription, throwError, finalize } from 'rxjs';
import { AppConfigService } from 'src/app/services/app-config.service';
import { AppService } from 'src/app/services/app.service';
import { TokenService } from 'src/app/services/token.service';

@Component({
  selector: 'app-update-profile-dialog',
  templateUrl: './update-profile-dialog.component.html',
  styleUrls: ['./update-profile-dialog.component.scss']
})
export class UpdateProfileDialogComponent implements OnInit {

  URL_UPDATE_PROFILE = '/api/account/update-profile';

  form: FormGroup = new FormGroup({});
  isSaving: boolean = false;
 
  constructor(
    public fb: FormBuilder,
    public appConfig: AppConfigService,
    public http: HttpClient,
    public snackBar: MatSnackBar,
    public translate: TranslateService,
    public dialogRef: MatDialogRef<UpdateProfileDialogComponent>,
    public tokenService: TokenService
  ) {
    this.form = fb.group({
      name: [tokenService.getName(), { validators: [Validators.required] }],
      phoneNumber: [tokenService.getPhoneNumber()]
    });
  }

  ngOnInit(): void {
  }

  submitForm() {
    const url = this.appConfig.apiEndpoint + this.URL_UPDATE_PROFILE;
    const params = new HttpParams()
      .append('name', this.form.value.name)
      .append('phoneNumber', this.form.value.phoneNumber);

    this.isSaving = true;
    this.http.post(url, null, { params: params }).pipe(
      catchError((error) => {
        this.snackBar.open(this.translate.instant('UPDATE_PROFILE_DIALOG.UPDATE_PROFILE_ERROR'), '', {
          duration: 3000
        });
        return throwError(() => new Error(error.message));
      }),
      finalize(()=>{
        this.isSaving = false;
      })
    ).subscribe(() => {
      this.snackBar.open(this.translate.instant('UPDATE_PROFILE_DIALOG.UPDATE_PROFILE_SUCCESS'), '', {
        duration: 3000
      });
      this.dialogRef.close();
      this.tokenService.renewAccessToken().subscribe();
    });
  }

}
