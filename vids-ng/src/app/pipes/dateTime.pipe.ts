import { Pipe, PipeTransform } from '@angular/core';
import * as dayjs from 'dayjs';

@Pipe({
  name: 'dateTime'
})
export class DateTimePipe implements PipeTransform {

  transform(value: string, arg: string = 'full'): any {
    if (value) {
      switch (arg) {
        case 'full':
        default:
          return dayjs(value).format('ddd, DD/MM/YYYY HH:mm:ss');
        case 'date-time':
          return dayjs(value).format('DD/MM/YYYY HH:mm:ss');
        case 'time':
          return dayjs(value).format('HH:mm:ss');
        case 'date':
          return dayjs(value).format('DD/MM/YYYY');
      }
    } else {
      return value;
    }
    
  }
}
