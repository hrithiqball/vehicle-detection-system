import { Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'bold'
})
export class BoldPipe implements PipeTransform {

  transform(value: string, regex:string): any {
    return this.replace(value,regex);
  }

  replace(str: string, regex:string) {
    return str.replace(new RegExp(`(${regex})`, 'gi'), '<b>$1</b>');
  }

}
