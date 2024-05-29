import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { catchError, throwError, finalize } from 'rxjs';
import { Role, RoleCC } from '../../models/role';
import { AppConfigService } from '../../services/app-config.service';

@Component({
  selector: 'app-delete-role-dialog',
  templateUrl: './delete-role-dialog.component.html',
  styleUrls: ['./delete-role-dialog.component.scss']
})
export class DeleteRoleDialogComponent implements OnInit {

  URL_DELETE_ROLE = '/api/role/cc/delete';

  isSaving: boolean = false

  constructor(
    @Inject(MAT_DIALOG_DATA) public role: Role,
    public appConfig: AppConfigService,
    public http: HttpClient,
    public snackBar: MatSnackBar,
    public translate: TranslateService,
    public dialogRef: MatDialogRef<DeleteRoleDialogComponent>
  ) {}

  ngOnInit(): void {
  }

  confirm() {
    const url = this.appConfig.apiEndpoint + this.URL_DELETE_ROLE;
    const params = new HttpParams()
      .append('roleName', this.role.roleName);

    this.isSaving = true;
    this.http.delete(url, { params: params }).pipe(
      catchError((error) => {
        this.snackBar.open(this.translate.instant('DELETE_ROLE_DIALOG.DELETE_ERROR'), '', {
          duration: 3000
        });
        return throwError(() => new Error(error.message));
      }),
      finalize(() => {
        this.isSaving = false;
      })
    ).subscribe(() => {
      this.snackBar.open(this.translate.instant('DELETE_ROLE_DIALOG.DELETE_SUCCESS'), '', {
        duration: 3000
      });
      this.dialogRef.close('deleted');
    })
  }
}
