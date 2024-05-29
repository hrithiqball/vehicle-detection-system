import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TranslateService}  from "@ngx-translate/core";
import { Router } from '@angular/router';
import { TokenService } from 'src/app/services/token.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, throwError } from 'rxjs';
import { AppService } from 'src/app/services/app.service';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss']
})
export class LoginFormComponent implements OnInit {

  form = new FormGroup({
    email: new FormControl('', [Validators.email, Validators.required]),
    password: new FormControl('', Validators.required)
  });

  languages = [
    {code: 'en', label: 'English'},
    {code: 'zh', label: '中文'},
    {code: 'ms', label: 'Bahasa Melayu'}
  ]

  currentLanguageCode = 'en';
  currentLanguageLabel = 'English'

  constructor(public translate: TranslateService, 
    public router: Router, 
    public tokenService: TokenService, 
    public appService: AppService,
    public snackBar: MatSnackBar) { 
    if(translate.currentLang){
      this.currentLanguageCode = translate.currentLang;
    }   
    this.currentLanguageLabel = this.languages.filter(t=>t.code === this.currentLanguageCode)[0].label;
    translate.setDefaultLang(this.currentLanguageCode);
  }

  ngOnInit(): void {
  }

  selectLanguage(code:string, label:string){
    this.currentLanguageCode = code;
    this.currentLanguageLabel = label;
    this.translate.use(this.currentLanguageCode);
  }

  submitForm():void {
    this.appService.signIn(this.form.value.email as string, this.form.value.password as string).pipe(
      catchError((error)=>{
        if(error.status === 401){
          this.snackBar.open(this.translate.instant('LOGIN_FORM.INVALID_EMAIL_OR_PASSWORD'), '', {
            duration: 3000
          });
        }else {
          this.snackBar.open(this.translate.instant('LOGIN_FORM.ERROR_OCCURED'), '', {
            duration: 3000
          });
        }
        return throwError(()=>error);
      })
    ).subscribe((token)=>{
      console.log(token);
      if(token){
        this.tokenService.setAccessToken(token.accessToken);
        this.tokenService.setRefreshToken(token.refreshToken);               
        console.log(this.tokenService.getEmail());         
        console.log(`access token expiry time: ${this.tokenService.getAccessTokenExpiryTime()}`);       
        console.log(`refresh token expiry time: ${this.tokenService.getRefreshTokenExpiryTime()}`);
        this.router.navigateByUrl('main');
      }
    });
  }

}
