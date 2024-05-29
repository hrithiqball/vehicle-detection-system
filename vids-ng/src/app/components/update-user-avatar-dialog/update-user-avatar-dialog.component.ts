import { HttpClient, HttpEvent, HttpEventType, HttpParams } from '@angular/common/http';
import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { Subscription, finalize } from 'rxjs';
import { User } from '../../models/user';
import { AppConfigService } from '../../services/app-config.service';

@Component({
  selector: 'app-update-user-avatar-dialog',
  templateUrl: './update-user-avatar-dialog.component.html',
  styleUrls: ['./update-user-avatar-dialog.component.scss']
})
export class UpdateUserAvatarDialogComponent implements OnInit {

  URL_UPLOAD_AVATAR = '/api/user/upload-avatar';
  URL_REMOVE_AVATAR = '/api/user/remove-avatar';

  fileName: string = '';
  uploadProgress: number | null = null;
  uploadSubcription: Subscription | null = null;

  MAX_FILE_SIZE_BYTE = 200000; //200kb

  @ViewChild('fileUpload') fileUpload: ElementRef | null = null;

  constructor(
    @Inject(MAT_DIALOG_DATA) public user: User,
    public appConfig: AppConfigService,
    public snackBar: MatSnackBar,
    public translate: TranslateService,
    public http: HttpClient
  ) { }

  ngOnInit(): void {
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      if (file.size < this.MAX_FILE_SIZE_BYTE) {
        this.fileName = file.name;
        const formData = new FormData();
        formData.append("thumbnail", file);

        const params = new HttpParams()
          .append('email', this.user.email);

        const url = this.appConfig.apiEndpoint + this.URL_UPLOAD_AVATAR;
        this.uploadSubcription = this.http.post(url, formData, {
          reportProgress: true,
          observe: 'events',
          params: params
        }).pipe(
          finalize(() => this.reset())
        ).subscribe((e: HttpEvent<object>) => {
          if (e.type == HttpEventType.UploadProgress) {
            if (e.total)
              this.uploadProgress = Math.round(100 * (e.loaded / e.total));
          } else if (e.type == HttpEventType.Response) {
            if (e.body) {
              this.user.avatar = e.body as unknown as string; //'data:image/png;base64,' + e.body;             
            }
          }
        });

      } else {
        this.snackBar.open(this.translate.instant('UPDATE_USER_AVATAR_DIALOG.FILE_SIZE_TOO_LARGE'), '', {
          duration: 3000
        });

        if (this.fileUpload)
          this.fileUpload.nativeElement.value = '';
      }
    }
  }

  cancelUpload() {
    if (this.uploadSubcription)
      this.uploadSubcription.unsubscribe();
    this.reset();
  }

  reset() {
    this.uploadProgress = null;
    this.uploadSubcription = null;
  }

  removeAvatar() {
    const url = this.appConfig.apiEndpoint + this.URL_REMOVE_AVATAR;
    const params = new HttpParams()
      .append('email', this.user.email);

    this.http.delete(url, { params: params }).subscribe(() => {
      this.user.avatar = '';
    });
  }
}
