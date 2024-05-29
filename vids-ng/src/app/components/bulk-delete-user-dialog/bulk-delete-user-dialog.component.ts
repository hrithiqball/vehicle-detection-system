import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { catchError, throwError, finalize } from 'rxjs';
import { BulkDeleteItem, Item } from '../../models/item';
import { User } from '../../models/user';
import { AppConfigService } from '../../services/app-config.service';

@Component({
  selector: 'app-bulk-delete-user-dialog',
  templateUrl: './bulk-delete-user-dialog.component.html',
  styleUrls: ['./bulk-delete-user-dialog.component.scss']
})
export class BulkDeleteUserDialogComponent implements OnInit {

  URL_BULK_DELETE_USER = '/api/user/bulk-delete';

  isSaving: boolean = false

  constructor(
    @Inject(MAT_DIALOG_DATA) public users: User[],
    public appConfig: AppConfigService,
    public http: HttpClient,
    public snackBar: MatSnackBar,
    public translate: TranslateService,
    public dialogRef: MatDialogRef<BulkDeleteUserDialogComponent>
  ) { }

  ngOnInit(): void {
  }

  confirm() {
    const url = this.appConfig.apiEndpoint + this.URL_BULK_DELETE_USER;

    const bulkDeleteItem: BulkDeleteItem = {
      itemList: []
    };

    for (let i = 0; i < this.users.length; i++) {
      const item: Item = {
        id: this.users[i].id,
        name: this.users[i].name
      };
      bulkDeleteItem.itemList.push(item);
    }
    
    this.isSaving = true;
    this.http.delete(url, { body: bulkDeleteItem }).pipe(
      catchError((error) => {
        this.snackBar.open(this.translate.instant('BULK_DELETE_USER_DIALOG.DELETE_ERROR'), '', {
          duration: 3000
        });
        return throwError(() => new Error(error.message));
      }),
      finalize(() => {
        this.isSaving = false;
      })
    ).subscribe(() => {
      this.snackBar.open(this.translate.instant('BULK_DELETE_USER_DIALOG.DELETE_SUCCESS'), '', {
        duration: 3000
      });
      this.dialogRef.close('deleted');
    })
  }

}
