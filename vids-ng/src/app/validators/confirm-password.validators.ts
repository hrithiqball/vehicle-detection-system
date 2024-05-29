import { FormGroup, ValidationErrors } from "@angular/forms";

export const ConfirmPasswordValidator = function(controlName: string, matchingControlName: string): ValidationErrors | null{
    return (formGroup: FormGroup)=>{
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];
      if(matchingControl.errors && !matchingControl.errors['confirmPasswordValidator']){
        return null; //skip this validator if got other error
      }
      if(control.value !== matchingControl.value){
        matchingControl.setErrors({confirmPassword: 'not-match'});
        return {confirmPassword:'not-match'};
      }else {
        return null;
      }
    }
  }
