import { AbstractControl, ValidationErrors } from "@angular/forms";

export const PasswordStrengthValidator = function(control: AbstractControl): ValidationErrors | null {
    let value: string = control.value || '';

    if (!value) {
      return null
    }

    let upperCaseCharacters = /[A-Z]+/g
    if (upperCaseCharacters.test(value) === false) {
        return { passwordStrength: 'uppercase_needed' };
    }

    let lowerCaseCharacters = /[a-z]+/g
    if (lowerCaseCharacters.test(value) === false) {
      return { passwordStrength: 'lowercase_needed' };
    }

    let numberCharacters = /[0-9]+/g
    if (numberCharacters.test(value) === false) {
      return { passwordStrength: 'number_needed' };
    }

    let specialCharacters = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/
    if (specialCharacters.test(value) === false) {
      return { passwordStrength: 'special_character_needed' };
    }

    if(value.length<8){
      return { passwordStrength: 'length_needed' };
    }

    return null;
}