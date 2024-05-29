import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-shimmer-list',
  templateUrl: './shimmer-list.component.html',
  styleUrls: ['./shimmer-list.component.scss']
})
export class ShimmerListComponent implements OnInit {

  @Input() itemCount: number = 8;
  @Input() width: string = '100%';
  @Input() height: string = '36px';

  constructor() { }

  ngOnInit(): void {
  }

}
