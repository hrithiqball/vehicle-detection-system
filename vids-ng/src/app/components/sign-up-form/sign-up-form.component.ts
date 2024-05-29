import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PasswordStrengthValidator } from 'src/app/validators/password-strenght.validators';
import { ConfirmPasswordValidator } from 'src/app/validators/confirm-password.validators';
import { UniqueEmailValidator } from 'src/app/validators/unique-email.validators';
import { AppService } from 'src/app/services/app.service';

@Component({
  selector: 'app-sign-up-form',
  templateUrl: './sign-up-form.component.html',
  styleUrls: ['./sign-up-form.component.scss']
})
export class SignUpFormComponent implements OnInit {

  form: FormGroup = new FormGroup({});

  constructor(private router: Router, private fb: FormBuilder, private appService: AppService, private uniqueCCEmailValidator: UniqueEmailValidator) {
    this.form = fb.group({
      name: ['', {validators: [Validators.required]}],
      email: ['', {validators: [Validators.required, Validators.email], asyncValidators: [uniqueCCEmailValidator], updateOn: 'blur'}], //asyn validator need to be injected and put after syn validator
      password: ['', {validators: [Validators.required, PasswordStrengthValidator]}],
      confirmPassword: ['', {validators: [Validators.required]}]
    }, { 
      validators: [ConfirmPasswordValidator('password', 'confirmPassword')]
    });

   }

  ngOnInit(): void {
  }

  submitForm(): void {
    this.appService.signUp(this.form.value.name as string, this.form.value.email as string, this.form.value.password as string).subscribe(()=>{
     //console.log('sign up success');
     this.router.navigateByUrl('/sign-up-success');
    });
  }

  cancelForm():void {
    this.router.navigateByUrl('/login');
  }

}

