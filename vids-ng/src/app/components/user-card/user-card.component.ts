import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { catchError, Subject, throwError } from 'rxjs';
import { User } from '../../models/user';
import { AppConfigService } from '../../services/app-config.service';
import { DeleteUserDialogComponent } from '../delete-user-dialog/delete-user-dialog.component';
import { UpdateUserAvatarDialogComponent } from '../update-user-avatar-dialog/update-user-avatar-dialog.component';
import { UpdateUserInfoDialogComponent } from '../update-user-info-dialog/update-user-info-dialog.component';
import { UpdateUserPasswordDialogComponent } from '../update-user-password-dialog/update-user-password-dialog.component';
import { UserInfoDialogComponent } from '../user-info-dialog/user-info-dialog.component';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.scss']
})
export class UserCardComponent implements OnInit {

  URL_USER_INFO = '/api/user/info';
  URL_DELETE = '/api/user/delete';
  URL_UPDATE_STATUS = '/api/user/update-status';
  URL_UPDATE_PASSWORD = '/api/user/update-password';

  @Input() user: User = new User;


  @Output() itemDeleted: EventEmitter<string> = new EventEmitter<string>();
  @Output() itemClicked: EventEmitter<string> = new EventEmitter<string>();

 
  constructor(
    public appConfig: AppConfigService,
    public http: HttpClient,
    public dialog: MatDialog,
    public snackBar: MatSnackBar,
    public translate: TranslateService,
  ) { 
    this.user.avatar = '';
  }

  ngOnInit(): void {
  }

  onClicked() {
    this.itemClicked.emit(this.user.email);
    //this.user.selected = !this.user.selected;
  }

  activateUser() {
    console.log('activate user');
    const url = this.appConfig.apiEndpoint + this.URL_UPDATE_STATUS;

    const params = new HttpParams()
      .append('email', this.user.email)
      .append('isBlock', false);

    this.http.post(url, null, { params: params }).pipe(
      catchError((error) => {
        this.snackBar.open(this.translate.instant('USER_CARD.ACTIVATE_USER_ERROR'), '', {
          duration: 3000
        });
        return throwError(() => new Error(error.message));
      })
    ).subscribe(() => {
      this.snackBar.open(this.translate.instant('USER_CARD.ACTIVATE_USER_SUCCESS'), '', {
        duration: 3000
      });
      this.user.isBlocked = false;
    });

  }

  lockUser() {
    console.log('block user');
    const url = this.appConfig.apiEndpoint + this.URL_UPDATE_STATUS;

    const params = new HttpParams()
      .append('email', this.user.email)
      .append('isBlock', true);

    this.http.post(url, null, { params: params }).pipe(
      catchError((error) => {
        this.snackBar.open(this.translate.instant('USER_CARD.BLOCK_USER_ERROR'), '', {
          duration: 3000
        });
        return throwError(() => new Error(error.message));
      })
    ).subscribe(() => {
      this.snackBar.open(this.translate.instant('USER_CARD.BLOCK_USER_SUCCESS'), '', {
        duration: 3000
      });
      this.user.isBlocked = true;
    });
  }

  openUserInfoDialog() {
    this.dialog.open(UserInfoDialogComponent, { disableClose: false, data: this.user, width: '480px' });
  }

  openUpdateUserInfoDialog() {
    this.dialog.open(UpdateUserInfoDialogComponent, { disableClose: false, data: this.user, width: '480px' });
  }

  openUpdateUserPasswordDialog() {
    this.dialog.open(UpdateUserPasswordDialogComponent, { disableClose: false, data: this.user, width: '480px' });
  }

  openUpdateUserAvatarDialog() {
    this.dialog.open(UpdateUserAvatarDialogComponent, { disableClose: false, data: this.user, width: '480px' });
  }

  openDeleteUserDialog() {
    const dialogRef = this.dialog.open(DeleteUserDialogComponent, { disableClose: false, data: this.user, width: '480px' });
    dialogRef.afterClosed().subscribe((msg) => {
      console.log(msg);
      if (msg === 'deleted') {
        this.itemDeleted.emit(this.user.email);
      }
    });
  }

}
