import { Component, OnInit, ViewChild } from '@angular/core';
import { RoleCC } from '../../models/role';
import { RoleComponent } from '../role/role.component';

@Component({
  selector: 'app-module-role',
  templateUrl: './module-role.component.html',
  styleUrls: ['./module-role.component.scss']
})
export class ModuleRoleComponent implements OnInit {

  @ViewChild('ccRole') ccRole: RoleComponent = {} as RoleComponent;

  constructor() { }


  ngOnInit(): void {
  }

  createNewRole() {
    this.ccRole.createNewRole();
  }
}
