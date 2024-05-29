import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-shimmer-form',
  templateUrl: './shimmer-form.component.html',
  styleUrls: ['./shimmer-form.component.scss']
})
export class ShimmerFormComponent implements OnInit {

  @Input()
  rowCount: number = 3;

  constructor() { }

  ngOnInit(): void {
  }

}
