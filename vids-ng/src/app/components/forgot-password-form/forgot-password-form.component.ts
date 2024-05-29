import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TranslateService } from '@ngx-translate/core';
import { catchError, throwError } from 'rxjs';
import { AppService } from 'src/app/services/app.service';

@Component({
  selector: 'app-forgot-password-form',
  templateUrl: './forgot-password-form.component.html',
  styleUrls: ['./forgot-password-form.component.scss']
})
export class ForgotPasswordFormComponent implements OnInit {

  public form = new FormGroup({
    email: new FormControl('', [Validators.email, Validators.required])
  });

  constructor(public router: Router, public appService: AppService, public snackBar: MatSnackBar, public translate: TranslateService) { }

  ngOnInit(): void {
  }

  submitForm(){
    this.appService.sendResetPasswordEmail(this.form.value.email as string).pipe(
      catchError(error=>{
        this.snackBar.open(this.translate.instant('FORGOT_PASS_FORM.RESET_PASSWORD_ERROR'), '', {
          duration: 3000
        });
        return throwError(() => new Error(error.message));
      })
    ).subscribe((e)=>{
      this.router.navigateByUrl('reset-password-requested');
    });
  }

  cancelForm():void {
    this.router.navigateByUrl('/login');
  }

}
