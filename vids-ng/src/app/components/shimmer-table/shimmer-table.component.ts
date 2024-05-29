import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-shimmer-table',
  templateUrl: './shimmer-table.component.html',
  styleUrls: ['./shimmer-table.component.scss']
})
export class ShimmerTableComponent implements OnInit {

  @Input() columnWidth: string = '250px';
  @Input() columnHeight: string = '40px';
  @Input() columnCount: number = 2;
  @Input() rowCount: number = 2;

  constructor() { }

  ngOnInit(): void {
  }

}
