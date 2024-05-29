import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { Subscription, finalize } from 'rxjs';
import { AppConfigService } from 'src/app/services/app-config.service';
import { AppService } from 'src/app/services/app.service';
import { TokenService } from 'src/app/services/token.service';

@Component({
  selector: 'app-update-avatar-dialog',
  templateUrl: './update-avatar-dialog.component.html',
  styleUrls: ['./update-avatar-dialog.component.scss']
})
export class UpdateAvatarDialogComponent implements OnInit {

  URL_UPLOAD_AVATAR = '/api/account/upload-avatar';
  URL_REMOVE_AVATAR = '/api/account/remove-avatar';

  fileName:string = '';
  uploadProgress: number | null = null;
  uploadSubcription: Subscription | null = null;

  MAX_FILE_SIZE_BYTE = 200000; //200kb

  @ViewChild('fileUpload') fileUpload: ElementRef | null = null;
  
  constructor(
    public appConfig: AppConfigService,
    public appService: AppService,
    public snackBar: MatSnackBar,
    public tokenService: TokenService,
    public translate: TranslateService,
    public http: HttpClient) {

    }

  ngOnInit(): void {
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      if(file.size < this.MAX_FILE_SIZE_BYTE){
        this.fileName = file.name;
        const formData = new FormData();
        formData.append("thumbnail", file);
  
        const url = this.appConfig.apiEndpoint + this.URL_UPLOAD_AVATAR;
        this.uploadSubcription = this.http.post(url, formData, {
          reportProgress: true,
          observe: 'events'
        }).pipe(
          finalize(() => this.reset())
        ).subscribe((e: HttpEvent<object>) => {
          if(e.type == HttpEventType.UploadProgress){
            if(e.total)
              this.uploadProgress = Math.round(100* (e.loaded / e.total));           
          }else if(e.type == HttpEventType.Response){
            if (e.body) {
              this.appService.avatar = e.body as unknown as string; //'data:image/png;base64,' + e.body;             
            }
          }
        });

      }else {
        this.snackBar.open(this.translate.instant('UPDATE_AVATAR_DIALOG.FILE_SIZE_TOO_LARGE'), '', {
          duration: 3000
        });

        if(this.fileUpload)
          this.fileUpload.nativeElement.value = '';
      }
    }
  }

  cancelUpload() {
    if(this.uploadSubcription)
      this.uploadSubcription.unsubscribe();
    this.reset();
  }

  reset() {
    this.uploadProgress = null;
    this.uploadSubcription = null;
  }

  removeAvatar(){
    const url = this.appConfig.apiEndpoint + this.URL_REMOVE_AVATAR;
    this.http.post(url, null).subscribe(()=>{
      this.appService.avatar = '';
    });
  }

}
