import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Inject, OnInit, Output } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { catchError, throwError, finalize } from 'rxjs';
import { Role } from '../../models/role';
import { User, UserFilter } from '../../models/user';
import { AppConfigService } from '../../services/app-config.service';
import { DeleteUserDialogComponent } from '../delete-user-dialog/delete-user-dialog.component';

@Component({
  selector: 'app-role-users-dialog',
  templateUrl: './role-users-dialog.component.html',
  styleUrls: ['./role-users-dialog.component.scss']
})
export class RoleUsersDialogComponent implements OnInit {

  URL_USER_LIST = '/api/user/user-list';

  pageSize: number = 50;
  users: User[] = [];
  currentPageIndex: number = 0;
  isLoading: boolean = false;
  isEmpty: boolean = false;
  isEndOfData: boolean = false;

  constructor(
    @Inject(MAT_DIALOG_DATA) public role: Role,
    public dialog: MatDialog,
    public http: HttpClient,
    public snackBar: MatSnackBar,
    public translate: TranslateService,
    public appConfig: AppConfigService,
    public dialogRef: MatDialogRef<RoleUsersDialogComponent>
  ) { }

  ngOnInit(): void {
    this.loadUserList(this.currentPageIndex);
  }

  loadMore() {
    this.currentPageIndex += 1;
    this.loadUserList(this.currentPageIndex);
  }

  loadUserList(pageIndex: number) {
    const url = this.appConfig.apiEndpoint + this.URL_USER_LIST;

    const filter: UserFilter = {
      pageIndex: pageIndex,
      pageSize: this.pageSize,
      sortBy: 'name',
      ascSort: true,
      keyword: '',
      role: this.role.roleName,
      isBlocked: null
    }

    this.isLoading = true;
    return this.http.post<User[]>(url, filter).pipe(
      catchError((error) => {
        this.snackBar.open(this.translate.instant('ROLE_USERS_DIALOG.LOAD_DATA_ERROR'), '', {
          duration: 3000
        });
        return throwError(() => new Error(error.message));
      }),
      finalize(() => { this.isLoading = false; })
    ).subscribe((res) => {
      console.log('Page ' + pageIndex + ' - ' + res.length);

      if (pageIndex === 0) {
        this.users = [];
      }

      this.users.push(...res);

      if (this.users.length > 0)
        this.isEmpty = false;
      else
        this.isEmpty = true;

      if (res.length === this.pageSize) {
        this.currentPageIndex += 1;
        this.isEndOfData = false;
      } else {
        this.isEndOfData = true; //end of record
      }
    });
  }

  openDeleteUserDialog(user: User) {
    const tmpDialogRef = this.dialog.open(DeleteUserDialogComponent, { disableClose: false, data: user, width: '480px' });
    tmpDialogRef.afterClosed().subscribe((msg) => {
      console.log(msg);
      if (msg === 'deleted') {
        this.users = this.users.filter((el) => {
          return !user.email.match(el.email);
        });
        this.dialogRef.close('deleted');
      }
    });
  }
}
