import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { catchError, throwError, finalize } from 'rxjs';
import { User } from '../../models/user';
import { AppConfigService } from '../../services/app-config.service';

@Component({
  selector: 'app-delete-user-dialog',
  templateUrl: './delete-user-dialog.component.html',
  styleUrls: ['./delete-user-dialog.component.scss']
})
export class DeleteUserDialogComponent implements OnInit {

  URL_DELETE_USER = '/api/user/delete';

  isSaving: boolean = false

  constructor(
    @Inject(MAT_DIALOG_DATA) public user: User,
    public appConfig: AppConfigService,
    public http: HttpClient,
    public snackBar: MatSnackBar,
    public translate: TranslateService,
    public dialogRef: MatDialogRef<DeleteUserDialogComponent>
  ) { }

  ngOnInit(): void {
  }

  confirm() {
    const url = this.appConfig.apiEndpoint + this.URL_DELETE_USER;
    const params = new HttpParams()
      .append('email', this.user.email);

    this.isSaving = true;
    this.http.delete(url, { params: params }).pipe(
      catchError((error) => {
        this.snackBar.open(this.translate.instant('DELETE_USER_DIALOG.DELETE_ERROR'), '', {
          duration: 3000
        });
        return throwError(() => new Error(error.message));
      }),
      finalize(() => {
        this.isSaving = false;
      })
    ).subscribe(() => {
      this.snackBar.open(this.translate.instant('DELETE_USER_DIALOG.DELETE_SUCCESS'), '', {
        duration: 3000
      });
      this.dialogRef.close('deleted');
    })
  }

}
